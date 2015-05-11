CarveSharp
==========

CarveSharp is a .NET wrapper for the fast and robust constructive solid geometry (CSG) library [Carve](https://code.google.com/p/carve/). Using CarveSharp, you could easily pass triangular meshes and perform boolean operations on them (such as union, intersect, etc.).

CarveSharp was release on the same day its development started! So beware that bugs are expected. Although the source code is well-documented, examples will be included in the near future.

## Usage
Build the solution using the desired configuration (Debug/Release). Binary files will be located in "bin" folder. Simply include the "CarveSharp.dll" file in your .NET solution and use the static method "PerformCSG" in the CarveSharp class to perform boolean operations on meshes.

## Acknowledgement
All credits go to the original developer of [Carve] (https://code.google.com/p/carve/).
Carve's fork [sybren-carve] (https://code.google.com/r/sybren-carve/) was used for building VS 2013 binaries.