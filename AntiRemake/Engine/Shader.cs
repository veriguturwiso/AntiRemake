using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace AntiRemake.Engine;

public class Shader
{
    private int ID;

    public Shader(string vertexPath, string fragmentPath)
    {
        var vertexSource = File.ReadAllText(vertexPath);
        var fragmentSource = File.ReadAllText(fragmentPath);

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(vertexShader, vertexSource);
        GL.ShaderSource(fragmentShader, fragmentSource);

        GL.CompileShader(vertexShader);
        CheckShaderCompilationErrors(vertexShader, vertexPath);

        GL.CompileShader(fragmentShader);
        CheckShaderCompilationErrors(fragmentShader, fragmentPath);

        ID = GL.CreateProgram();

        GL.AttachShader(ID, vertexShader);
        GL.AttachShader(ID, fragmentShader);

        GL.LinkProgram(ID);

        CheckProgramLinkStatus();

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Bind()
    {
        GL.UseProgram(ID);
    }

    public void Unbind()
    {
        GL.UseProgram(0);
    }

    public void SetInt(string name, int value)
    {
        var location = GL.GetUniformLocation(ID, name);
        GL.Uniform1(location, value);
    }

    public void SetFloat(string name, float value)
    {
        var location = GL.GetUniformLocation(ID, name);
        GL.Uniform1(location, value);
    }

    public void SetBool(string name, bool value)
    {
        SetInt(name, value ? 1 : 0);
    }

    public void SetMatrix4(string name, Matrix4 value)
    {
        var location = GL.GetUniformLocation(ID, name);
        GL.UniformMatrix4(location, false, ref value);
    }

    public void SetVector3(string name, Vector3 value)
    {
        var location = GL.GetUniformLocation(ID, name);
        GL.Uniform3(location, ref value);
    }

    public void SetVector4(string name, Vector4 value)
    {
        var location = GL.GetUniformLocation(ID, name);
        GL.Uniform4(location, ref value);
    }

    private void CheckProgramLinkStatus()
    {
        int success;
        GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out success);

        if (success == 0)
        {
            Console.WriteLine($"Program linking failed\n- {GL.GetProgramInfoLog(ID)}");
        }
    }

    private void CheckShaderCompilationErrors(int shader, string shaderPath)
    {
        int success;
        GL.GetShader(shader, ShaderParameter.CompileStatus, out success);

        if (success == 0)
        {
            Console.WriteLine($"Shader compilation failed: '{shaderPath}'\n- {GL.GetShaderInfoLog(shader)}");
        }
    }
}
