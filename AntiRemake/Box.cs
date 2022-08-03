using OpenTK.Mathematics;

namespace AntiRemake;

public class Box
{
    public Vector3 Anchor { get; set; }
    public Vector3 Size { get; set; }
    public Vector3 Center => (Anchor + (Anchor + Size)) / 2;
    public Vector3 TopCenter => new Vector3((Anchor.X + (Anchor.X + Size.X)) / 2, Anchor.Y + Size.Y, (Anchor.Z + (Anchor.Z + Size.Z)) / 2);
    public float Top => Anchor.Y + Size.Y;

    public Box(Vector3 anchor, Vector3 size)
    {
        Anchor = anchor;
        Size = size;
    }

    public bool CollidesWith(Box other, out Side collisionSide)
    {
        // Compute an array of 8 corners, and check if any of them are
        // in the 'other' box.

        var corners = new Vector3[]
        {
            // Bottom corners
            Anchor,
            Anchor + new Vector3(Size.X, 0, 0),
            Anchor + new Vector3(0, 0, Size.Z),
            Anchor + new Vector3(Size.X, 0, Size.Z),

            // Top corners
            Anchor + new Vector3(0, Size.Y, 0),
            Anchor + new Vector3(Size.X, Size.Y, 0),
            Anchor + new Vector3(0, Size.Y, Size.Z),
            Anchor + Size
        };

        collisionSide = GetCollisionSide(other);

        return corners.Any(c => PositionInBox(c, other));
    }

    private bool PositionInBox(Vector3 position, Box box)
    {
        return position.X > box.Anchor.X && position.X < box.Anchor.X + box.Size.X &&
            position.Y > box.Anchor.Y && position.Y < box.Anchor.Y + box.Size.Y &&
            position.Z > box.Anchor.Z && position.Z < box.Anchor.Z + box.Size.Z;
    }

    private Side GetCollisionSide(Box other)
    {
        var right = Math.Abs(other.Anchor.X - (Anchor.X + Size.X));
        var left = Math.Abs((other.Anchor.X + other.Size.X) - Anchor.X);
        var top = Math.Abs(other.Anchor.Y - (Anchor.Y + Size.Y));
        var bottom = Math.Abs((other.Anchor.Y + other.Size.Y) - Anchor.Y);
        var back = Math.Abs(other.Anchor.Z - (Anchor.Z + Size.Z));
        var front = Math.Abs((other.Anchor.Z + other.Size.Z) - Anchor.Z);

        var smallest = new[] { right, left, top, bottom, back, front }.Min();

        if (smallest == right)
        {
            return Side.Right;
        }
        else if (smallest == left)
        {
            return Side.Left;
        }
        else if (smallest == top)
        {
            return Side.Top;
        }
        else if (smallest == bottom)
        {
            return Side.Bottom;
        }
        else if (smallest == back)
        {
            return Side.Back;
        }
        else
        {
            return Side.Front;
        }
    }
}
