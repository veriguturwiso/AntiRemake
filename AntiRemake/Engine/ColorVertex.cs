using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace AntiRemake.Engine;

[StructLayout(LayoutKind.Sequential)]
public struct ColorVertex
{
    public Vector3 Position;
    public Vector3 Color;
}
