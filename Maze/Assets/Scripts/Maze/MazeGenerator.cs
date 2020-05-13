using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MazeGame.MazeGeneration
{
    public class MazeGenerator : MonoBehaviour
    {
        public int numberOfRooms;

        private Maze _maze;
        private List<Room> generatedRooms;
        private List<RoomExit> generatedExits;
        private List<int> unvisitedRooms;

        public Maze Maze { get; set; }

        public Maze LoadMaze()
        {
            _maze = MazeManager.LoadMaze();

            if(_maze == null)
            {
                _maze = GenerateMaze();

                MazeManager.SaveMaze(_maze);
            }

            return _maze;
        }

        private Maze GenerateMaze()
        {
            generatedRooms = new List<Room>();
            generatedExits = new List<RoomExit>();
            unvisitedRooms = new List<int>();

            Room[] mazeRooms = new Room[numberOfRooms];

            for (int i = 0; i <= numberOfRooms; i++)
            {
                unvisitedRooms.Add(i);
            }

            for (int i = 0; i < mazeRooms.Length; i++)
            {
                Room newRoom = new Room
                {
                    RoomId = i,
                    Exits = GenerateRoomExits(i)
                };

                if (i != 0) // First room should have no entities
                {
                    newRoom.Entities = MazeEntities.GenerateRoomEntities();
                }

                generatedRooms.Add(newRoom);

                Debug.Log($"Room {i} generated");
            }

            Debug.Log("Maze Generated");

            return new Maze
            {
                ExitRoomId = numberOfRooms,
                Rooms = generatedRooms.ToArray()
            };
        }

        private RoomExit[] GenerateRoomExits(int roomId)
        {
            ExitPosition randomPosition = ExitPositions.GetRandomPostion();
            ExitPosition forbiddenPosition = randomPosition;

            unvisitedRooms.Remove(unvisitedRooms.Where(x => x == roomId).FirstOrDefault()); // Mark room as visited
            bool anyUnvisitedRooms = unvisitedRooms.Count() > 0;

            List<RoomExit> newExits = new List<RoomExit>();

            // Add any connected rooms
            if (generatedExits.Where(x => x.NeighbouringRoom == roomId).Count() > 0)
            {
                var neighbourExit = generatedExits.Where(x => x.NeighbouringRoom == roomId).FirstOrDefault();
                Room neighbour = null;

                if (neighbourExit != null)
                {
                    neighbour = generatedRooms.Where(x => x.Exits.Contains(neighbourExit)).FirstOrDefault();
                    forbiddenPosition = neighbourExit.Position.GetOpposite();
                    randomPosition = ExitPositions.GetRandomPostion(forbiddenPosition);
                }

                var newExit = new RoomExit
                {
                    Position = forbiddenPosition,
                    IsExit = neighbour != null,
                    NeighbouringRoom = neighbour == null ? (int?)null : neighbour.RoomId
                };

                newExits.Add(newExit);
            }

            if (anyUnvisitedRooms)
            {
                // Generate room with assumption of at least 1 new exit
                int? newVistedRoom = anyUnvisitedRooms ? unvisitedRooms.FirstOrDefault() : (int?)null;

                newExits.Add(new RoomExit
                {
                    IsExit = anyUnvisitedRooms, // Guarantee at least one exit, unless all rooms have been visited
                    Position = randomPosition,
                    NeighbouringRoom = newVistedRoom
                });

                int exactRoomId;

                if (int.TryParse(newVistedRoom.ToString(), out exactRoomId))
                {
                    unvisitedRooms.Remove(unvisitedRooms.Where(x => x == exactRoomId).FirstOrDefault());
                }
            }

            // Generate all other exits
            for (int i = 0; i < ExitPositions.positions.Length; i++)
            {
                var position = (ExitPosition)i;

                if (position != randomPosition && position != forbiddenPosition)
                {
                    bool isExit = false;
                    int? neighbouringRoom = null;

                    if (unvisitedRooms.Count() > 0)
                    {
                        // Leave it to chance
                        var chance = Random.Range(1, 10) % 2 == 0;

                        if (chance)
                        {
                            isExit = true;
                            neighbouringRoom = unvisitedRooms.FirstOrDefault();
                            unvisitedRooms.Remove(unvisitedRooms.Where(x => x == neighbouringRoom).FirstOrDefault());
                        }
                    }

                    newExits.Add(new RoomExit
                    {
                        IsExit = isExit,
                        Position = position,
                        NeighbouringRoom = neighbouringRoom
                    });
                }
            }

            generatedExits.AddRange(newExits);

            return newExits.ToArray();
        }
    }
}
