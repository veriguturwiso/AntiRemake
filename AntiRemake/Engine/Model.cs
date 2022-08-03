using Assimp;
using OpenTK.Mathematics;

namespace AntiRemake.Engine;

public class Model
{
    private List<Mesh> meshes = new List<Mesh>();
    private List<Texture> loadedTextures = new List<Texture>();
    private string directory = string.Empty;

    public Model(string path)
    {
        var importer = new AssimpContext();
        var scene = importer.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs);

        if (path.Contains('/'))
            directory = $"{path[..path.LastIndexOf('/')]}/";
        
        foreach (var mesh in scene.Meshes)
        {
            meshes.Add(ProcessMesh(mesh, scene));
        }
    }

    public Model(List<Mesh> meshes)
    {
        this.meshes = meshes;
    }

    public void Render(Shader shader)
    {
        foreach (var mesh in meshes)
        {
            mesh.Render(shader);
        }
    }

    private Mesh ProcessMesh(Assimp.Mesh mesh, Assimp.Scene scene)
    {
        var vertices = new VertexCollection();
        var indices = new List<int>();
        var textures = new List<Texture>();
        Color4 colorDiffuse = Color4.White;

        for (int i = 0; i < mesh.VertexCount; i++)
        {
            Vector2 texCoords = new Vector2();

            if (mesh.TextureCoordinateChannels[0].Count > 0)
            {
                texCoords.X = mesh.TextureCoordinateChannels[0][i].X;
                texCoords.Y = mesh.TextureCoordinateChannels[0][i].Y;
            }
            
            vertices.Add(new Vertex
            {
                Position = new Vector3(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z),
                Normal = new Vector3(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z),
                TexCoords = texCoords
            });
        }

        foreach (var face in mesh.Faces)
        {
            foreach (var index in face.Indices)
            {
                indices.Add(index);
            }
        }
        
        if (mesh.MaterialIndex >= 0)
        {
            var material = scene.Materials[mesh.MaterialIndex];

            colorDiffuse = new Color4(material.ColorDiffuse.R, material.ColorDiffuse.G, material.ColorDiffuse.B, material.ColorDiffuse.A);

            var diffuseMaps = LoadMaterialTextures(material, TextureType.Diffuse, "texture_diffuse");
            textures.AddRange(diffuseMaps);

            var specularMaps = LoadMaterialTextures(material, TextureType.Specular, "texture_specular");
            textures.AddRange(specularMaps);
        }

        return new Mesh(vertices, indices, textures, colorDiffuse);
    }

    private List<Texture> LoadMaterialTextures(Material material, TextureType textureType, string typeName)
    {
        var textures = new List<Texture>();

        for (int i = 0; i < material.GetMaterialTextureCount(textureType); i++)
        {
            Console.WriteLine(material.GetMaterialTexture(textureType, i, out var textureSlot));

            var skip = false;

            foreach (var loadedTexture in loadedTextures)
            {
                if (loadedTexture.Path == $"{directory}/{textureSlot.FilePath}")
                {
                    textures.Add(loadedTexture);
                    skip = true;
                    break;
                }
            }

            if (!skip)
            {
                var texture = new Texture($"{directory}{textureSlot.FilePath}", typeName);
                textures.Add(texture);
                loadedTextures.Add(texture);
            }
        }

        return textures;
    }
}
