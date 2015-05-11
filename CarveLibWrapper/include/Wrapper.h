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
#ifndef WRAPPER_H
#define WRAPPER_H

#include <cstdlib>
#include "carve/csg.hpp"
#include "carve/csg_triangulator.hpp"

struct InteropMesh {
	double *vertices;
	int *triangleIndices;
	int numVertices;
	int numTriangles;

	void free() {
		delete[] vertices;
		delete[] triangleIndices;
	}
};

/**
 * The mesh operations that can be performed.
 */
enum Operation {
	UNION,                  /** in a or b. */
	INTERSECTION,           /** in a and b. */
	A_MINUS_B,              /** in a, but not b. */
	B_MINUS_A,              /** in b, but not a. */
	SYMMETRIC_DIFFERENCE,   /** in a or b, but not both. */
	ALL                     /** all split faces from a and b */
};

extern "C" __declspec(dllexport) InteropMesh* performCSG(InteropMesh* a, InteropMesh* b, Operation op);

/**
 * Frees a previously returned results. The users are responsible for invoking
 * this method after a mesh operation to avoid memory leaks.
 */
extern "C" __declspec(dllexport) void freeMesh(InteropMesh* mesh);

/**
 * Converts the flattened array of vertices and triangle indices to Carve's MeshSet format
 */
carve::mesh::MeshSet<3>* getMesh(InteropMesh* mesh);

/**
 * Processes a Carve's mesh and extracts the triangle faces. The properties
 * are then set on the provided MeshResult instance. The non-triangular faces
 * are also broken into triangular faces.
 */
void setTriangleProperties(InteropMesh* output, carve::mesh::MeshSet<3>* mesh);

/**
 * Flattens the vertices of Carve's mesh and sets them on the specified MeshResult
 * instance.
 */
void setVertexProperties(InteropMesh* output, carve::mesh::MeshSet<3>* mesh);

#endif // !WRAPPER_H