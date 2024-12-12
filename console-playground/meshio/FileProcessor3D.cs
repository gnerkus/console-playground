using MeshIO;
using MeshIO.FBX;
using MeshIO.Core;
using MeshIO.Entities.Geometries;

namespace Main.meshio
{
    public class NotificationHelper
    {
        public static void LogConsoleNotification(object sender, NotificationEventArgs e)
        {
            switch (e.NotificationType)
            {
                case NotificationType.NotImplemented:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case NotificationType.Information:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case NotificationType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case NotificationType.Error:
                case NotificationType.NotSupported:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    break;
            }

            //Write in the console all the messages
            Console.WriteLine(e.Message);
        }
    }
    
    public static class FileProcessor3D
    {
        public static FbxRootNode ParseFbx(string file)
        {
            using var reader = new FbxReader(file);
            var node = reader.Parse();
            return node;
        }
        
        public static Scene ReadFbx(string file)
        {
            using var reader = new FbxReader(file);
            reader.OnNotification += NotificationHelper.LogConsoleNotification;
            var scene = reader.Read();
            return scene;
        }

        public static IEnumerable<Geometry> GetAllGeometryInScene(Scene scene)
        {
            foreach (var node in scene.RootNode.Nodes)
            {
                if (node.Parent is Geometry geometry)
                {
                    yield return geometry;
                }

                if (node.Nodes.Count <= 0) continue;
                foreach (var nestedNode in node.Nodes)
                {
                    Console.WriteLine(nestedNode);
                }
            }
        }

        public static IEnumerable<Geometry> ExamineFile(string filePath)
        {
            var scene = ReadFbx(filePath);
            return GetAllGeometryInScene(scene);
        }
    }
}