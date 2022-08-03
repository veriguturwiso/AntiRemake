using AntiRemake.Engine;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace AntiRemake;

public class Main : GameWindow
{
    /*
     * WARNING!
     * Colliding with walls doesn't work because I was too lazy to implement it :3
     */
    const float outlineThickness = 0.007f;
    const float insidePortalOutlineThickness = 0.003f;

    const float gravity = 5f;

    // Objects
    ColorGameObject level;
    ColorGameObject portal1;
    ColorGameObject portal2;
    ColorGameObject fakeCorridor1;
    ColorGameObject fakeCorridor2;
    ColorGameObject fakeCorridorWrapper;
    ColorGameObject fakeLevel1;
    ColorGameObject fakeLevel2;
    ColorGameObject actualCorridor;
    ColorGameObject actualLevel1;
    ColorGameObject actualLevel2;
    ColorGameObject actualTrigger1;
    ColorGameObject actualTrigger2;
    ColorGameObject smallRedCube;

    Vector3 portalNormal = new Vector3(0, 0, 1);
    Box player = new Box(new Vector3(0, 1, 0), new Vector3(0.0006f, 0.5f, 0.0006f));
    float velocity = 0f;

    // Scene stuff
    Shader defaultShader;
    Camera camera = new() { Yaw = -90 };
    Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.Fov), (float)Size.X / Size.Y, 0.001f, 100f);

    public Main() : base(
        GameWindowSettings.Default,
        new NativeWindowSettings
        {
            Size = new Vector2i(1280, 720),
            WindowState = WindowState.Maximized,
            NumberOfSamples = 4
        }
    )
    {
        CursorState = CursorState.Grabbed;

        defaultShader = new Shader(
            "Shaders/default.vert",
            "Shaders/default.frag"
        );

        smallRedCube = ColorGameObjectFactory.CreateRectangle(Vector3.Zero, Origin.Center, new Vector3(0.05f), Color4.Red);

        level = ColorGameObjectFactory.CreateBox(new Vector3(0, 4.5f, -3), Origin.Center, new Vector3(10, 10, 10), 0.5f, outlineThickness);

        fakeCorridor1 = new ColorGameObject()
        {
            Children = new List<ColorGameObject>
            {
                // Bottom
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3(-0.5f, -0.5f, -13f), Origin.BottomBackLeft, new Vector3(1f, 0f, 10f), Color4.White, insidePortalOutlineThickness, Color4.Black),

                // Left
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3(-0.5f, -0.5f, -13f), Origin.BottomBackLeft, new Vector3(0f, 1f, 10f), Color4.White, insidePortalOutlineThickness, Color4.Black),

                // Top
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3(-0.5f,  0.5f, -13f), Origin.BottomBackLeft, new Vector3(1f, 0f, 10f), Color4.White, insidePortalOutlineThickness, Color4.Black),

                // Right
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3( 0.5f, -0.5f, -13f), Origin.BottomBackLeft, new Vector3(0f, 1f, 10f), Color4.White, insidePortalOutlineThickness, Color4.Black),
            }
        };

        fakeCorridor2 = new ColorGameObject()
        {
            Children = new List<ColorGameObject>
            {
                // Bottom
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3(-0.5f, -0.5f, -4f), Origin.BottomBackLeft, new Vector3(1f, 0f, 10f), Color4.White, insidePortalOutlineThickness, Color4.Black),

                // Left
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3(-0.5f, -0.5f, -4f), Origin.BottomBackLeft, new Vector3(0f, 1f, 10f), Color4.White, insidePortalOutlineThickness, Color4.Black),

                // Top
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3(-0.5f,  0.5f, -4f), Origin.BottomBackLeft, new Vector3(1f, 0f, 10f), Color4.White, insidePortalOutlineThickness, Color4.Black),

                // Right
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3( 0.5f, -0.5f, -4f), Origin.BottomBackLeft, new Vector3(0f, 1f, 10f), Color4.White, insidePortalOutlineThickness, Color4.Black),
            }
        };

        fakeLevel1 = ColorGameObjectFactory.CreateBox(new Vector3(0, 4.5f, -14), Origin.Center, new Vector3(10, 10, 10), 0.0f, insidePortalOutlineThickness);

        fakeLevel2 = ColorGameObjectFactory.CreateBox(new Vector3(0, 4.5f, 6), Origin.Center, new Vector3(10, 10, 10), 0.0f, insidePortalOutlineThickness, s => s == Side.Back);

        fakeCorridorWrapper = new ColorGameObject()
        {
            Children = new List<ColorGameObject>
            {
                // Left
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3(-0.5f, -0.5f, -4f), Origin.BottomBackLeft, new Vector3(0f, 1f, 1f), Color4.White, outlineThickness, Color4.Black),

                // Top
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3(-0.5f,  0.5f, -4f), Origin.BottomBackLeft, new Vector3(1f, 0f, 1f), Color4.White, outlineThickness, Color4.Black),

                // Right
                ColorGameObjectFactory.CreateOutlinedRectangle(new Vector3( 0.5f, -0.5f, -4f), Origin.BottomBackLeft, new Vector3(0f, 1f, 1f), Color4.White, outlineThickness, Color4.Black),
            }
        };

        actualCorridor = ColorGameObjectFactory.CreateBox(new Vector3(10, 30, 4), Origin.Center, new Vector3(1, 1, 10), 0.5f, outlineThickness, s => s == Side.Front || s == Side.Back);

        actualLevel1 = ColorGameObjectFactory.CreateBox(new Vector3(10, 34.5f, -3.5f), Origin.Center, new Vector3(10, 10, 5), 0.5f, outlineThickness, s => s == Side.Front);

        actualLevel2 = ColorGameObjectFactory.CreateBox(new Vector3(10, 34.5f,  11.5f), Origin.Center, new Vector3(10, 10, 5), 0.5f, outlineThickness, s => s == Side.Back);

        actualTrigger1 = new ColorGameObject(
            null,
            new Box(
                //                                  3 is floor (yep thats bad design but i will refactor it in the future probably)
                new Vector3(actualCorridor.Children[3].Box!.Anchor.X, actualCorridor.Children[3].Box!.Anchor.Y, actualCorridor.Children[3].Box!.Anchor.Z + actualCorridor.Children[3].Box!.Size.Z), new Vector3(1f, 1f, 0.01f)
            )
        );

        actualTrigger2 = new ColorGameObject(
            null,
            new Box(
                //                      3 is floor (yep thats bad design but i will refactor it in the future probably)
                actualCorridor.Children[3].Box!.Anchor, new Vector3(1f, 1f, 0.01f)
            )
        );

        portal1 = new ColorGameObject(
            new ColorMesh(
                new ColorVertexCollection
                {
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f,  0.0f), Color = new Vector3(0.0f, 0.0f, 0.0f) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f,  0.0f), Color = new Vector3(0.0f, 0.0f, 0.0f) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f,  0.0f), Color = new Vector3(0.0f, 0.0f, 0.0f) },
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f,  0.0f), Color = new Vector3(0.0f, 0.0f, 0.0f) },
                },
                new List<int>
                {
                    0, 1, 2,
                    2, 3, 0
                }
            ),
            new Box(
                new Vector3(-0.5f, -0.5f, -3.5f),
                new Vector3(1f, 1f, 0.5f)
            )
        )
        {
            Position = new Vector3(0, 0, -3)
        };

        portal2 = new ColorGameObject(
            new ColorMesh(
                new ColorVertexCollection
                {
                    new ColorVertex { Position = new Vector3(-0.5f,  0.5f,  0.0f), Color = new Vector3(0.0f, 0.0f, 0.0f) },
                    new ColorVertex { Position = new Vector3( 0.5f,  0.5f,  0.0f), Color = new Vector3(0.0f, 0.0f, 0.0f) },
                    new ColorVertex { Position = new Vector3( 0.5f, -0.5f,  0.0f), Color = new Vector3(0.0f, 0.0f, 0.0f) },
                    new ColorVertex { Position = new Vector3(-0.5f, -0.5f,  0.0f), Color = new Vector3(0.0f, 0.0f, 0.0f) },
                },
                new List<int>
                {
                    0, 1, 2,
                    2, 3, 0
                }
            ),
            new Box(
                new Vector3(-0.5f, -0.5f, -4f),
                new Vector3(1f, 1f, 0.5f)
            )
        )
        {
            Position = new Vector3(0, 0, -4f)
        };

        GL.Enable(EnableCap.StencilTest);
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key == Keys.L)
            Console.WriteLine(camera.Position);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (IsKeyDown(Keys.W))
        {
            player.Anchor += 0.01f * camera.Forward;
        }
        if (IsKeyDown(Keys.S))
        {
            player.Anchor -= 0.01f * camera.Forward;
        }
        if (IsKeyDown(Keys.A))
        {
            player.Anchor -= 0.01f * camera.Right;
        }
        if (IsKeyDown(Keys.D))
        {
            player.Anchor += 0.01f * camera.Right;
        }

        velocity += gravity * (float)e.Time;
        player.Anchor = new Vector3(player.Anchor.X, player.Anchor.Y - velocity * (float)e.Time, player.Anchor.Z);

        // Teleporting
        if (player.CollidesWith(portal1.Box!, out _))
        {
            player.Anchor += new Vector3(10, 30, 11.99f);
        }
        if (player.CollidesWith(portal2.Box!, out _))
        {
            player.Anchor += new Vector3(10, 30, 3.01f);
        }
        if (player.CollidesWith(actualTrigger1.Box!, out _))
        {
            player.Anchor -= new Vector3(10, 30, 11.99f);
        }
        if (player.CollidesWith(actualTrigger2.Box!, out _))
        {
            player.Anchor -= new Vector3(10, 30, 3.01f);
        }

        // Floors
        if (player.CollidesWith(level.Children[3].Box!, out var collisionSide))
        {
            if (collisionSide == Side.Bottom)
            {
                velocity = 0;
                player.Anchor = new Vector3(player.Anchor.X, level.Children[3].Box!.Top, player.Anchor.Z);
            }
        }

        if (player.CollidesWith(actualCorridor.Children[3].Box!, out collisionSide))
        {
            if (collisionSide == Side.Bottom)
            {
                velocity = 0;
                player.Anchor = new Vector3(player.Anchor.X, actualCorridor.Children[3].Box!.Top, player.Anchor.Z);
            }
        }
        
        camera.Position = player.TopCenter;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        defaultShader.Bind();

        defaultShader.SetMatrix4("projection", ProjectionMatrix);
        defaultShader.SetMatrix4("view", camera.ViewMatrix);

        GL.Enable(EnableCap.DepthTest);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);

        GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

        // Portal 1
        if (Vector3.Dot(portalNormal.Normalized(), (camera.Position - portal1.Position).Normalized()) >= 0)
        {
            GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            GL.StencilMask(0xFF);

            Render(portal1, defaultShader);
        }

        // Portal 2
        if (Vector3.Dot(portalNormal.Normalized(), (camera.Position - portal2.Position).Normalized()) <= 0)
        {
            GL.StencilFunc(StencilFunction.Always, 2, 0xFF);
            GL.StencilMask(0xFF);

            Render(portal2, defaultShader);
        }

        // Drawing inside of the portal 1
        GL.StencilMask(0x00);
        GL.StencilFunc(StencilFunction.Equal, 1, 0xFF);

        GL.Disable(EnableCap.DepthTest);

        Render(fakeLevel1, defaultShader);
        Render(fakeCorridor1, defaultShader);

        // Drawing inside of the portal 2
        GL.StencilFunc(StencilFunction.Equal, 2, 0xFF);

        Render(fakeLevel2, defaultShader);
        Render(fakeCorridor2, defaultShader);

        // Drawing outside of the portal
        GL.StencilMask(0xFF);
        GL.StencilFunc(StencilFunction.Always, 0, 0xFF);
        GL.Enable(EnableCap.DepthTest);

        Render(level, defaultShader);
        Render(fakeCorridorWrapper, defaultShader);

        Render(actualCorridor, defaultShader);

        Render(actualLevel1, defaultShader);
        Render(actualLevel2, defaultShader);

        SwapBuffers();
    }

    private void Render(ColorGameObject gameObject, Shader shader)
    {
        gameObject.Render(shader);
        
        if (gameObject.Children.Any())
        {
            foreach (var child in gameObject.Children)
            {
                Render(child, shader);
            }
        }
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        base.OnMouseMove(e);

        camera.Yaw += e.DeltaX;
        camera.Pitch -= e.DeltaY;
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }
}
