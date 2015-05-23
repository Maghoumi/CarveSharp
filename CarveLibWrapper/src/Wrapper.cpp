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

#include "Wrapper.h"

using namespace carve::csg;
using namespace carve::mesh;
using namespace std;

/**
 * Converts the flattened array of vertices and triangle indices to Carve's MeshSet format
 */
carve::mesh::MeshSet<3>* getMesh(InteropMesh* mesh) {
	//double* vertices, int vertCount, int* triangleIndices, int triCount
	vector<carve::geom3d::Vector> verts;
	vector<int> faces;

	// Copy the vertices of the mesh
	for (int i = 0; i < mesh->numVertices; i+=3)
		verts.push_back(carve::geom::VECTOR(mesh->vertices[i], mesh->vertices[i + 1], mesh->vertices[i + 2]));

	// Copy the faces of the mesh
	for (int i = 0, j = 0; i < mesh->numTriangles; i++, j++)
	{
		if (j % 3 == 0) {
			faces.push_back(3);
			j = 0;
		}

		faces.push_back(mesh->triangleIndices[i]);
	}

	// Create the mesh
	return new carve::mesh::MeshSet<3>(verts, mesh->numTriangles / 3, faces);
}

void setTriangleProperties(InteropMesh* output, MeshSet<3>* mesh) {
	std::vector<const carve::mesh::MeshSet<3>::face_t *> faces;
	std::copy(mesh->faceBegin(), mesh->faceEnd(), std::back_inserter(faces));

	// Convert faces to indices
	// It's a bit tricky and requires pointer arithmetic's help
	// Also, careful because Carve can generate non-triangular faces

	vector<int> resultTris;

	for (int i = 0; i < faces.size(); i++) {
		const carve::mesh::MeshSet<3>::face_t *currentFace = faces[i];
		int nVertices = currentFace->nVertices();
		carve::mesh::MeshSet<3>::vertex_t* baseIndex = &currentFace->mesh->meshset->vertex_storage[0];

		carve::mesh::MeshSet<3>::face_t::const_edge_iter_t iter = currentFace->begin();
		const carve::mesh::MeshSet<3>::vertex_t *v = iter->vert;
		int startIdx = v - baseIndex; // the triangle fan start index
		iter++;

		int midIdx = iter->vert - baseIndex;
		iter++;
		int endIdx = iter->vert - baseIndex;
		
		// n-poly can be divided into n-2 triangles
		// Convert polygonal faces to triangular faces
		for (int i = 0; i < nVertices - 2; i++)
		{
			resultTris.push_back(startIdx);
			resultTris.push_back(midIdx);
			resultTris.push_back(endIdx);

			midIdx = endIdx;
			iter++;
			endIdx = iter->vert - baseIndex;
		}
	}

	// Copy results to result array using memcpy
	output->numTriangles = resultTris.size();
	output->triangleIndices = new int[output->numTriangles];
	memcpy(output->triangleIndices, &resultTris[0], output->numTriangles * sizeof(int));
}

void setVertexProperties(InteropMesh* output, MeshSet<3>* mesh) {
	output->numVertices = mesh->vertex_storage.size() * 3;
	output->vertices = new double[output->numVertices];

	for (int i = 0; i < mesh->vertex_storage.size(); i++)
	{
		carve::geom::vector<3> v = mesh->vertex_storage[i].v;
		output->vertices[3 * i] = v.x;
		output->vertices[3 * i + 1] = v.y;
		output->vertices[3 * i + 2] = v.z;
	}
}

InteropMesh* performCSG(InteropMesh* a, InteropMesh* b, Operation op) {
	MeshSet<3>* first = getMesh(a);
	MeshSet<3>* second = getMesh(b);

	CSG csg;

	MeshSet<3> *result = csg.compute(first, second, (CSG::OP)op);

	delete first;
	delete second;

	InteropMesh* meshResult = new InteropMesh;

	if (result->vertex_storage.size() != 0) {
		setVertexProperties(meshResult, result);
		setTriangleProperties(meshResult, result);
	}
	else {
		meshResult->vertices = NULL;
		meshResult->triangleIndices = NULL;
		meshResult->numVertices = 0;
		meshResult->numTriangles = 0;
	}

	delete result;

	return meshResult;

}

void freeMesh(InteropMesh* mesh) {
	mesh->free();
}