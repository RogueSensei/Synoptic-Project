using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace MazeGame.MazeGeneration
{
    public static class MazeManager
    {
        public static void SaveMaze(Maze maze)
        {
            string directory = Application.persistentDataPath;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = $"{directory}/maze.json";
            string jsonString = JsonConvert.SerializeObject(maze);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(jsonString);
                }
            }
        }

        public static Maze LoadMaze()
        {
            string path = $"{Application.persistentDataPath}/maze.json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Maze maze = JsonConvert.DeserializeObject<Maze>(json);

                return maze;
            }
            else
            {
                Debug.LogWarning($"File was not found: {path}");
                return null;
            }
        }

        public static void DeleteMaze()
        {
            string path = $"{Application.persistentDataPath}/maze.json";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
