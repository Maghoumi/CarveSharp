/**
 * CarveSharp, .NET Wrapper for Carve's CSG and mesh boolean operations
 * Copyright (C) 2015  Mehran Maghoumi (https://www.maghoumi.com)
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CodeFull.Graphics;
using OpenTK;
using System.Collections.Concurrent;
using CodeFull.Graphics3D;

namespace CodeFull.CarveSharp
{
    /// <summary>
    /// Contains the methods for performing CSG operations using Carve
    /// </summary>
    public static class CarveSharp
    {
        /// <summary>
        /// Enum of all the operations that can be performed on
        /// two meshes using CSG.
        /// </summary>
        public enum CSGOperations
        {
            /// <summary>
            /// In A or in B.
            /// </summary>
            Union,

            /// <summary>
            /// In A and in B.
            /// </summary>
            Intersection,

            /// <summary>
            /// In A, but not in B.
            /// </summary>
            AMinusB,

            /// <summary>
            /// In B, but not in A.
            /// </summary>
            BMinusA,

            /// <summary>
            /// In A or B, but not in both.
            /// </summary>
            SymmetricDifferent,

            /// <summary>
            /// All split faces from A and B.
            /// </summary>
            All
        }

        /// <summary>
        /// Defines the low-level structure that the DLL wrapper uses
        /// to represent a triangular mesh.
        /// </summary>
        private unsafe struct InteropMesh
        {
            /// <summary>
            /// The array containing the vertices
            /// </summary>
            public double* vertices;

            /// <summary>
            /// The array containing the triangle indices
            /// </summary>
            public int* triangleIndices;

            /// <summary>
            /// The number of elements in the vertices array
            /// </summary>
            public int vertsArrayLength;

            /// <summary>
            /// The number of elements in the triangle array
            /// </summary>
            public int triArrayLength;
        }

        /// <summary>
        /// The DLL entry definition for performing CSG operations.
        /// </summary>
        /// <param name="a">The first mesh</param>
        /// <param name="b">The second mesh</param>
        /// <param name="op">The operation that should be performed on the meshes</param>
        /// <returns>The resulted mesh</returns>
        [DllImport("CarveLibWrapper.dll")]
        private static unsafe extern InteropMesh* performCSG(ref InteropMesh a, ref InteropMesh b, CSGOperations op);

        /// <summary>
        /// The DLL entry definition for freeing the memory after a CSG operation.
        /// </summary>
        /// <param name="a"></param>
        [DllImport("CarveLibWrapper.dll")]
        private static unsafe extern void freeMesh(InteropMesh* a);

        /// <summary>
        /// Performs the specified operation on the provided meshes.
        /// </summary>
        /// <param name="first">The first mesh</param>
        /// <param name="second">The second mesh</param>
        /// <param name="operation">The mesh opration to perform on the two meshes</param>
        /// <returns>A triangular mesh resulting from performing the specified operation. If the resulting mesh is empty, will return null.</returns>
        public static Mesh PerformCSG(Mesh first, Mesh second, CSGOperations operation)
        {
            Mesh finalResult = null;

            unsafe
            {
                InteropMesh a = new InteropMesh();
                InteropMesh b = new InteropMesh();

                InteropMesh* result;

                fixed (Vector3d* aVerts = &first.GetTransformedVertices()[0], bVerts = &second.GetTransformedVertices()[0])
                {
                    fixed (int* aTris = &first.TriangleIndices[0], bTris = &second.TriangleIndices[0])
                    {
                        a.vertsArrayLength = first.Vertices.Length * 3;
                        a.triArrayLength = first.TriangleIndices.Length;
                        a.vertices = (double*)aVerts;
                        a.triangleIndices = aTris;

                        b.vertsArrayLength = second.Vertices.Length * 3;
                        b.triArrayLength = second.TriangleIndices.Length;
                        b.vertices = (double*)bVerts;
                        b.triangleIndices = bTris;

                        try
                        {
                            result = performCSG(ref a, ref b, operation);
                        }
                        catch (SEHException ex)
                        {
                            ArgumentException e = new ArgumentException("Carve has thrown an error. Possible reason is corrupt or self-intersecting meshes", ex);
                            throw e;
                        }
                    }
                }

                if (result->vertsArrayLength == 0)
                {
                    freeMesh(result);
                    return null;
                }

                Vector3d[] vertices = new Vector3d[result->vertsArrayLength / 3];
                int[] triangleIndices = new int[result->triArrayLength];

                // Copy the results back in parallel
                Parallel.For(0, vertices.Length, i =>
                {
                    vertices[i] = new Vector3d(result->vertices[3 * i], result->vertices[3 * i + 1], result->vertices[3 * i + 2]);
                });

                Parallel.For(0, triangleIndices.Length, i =>
                {
                    triangleIndices[i] = result->triangleIndices[i];
                });

                // If none of the vertices had colors, return whatever we have
                if (!first.HasColor && !second.HasColor)
                {
                    finalResult = new Mesh(vertices, triangleIndices);
                    freeMesh(result);
                }
                else    // Assign colors to the resulting mesh
                {
                    uint[] colors = new uint[vertices.Length];
                    uint grayCode = 4286611584; // The uint value of the color gray (representing no color)

                    // Assign the default gray to all vertices
                    Parallel.For(0, colors.Length, i => 
                    {
                        colors[i] = grayCode;
                    });

                    #region Worst practices of parallel coding
                    /**
                     * The procedure for color matching is creating a map of (vertex=>color) and then
                     * comparing all vertices of the resulting mesh (in parallel) with this map and
                     * assigning colors as necessary
                     */
                    if (first.HasColor)
                    {
                        ConcurrentDictionary<Vector3d, uint> firstMap = new ConcurrentDictionary<Vector3d, uint>();
                        var fVerts = first.GetTransformedVertices();

                        // Create vertex to color map
                        Parallel.For(0, fVerts.Length, i =>
                        {
                            firstMap[fVerts[i]] = first.Colors[i];
                        });

                        // Assign colors
                        Parallel.For(0, vertices.Length, i =>
                        {
                            if (firstMap.ContainsKey(vertices[i]))
                                colors[i] = firstMap[vertices[i]];
                        });
                    }

                    if (second.HasColor)
                    {
                        ConcurrentDictionary<Vector3d, uint> secondMap = new ConcurrentDictionary<Vector3d, uint>();
                        var sVerts = second.GetTransformedVertices();

                        Parallel.For(0, sVerts.Length, i =>
                        {
                            secondMap[sVerts[i]] = second.Colors[i];
                        });

                        Parallel.For(0, vertices.Length, i =>
                        {
                            if (secondMap.ContainsKey(vertices[i]))
                                colors[i] = secondMap[vertices[i]];
                        });
                    } 
                    #endregion

                    finalResult = new Mesh(vertices, triangleIndices, colors);
                }
            }   // end-unsafe

            return finalResult;
        }
    }
}
