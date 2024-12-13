using System.Text.Json;
using System.Text.Json.Serialization;
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
    
    public class FileProcessor3D
    {
        private JsonSerializerOptions _options = new (){
            ReferenceHandler = ReferenceHandler.Preserve
        };
        public FbxRootNode ParseFbx(string file)
        {
            using var reader = new FbxReader(file);
            var node = reader.Parse();
            return node;
        }

        private Scene ReadFbx(string file)
        {
            using var reader = new FbxReader(file);
            reader.OnNotification += NotificationHelper.LogConsoleNotification;
            var scene = reader.Read();
            return scene;
        }

        private string GetAllGeometryInScene(Scene scene)
        {
            return JsonSerializer.Serialize(scene, _options);
        }

        public string ExamineFbxFile(string filePath)
        {
            var scene = ReadFbx(filePath);
            var geometries = GetAllGeometryInScene(scene);
            return geometries;
        }
    }
}