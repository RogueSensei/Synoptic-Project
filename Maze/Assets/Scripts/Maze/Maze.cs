namespace MazeGame.MazeGeneration
{
    [System.Serializable]
    public class Maze
    {
        public int ExitRoomId { get; set; }
        public Room[] Rooms { get; set; }
    }

    [System.Serializable]
    public class Room
    {
        public int RoomId { get; set; }
        public RoomExit[] Exits { get; set; }
        public RoomEntity[] Entities { get; set; }
    }

    [System.Serializable]
    public class RoomExit
    {
        public ExitPosition Position { get; set; }
        public bool IsExit { get; set; }
        public int? NeighbouringRoom { get; set; }
    }

    [System.Serializable]
    public class RoomEntity
    {
        public EntityType Type { get; set; }
        public int Health { get; set; }
        public EntityPosition Position { get; set; }
        public bool Active { get; set; }
    }

    [System.Serializable]
    public class EntityPosition
    {
        public float x { get; set; }
        public float y { get; set; }
    }
}
