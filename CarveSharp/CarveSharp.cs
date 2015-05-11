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

namespace CodeFull.CarveSharp
{
    public static class CarveSharp
    {
        /// <summary>
        /// Enum of all the operations that can be performed on
        /// two meshes using CSG.
        /// </summary>
        public enum CSGOperations
        {
            Union,                  /** in a or b. */
            Intersection,           /** in a and b. */
            AMinusB,              /** in a, but not b. */
            BMinusA,              /** in b, but not a. */
            SymmetricDifferent,   /** in a or b, but not both. */
            All                     /** all split faces from a and b */
        }

        /// <summary>
        /// Defines the low-level structure that the DLL wrapper uses
        /// to represent a triangular mesh.
        /// </summary>
        private unsafe struct InteropMesh
        {
            public double* vertices;
            public int* triangleIndices;
            public int numVertices;
            public int numTriangles;
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
        /// <returns>A triangular mesh resulting from performing the specified operation</returns>
        public static TriangularMesh PerformCSG(TriangularMesh first, TriangularMesh second, CSGOperations operation)
        {
            TriangularMesh finalResult = new TriangularMesh();

            unsafe
            {
                InteropMesh a = new InteropMesh();
                InteropMesh b = new InteropMesh();

                InteropMesh* result;

                fixed (double* aVerts = &first.vertices.ToArray()[0], bVerts = &second.vertices.ToArray()[0])
                {
                    fixed (int* aTris = &first.triangleIndices.ToArray()[0], bTris = &second.triangleIndices.ToArray()[0])
                    {
                        a.numVertices = first.vertices.Count;
                        a.numTriangles = first.triangleIndices.Count;
                        a.vertices = aVerts;
                        a.triangleIndices = aTris;

                        b.numVertices = second.vertices.Count;
                        b.numTriangles = second.triangleIndices.Count;
                        b.vertices = bVerts;
                        b.triangleIndices = bTris;

                        try
                        {
                            result = performCSG(ref a, ref b, operation);
                        }
                        catch (SEHException ex)
                        {
                            ArgumentException e = new ArgumentException("Carve has thrown an error. Possible reason is corrupt or self-intersecting meshes", ex);
                            throw ex;
                        }
                    }
                }

                // TODO use parallel copy?
                for (int i = 0; i < result->numVertices; i++)
                    finalResult.AddVertex(result->vertices[i]);

                for (int i = 0; i < result->numTriangles; i++)
                    finalResult.AddTriangleIndex(result->triangleIndices[i]);

                freeMesh(result);
            }   // end-unsafe

            return finalResult;
        }
    }

    /// <summary>
    /// Defines a simple representation for a triangular mesh.
    /// A triangular mesh can be represented using the vertices
    /// and its triangle indices.
    /// </summary>
    public class TriangularMesh
    {
        /// <summary>
        /// Flattened array of vertices. Every 3 elements denote (x, y, z) point
        /// </summary>
        public List<double> vertices = new List<double>();

        /// <summary>
        /// Array of triangle indices
        /// </summary>
        public List<int> triangleIndices = new List<int>();

        /// <summary>
        /// Gets the number of elements in the flattened vertex array
        /// </summary>
        public int NumVertices
        {
            get
            {
                return vertices.Count;
            }
        }

        /// <summary>
        /// Gets the number of elements in the array of triange indices
        /// </summary>
        public int NumTriangleIndices
        {
            get
            {
                return triangleIndices.Count;
            }
        }

        /// <summary>
        /// Add a vertex to the end of flattened array of vertices
        /// </summary>
        /// <param name="vertex">The vertex to add</param>
        public void AddVertex(double vertex)
        {
            this.vertices.Add(vertex);
        }

        /// <summary>
        /// Add an index to the end of the triangle indices array
        /// </summary>
        /// <param name="index">The index to add</param>
        public void AddTriangleIndex(int index)
        {
            this.triangleIndices.Add(index);
        }
    }
}
