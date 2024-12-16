using System.Text.Json;
using System.Text.Json.Serialization;
using MR.DotNet;

namespace Main.meshlib;

public class FileImporter
{
    private JsonSerializerOptions _options = new (){
        ReferenceHandler = ReferenceHandler.Preserve
    };
    private Mesh ReadMesh(string filePath)
    {
        var loadedMesh = MeshLoad.FromAnySupportedFormat(filePath);
        Console.WriteLine(loadedMesh.Area());
        return loadedMesh;
    }

    private string SerializeMesh(Mesh mesh)
    {
        return JsonSerializer.Serialize(mesh, _options);
    }

    public string ExamineStlFile(string filePath)
    {
        var mesh = ReadMesh(filePath);
        return SerializeMesh(mesh);
    }
}
