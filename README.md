CarveSharp
==========

CarveSharp is a .NET wrapper for the fast and robust constructive solid geometry (CSG) library [Carve](https://code.google.com/p/carve/). Using CarveSharp, you could easily pass triangular meshes and perform boolean operations on them (such as union, intersect, etc.). CarveSharp is targeted for .NET v4 and above (due to the use of parallel for loops for increased performance). It can be easily integrated into Unity by rewriting all Parallel.For loops as regular C# for loops (note that the performance may significantly decrease).

CarveSharp is work-in-progress. Beware that bugs may exist and are expected.

## Usage
Build the solution using the desired configuration (Debug/Release). Binary files will be located in "bin" folder. Simply include the "CarveSharp.dll" file in your .NET solution and use the static method "PerformCSG" in the CarveSharp class to perform boolean operations on meshes.

## Included Example
The latest version of CarveSharp includes an example project. Simply run the example project, load two meshes and experiment with them.
Here's an example output screenshot:  

Before:
![Screenshot of an example](https://www.maghoumi.com/wp-content/uploads/2015/08/2.png)

After:
![Screenshot of an example](https://www.maghoumi.com/wp-content/uploads/2015/08/4.png)

## Acknowledgement
All credits go to the original developer of [Carve] (https://code.google.com/p/carve/).    
Carve's fork [sybren-carve] (https://code.google.com/r/sybren-carve/) was used for building VS 2013 binaries.  
CarveSharp depends on the [OpenTK] (http://www.opentk.org) and [CodeFullToolkit] (https://github.com/Maghoumi/CodeFullToolkit) libraries.