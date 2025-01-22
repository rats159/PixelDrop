using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics.Vulkan;
using OpenTK.Mathematics;

namespace PixelDrop.Renderer.Shaders;

public class Shader
{
    private readonly int _handle;
    private bool _disposed = false;

    private readonly Dictionary<string, int> _uniforms = [];

    public Shader(string vertexPath, string fragmentPath)
    {
        string vertexSource = File.ReadAllText(vertexPath);
        string fragmentSource = File.ReadAllText(fragmentPath);

        int vertexId = GL.CreateShader(ShaderType.VertexShader);
        int fragmentId = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(vertexId, vertexSource);
        GL.ShaderSource(fragmentId, fragmentSource);

        GL.CompileShader(vertexId);

        GL.GetShaderi(vertexId, ShaderParameterName.CompileStatus, out int success);
        if (success == 0)
        {
            GL.GetShaderInfoLog(vertexId, out string log);
            Console.WriteLine(log);
        }

        GL.CompileShader(fragmentId);

        GL.GetShaderi(fragmentId, ShaderParameterName.CompileStatus, out success);
        if (success == 0)
        {
            GL.GetShaderInfoLog(fragmentId, out string log);
            Console.WriteLine(log);
        }

        this._handle = GL.CreateProgram();
        GL.AttachShader(this._handle, vertexId);
        GL.AttachShader(this._handle, fragmentId);

        GL.LinkProgram(this._handle);

        GL.GetProgrami(this._handle, ProgramProperty.LinkStatus, out success);

        if (success == 0)
        {
            GL.GetProgramInfoLog(this._handle, out string log);
            Console.WriteLine(log);
        }

        GL.DetachShader(this._handle, vertexId);
        GL.DetachShader(this._handle, fragmentId);
        GL.DeleteShader(vertexId);
        GL.DeleteShader(fragmentId);
    }

    ~Shader()
    {
        if (!this._disposed)
        {
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }

    public int GetUniform(string name)
    {
        if (this._uniforms.TryGetValue(name, out int value)) return value;

        value = GL.GetUniformLocation(this._handle, name);
        this._uniforms[name] = value;

        return value;
    }

    public void Use()
    {
        GL.UseProgram(this._handle);
    }

    public void Dispose()
    {
        if (this._disposed) return;

        GL.DeleteProgram(this._handle);
        this._disposed = true;
    }

    public void Load<T>(string location, T value)
    {
        this.Load(this.GetUniform(location), value);
    }

    public void Load<T>(int location, T value)
    {
        switch (value)
        {
            case bool b: this.LoadBool(location, b); break;
            case float f: this.LoadFloat(location, f); break;
            case Vector2 vec2: this.LoadVec2(location, vec2); break;
            case Vector3 vec3: this.LoadVec3(location, vec3); break;
            case Matrix4 mat: this.LoadMatrix4(location, mat); break;
            default: throw new ArgumentException("Unsupported type to load to shader");
        }
    }

    public void LoadBool(int location, bool b)
    {
        GL.Uniform1i(location, b ? 1 : 0);
    }

    public void LoadFloat(int location, float f)
    {
        GL.Uniform1f(location, f);
    }

    public void LoadVec2(int location, Vector2 vec)
    {
        GL.Uniform2f(location, vec.X, vec.Y);
    }

    public void LoadVec3(int location, Vector3 vec)
    {
        GL.Uniform3f(location, vec.X, vec.Y, vec.Z);
    }

    public void LoadMatrix4(int location, Matrix4 mat)
    {
        GL.UniformMatrix4f(location, 1, false, in mat);
    }
}