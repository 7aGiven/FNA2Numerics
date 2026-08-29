# FNA2Numerics
A Project that migrate FNA game to System.Numerics.Vectors

# Usage
### For dotnet project with Visual Studio
  Create a local folder and add it as nuget source

  Put FNA2NumericsTask.0.2.0.nupkg into the folder

  Install nupkg with Visual Studio
### For dotnet project with command line
  Enter main project folder

  e.g. nupkg file locate at C:\nuget_source\FNA2NumericsTask.0.2.0.nupkg

  Add nupkg: `dotnet add package FNA2NumericsTask -s "C:\nuget_source\"`

  Then build: `dotnet build`

  If want to remove the nupkg: `dotnet remove package FNA2NumericsTask`

### For only exe and dll without project
  Do both command
```
FNA2NumericsCLI.exe FNA.dll
FNA2NumericsCLI.exe game.exe
```

# Detail
Methods in Vector2Extension, Vector3Extension, Vector4Extension, PlaneExtension, QuaternionExtension and Matrix4x4Extension are used for override FNA implemention.

If a method have implemented by System.Numerics, will directly call.

Then if a method implement in *Extension.cs, will call method in *Extension.cs

Then if not above, it will call method that implemented by FNA