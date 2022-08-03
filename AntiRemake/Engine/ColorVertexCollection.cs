using System.Collections.ObjectModel;

namespace AntiRemake.Engine;

public class ColorVertexCollection : Collection<ColorVertex>
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
                output.Add(vertex.Color.X);
                output.Add(vertex.Color.Y);
                output.Add(vertex.Color.Z);
            }

            return output;
        }
    }
}
