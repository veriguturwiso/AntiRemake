using AntiRemake.Engine;
using OpenTK.Mathematics;

namespace AntiRemake;

public class ColorGameObject
{
    public List<ColorGameObject> Children { get; set; } = new List<ColorGameObject>();
    public ColorMesh? Mesh { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Scale { get; set; } = new Vector3(1.0f);
    public Box? Box { get; set; }

    public ColorGameObject(ColorMesh? mesh, Box? box)
    {
        Mesh = mesh;
        Box = box;
    }

    public ColorGameObject()
    {
        Mesh = null;
        Box = null;
    }

    public void Render(Shader shader)
    {
        shader.SetMatrix4("model", Matrix4.CreateScale(Scale) * Matrix4.CreateTranslation(Position));

        Mesh?.Render();
    }

    /// <param name="position">Position to add to original for this render only.</param>
    /// <param name="scale">Scale to multiply with original for this render only.</param>
    public void Render(Shader shader, Vector3 position, Vector3 scale)
    {
        shader.SetMatrix4("model", Matrix4.CreateScale(scale * Scale) * Matrix4.CreateTranslation(Position + position));

        Mesh?.Render();
    }
}
