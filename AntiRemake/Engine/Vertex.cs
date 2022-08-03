using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace AntiRemake.Engine;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Vector2 TexCoords;
}
