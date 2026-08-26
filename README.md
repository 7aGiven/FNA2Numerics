# FNA2Numerics

A Project that migrate FNA game to System.Numerics.Vectors

Methods in Vector2Extension, Vector3Extension, Vector4Extension, PlaneExtension, QuaternionExtension and Matrix4x4Extension are used for overide FNA implement.

If a method have implemented by System.Numerics, will directly call.

Then if a method implement in *Extension.cs, will call method in *Extension.cs

Then if not above, it will call method that implemented by FNA

Usage:
Do both command
```
FNA2Numerics.exe FNA.dll
FNA2Numerics.exe game.exe
```