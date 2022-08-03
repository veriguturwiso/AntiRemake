using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AntiRemake.Engine;

public class Mesh
{
    public VertexCollection Vertices { get; }
    public List<int> Indices { get; }
    public List<Texture> Textures { get; }
    public Color4 ColorDiffuse { get; }

    private int _vbo, _vao, _ebo;

    public Mesh(VertexCollection vertices, IEnumerable<int> indices, IEnumerable<Texture> textures, Color4 diffuseColor)
    {
        Vertices = vertices;
        Indices = indices.ToList();
        Textures = textures.ToList();
        ColorDiffuse = diffuseColor;

        var vertexSize = Unsafe.SizeOf<Vertex>();

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

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>(nameof(Vertex.Normal)));
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>(nameof(Vertex.TexCoords)));
        GL.EnableVertexAttribArray(2);

        // When there is no texture, nothing will render, so we need to add
        // a small white 1x1 texture.
        if (Textures.Count == 0)
        {
            Textures.Add(new Texture(1, 1, Color4.White, "texture_diffuse"));
        }
    }

    public void Render(Shader shader)
    {
        shader.SetVector4("colorDiffuse", new Vector4(ColorDiffuse.R, ColorDiffuse.G, ColorDiffuse.B, ColorDiffuse.A));
        
        var diffuseNum = 1;
        var specularNum = 1;

        for (int i = 0; i < Textures.Count; i++)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + i);

            int number = 0;
            string name = Textures[i].Type;

            if (name == "texture_diffuse")
            {
                number = diffuseNum++;
            }
            else if (name == "texture_specular")
            {
                number = specularNum++;
            }

            shader.SetInt($"{name}{number}", i);
            GL.BindTexture(TextureTarget.Texture2D, Textures[i].Id);
        }

        GL.ActiveTexture(TextureUnit.Texture0);

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

        GL.DrawElements(PrimitiveType.Triangles, Indices.Count(), DrawElementsType.UnsignedInt, 0);
    }
}
