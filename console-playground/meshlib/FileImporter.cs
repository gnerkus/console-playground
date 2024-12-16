using MR.DotNet;

namespace Main.meshlib;

public class FileImporter
{
    private Mesh ReadMesh(string filePath)
    {
        var loadedMesh = MeshLoad.FromAnySupportedFormat(filePath);
        Console.WriteLine(loadedMesh.Area());
        return loadedMesh;
    }

    public Mesh ExamineFbxFile(string filePath)
    {
        var mesh = ReadMesh(filePath);
        return mesh;
    }
}
