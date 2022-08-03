using OpenTK.Mathematics;

namespace AntiRemake.Engine;

public class Camera
{
    public Vector3 Position { get; set; } = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 Front { get; set; } = new Vector3(0.0f, 0.0f, -1.0f);
    public Vector3 Up { get; set; }
    public Vector3 Right { get; set; }
    public Vector3 Forward => Vector3.Normalize(new Vector3
    {
        X = (float)(MathHelper.Cos(MathHelper.DegreesToRadians(Yaw)) * MathHelper.Cos(MathHelper.DegreesToRadians(Pitch))),
        Y = 0,
        Z = (float)(MathHelper.Sin(MathHelper.DegreesToRadians(Yaw)) * MathHelper.Cos(MathHelper.DegreesToRadians(Pitch)))
    });

    public float Yaw
    {
        get { return yaw; }
        set { yaw = value; UpdateCameraVectors(); }
    }
    public float Pitch
    {
        get { return pitch; }
        set { pitch = MathHelper.Clamp(value, -89.9f, 89.9f); UpdateCameraVectors(); }
    }

    public float MovementSpeed { get; set; } = 1.0f;
    public float MouseSensitivity { get; set; } = 1.0f;
    public float Fov
    {
        get { return fov; }
        set { fov = MathHelper.Clamp(value, 0.1f, 179.9f); }
    }

    public Matrix4 ViewMatrix => Matrix4.LookAt(Position, Position + Front, Up);

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float fov = 70.0f;

    public Camera()
    {
        UpdateCameraVectors();
    }

    private void UpdateCameraVectors()
    {
        Front = Vector3.Normalize(new Vector3
        {
            X = (float)(MathHelper.Cos(MathHelper.DegreesToRadians(Yaw)) * MathHelper.Cos(MathHelper.DegreesToRadians(Pitch))),
            Y = (float)MathHelper.Sin(MathHelper.DegreesToRadians(Pitch)),
            Z = (float)(MathHelper.Sin(MathHelper.DegreesToRadians(Yaw)) * MathHelper.Cos(MathHelper.DegreesToRadians(Pitch)))
        });
        Right = Vector3.Normalize(Vector3.Cross(Front, new Vector3(0, 1, 0)));
        Up = Vector3.Normalize(Vector3.Cross(Right, Front));
    }
}
