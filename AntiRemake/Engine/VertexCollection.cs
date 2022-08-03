using System.Collections.ObjectModel;

namespace AntiRemake.Engine;

public class VertexCollection : Collection<Vertex>
{
    public List<float> ListOfFloats
    {
        get
        {
            var output = new List<float>();

            foreach (var vertex in this)
            {
                output.Add(vertex.Position.X);
                output.Add(vertex.Position.Y);
                output.Add(vertex.Position.Z);
                output.Add(vertex.Normal.X);
                output.Add(vertex.Normal.Y);
                output.Add(vertex.Normal.Z);
                output.Add(vertex.TexCoords.X);
                output.Add(vertex.TexCoords.Y);
            }

            return output;
        }
    }
}
