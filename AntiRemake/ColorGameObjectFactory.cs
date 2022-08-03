using AntiRemake.Engine;
using OpenTK.Mathematics;

namespace AntiRemake;

public static class ColorGameObjectFactory
{
    public static ColorGameObject CreateRectangle(Vector3 position, Origin origin, Vector3 size, Color4 color)
    {
        return new ColorGameObject(
            new ColorMesh(
                new ColorVertexCollection
                {
                    // Front face
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },

                    // Top face
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },

                    // Back face
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },

                    // Bottom face
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },

                    // Left face
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },

                    // Right face
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) }
                },
                new List<int>
                {
                    0, 1, 2,
                    2, 3, 0,
                    4, 5, 6,
                    6, 7, 4,
                    8, 9, 10,
                    10, 11, 8,
                    12, 13, 14,
                    14, 15, 12,
                    16, 17, 18,
                    18, 19, 16,
                    20, 21, 22,
                    22, 23, 20
                }
            ),
            new Box(origin == Origin.Center ? position - size / 2 : position, size)
        )
        {
            Position = origin == Origin.Center ? position : position + size / 2,
            Scale = size
        };
    }

    public static ColorGameObject CreateOutlinedRectangle(Vector3 position, Origin origin, Vector3 size, Color4 color, float outlineThickness, Color4 outlineColor)
    {
        var halfThickness = outlineThickness / 2;
        var halfSize = size / 2;
        var originedPosition = origin == Origin.Center ? position - size / 2 : position;

        return new ColorGameObject(
            new ColorMesh(
                new ColorVertexCollection
                {
                    // Front face
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },

                    // Top face
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },

                    // Back face
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },

                    // Bottom face
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },

                    // Left face
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },

                    // Right face
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Color = new Vector3(color.R, color.G, color.B) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Color = new Vector3(color.R, color.G, color.B) }
                },
                new List<int>
                {
                    0, 1, 2,
                    2, 3, 0,
                    4, 5, 6,
                    6, 7, 4,
                    8, 9, 10,
                    10, 11, 8,
                    12, 13, 14,
                    14, 15, 12,
                    16, 17, 18,
                    18, 19, 16,
                    20, 21, 22,
                    22, 23, 20
                }
            ),
            new Box(origin == Origin.Center ? position - size / 2 : position, size)
        )
        {
            Position = origin == Origin.Center ? position : position + size / 2,
            Scale = size,
            Children = new List<ColorGameObject>
            {
                // Bottom left
                CreateRectangle(new Vector3(originedPosition.X - halfThickness, originedPosition.Y - halfThickness, originedPosition.Z - halfThickness), Origin.BottomBackLeft, new Vector3(outlineThickness, outlineThickness, size.Z), outlineColor),

                // Bottom right
                CreateRectangle(new Vector3(originedPosition.X + size.X - halfThickness, originedPosition.Y - halfThickness, originedPosition.Z - halfThickness), Origin.BottomBackLeft, new Vector3(outlineThickness, outlineThickness, size.Z), outlineColor),

                // Bottom front
                CreateRectangle(new Vector3(originedPosition.X - halfThickness, originedPosition.Y - halfThickness, originedPosition.Z + size.Z - halfThickness), Origin.BottomBackLeft, new Vector3(size.X, outlineThickness, outlineThickness), outlineColor),

                // Bottom back
                CreateRectangle(new Vector3(originedPosition.X - halfThickness, originedPosition.Y - halfThickness, originedPosition.Z - halfThickness), Origin.BottomBackLeft, new Vector3(size.X, outlineThickness, outlineThickness), outlineColor),

                // Back left
                CreateRectangle(new Vector3(originedPosition.X - halfThickness, originedPosition.Y, originedPosition.Z - halfThickness), Origin.BottomBackLeft, new Vector3(outlineThickness, size.Y, outlineThickness), outlineColor),

                // Back right
                CreateRectangle(new Vector3(originedPosition.X + size.X - halfThickness, originedPosition.Y, originedPosition.Z - halfThickness), Origin.BottomBackLeft, new Vector3(outlineThickness, size.Y, outlineThickness), outlineColor),

                // Front left
                CreateRectangle(new Vector3(originedPosition.X - halfThickness, originedPosition.Y, originedPosition.Z + size.Z - halfThickness), Origin.BottomBackLeft, new Vector3(outlineThickness, size.Y, outlineThickness), outlineColor),

                // Front right
                CreateRectangle(new Vector3(originedPosition.X - halfThickness + size.X, originedPosition.Y, originedPosition.Z + size.Z - halfThickness), Origin.BottomBackLeft, new Vector3(outlineThickness, size.Y, outlineThickness), outlineColor),

                // Top left
                CreateRectangle(new Vector3(originedPosition.X - halfThickness, originedPosition.Y + size.Y - halfThickness, originedPosition.Z - halfThickness), Origin.BottomBackLeft, new Vector3(outlineThickness, outlineThickness, size.Z), outlineColor),

                // Top right
                CreateRectangle(new Vector3(originedPosition.X + size.X - halfThickness, originedPosition.Y + size.Y - halfThickness, originedPosition.Z - halfThickness), Origin.BottomBackLeft, new Vector3(outlineThickness, outlineThickness, size.Z), outlineColor),

                // Top front
                CreateRectangle(new Vector3(originedPosition.X - halfThickness, originedPosition.Y + size.Y - halfThickness, originedPosition.Z + size.Z - halfThickness), Origin.BottomBackLeft, new Vector3(size.X, outlineThickness, outlineThickness), outlineColor),

                // Top back
                CreateRectangle(new Vector3(originedPosition.X - halfThickness, originedPosition.Y + size.Y - halfThickness, originedPosition.Z - halfThickness), Origin.BottomBackLeft, new Vector3(size.X, outlineThickness, outlineThickness), outlineColor)
            }
        };
    }

    public static ColorGameObject CreateBox(Vector3 position, Origin origin, Vector3 size, float wallThickness, float outlineThickness, Func<Side, bool>? shouldDeleteSide = null)
    {
        var originedPosition = origin == Origin.Center ? position - size / 2 : position;

        var box = new ColorGameObject
        {
            Position = origin == Origin.Center ? position : position + size / 2,
            Children = new List<ColorGameObject>
            {
                // Left
                CreateOutlinedRectangle(new Vector3(originedPosition.X - wallThickness, originedPosition.Y, originedPosition.Z), Origin.BottomBackLeft, new Vector3(wallThickness, size.Y, size.Z), Color4.White, outlineThickness, Color4.Black),

                // Right
                CreateOutlinedRectangle(new Vector3(originedPosition.X + size.X, originedPosition.Y, originedPosition.Z), Origin.BottomBackLeft, new Vector3(wallThickness, size.Y, size.Z), Color4.White, outlineThickness, Color4.Black),

                // Top
                CreateOutlinedRectangle(new Vector3(originedPosition.X, originedPosition.Y + size.Y, originedPosition.Z), Origin.BottomBackLeft, new Vector3(size.X, wallThickness, size.Z), Color4.White, outlineThickness, Color4.Black),

                // Bottom
                CreateOutlinedRectangle(new Vector3(originedPosition.X, originedPosition.Y - wallThickness, originedPosition.Z), Origin.BottomBackLeft, new Vector3(size.X, wallThickness, size.Z), Color4.White, outlineThickness, Color4.Black),

                // Front
                CreateOutlinedRectangle(new Vector3(originedPosition.X, originedPosition.Y, originedPosition.Z + size.Z), Origin.BottomBackLeft, new Vector3(size.X, size.Y, wallThickness), Color4.White, outlineThickness, Color4.Black),

                // Back
                CreateOutlinedRectangle(new Vector3(originedPosition.X, originedPosition.Y, originedPosition.Z - wallThickness), Origin.BottomBackLeft, new Vector3(size.X, size.Y, wallThickness), Color4.White, outlineThickness, Color4.Black)
            }
        };

        if (shouldDeleteSide is null)
            return box;

        foreach (var side in Enum.GetValues<Side>().Cast<Side>())
        {
            if (shouldDeleteSide(side))
            {
                box.Children[(int)side] = null!;
            }
        }

        box.Children = box.Children.Where(c => c is not null).ToList();

        return box;
    }
}
