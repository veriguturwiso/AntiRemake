using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;

namespace AntiRemake.Engine;

public class Texture
{
    public string? Path { get; }
    public int Id { get; }
    public string Type { get; }

    public Texture(string path, string type)
    {
        Path = path;
        Type = type;

        using (var image = Image.Load<Rgba32>(path))
        {
            var data = new List<byte>(image.Width * image.Height * 4);

            image.Mutate(x => x.Flip(FlipMode.Vertical));

            for (int y = 0; y < image.Height; y++)
            {
                image.ProcessPixelRows(r =>
                {
                    var pixels = r.GetRowSpan(y);

                    foreach (var pixel in pixels)
                    {
                        data.Add(pixel.R);
                        data.Add(pixel.G);
                        data.Add(pixel.B);
                        data.Add(pixel.A);
                    }
                });
            }

            Id = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, Id);

            // Setting parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            // Setting texture
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data.ToArray());
        }
    }

    public Texture(int width, int height, Color4 color, string type)
    {
        Type = type;

        var data = new List<float>(width * height * 4);

        for (int i = 0; i < width * height; i++)
        {
            data.Add(color.R);
            data.Add(color.G);
            data.Add(color.B);
            data.Add(color.A);
        }

        Id = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, Id);

        // Setting parameters
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        // Setting texture
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.Float, data.ToArray());
    }

    public Texture(int width, int height, float[] data, string type)
    {
        System.Diagnostics.Debug.Assert(width * height * 4 == data.Length);

        Type = type;

        Id = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, Id);

        // Setting parameters
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        // Setting texture
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.Float, data);
    }

    public static Texture Outlined(int width, int height, Color4 color, int lineThickness, Color4 lineColor)
    {
        List<float> data = new List<float>(width * height * 4);

        var currentRow = 0;

        for (int i = 0; i < width * height; i++)
        {
            if (currentRow != i / width)
                currentRow++;

            var currentX = i - width * currentRow;

            if (currentRow <= lineThickness || currentX <= lineThickness || currentX >= width - lineThickness || currentRow >= height - lineThickness)
            {
                data.Add(lineColor.R);
                data.Add(lineColor.G);
                data.Add(lineColor.B);
                data.Add(lineColor.A);

                continue;
            }

            data.Add(color.R);
            data.Add(color.G);
            data.Add(color.B);
            data.Add(color.A);
        }

        return new Texture(width, height, data.ToArray(), "texture_diffuse");
    }
}
