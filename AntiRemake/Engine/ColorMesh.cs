using OpenTK.Graphics.OpenGL;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AntiRemake.Engine;

public class ColorMesh
{
    public ColorVertexCollection Vertices { get; }
    public List<int> Indices { get; }

    private int _vbo, _vao, _ebo;

    public ColorMesh(ColorVertexCollection vertices, IEnumerable<int> indices)
    {
        Vertices = vertices;
        Indices = indices.ToList();

        var vertexSize = Unsafe.SizeOf<ColorVertex>();

        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

        GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * vertexSize, Vertices.ListOfFloats.ToArray(), BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Count * sizeof(int), indices.ToArray(), BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSize, 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<ColorVertex>(nameof(ColorVertex.Color)));
        GL.EnableVertexAttribArray(1);
    }

    public void Render()
    {
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, Indices.Count, DrawElementsType.UnsignedInt, 0);
    }
}
