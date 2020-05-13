using MazeGame.Entities;
using MazeGame.MazeGeneration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeGame
{
    public class RoomManager : MonoBehaviour
    {
        public Wall wallPrefab;
        public Exit exitPrefab;
        public Rock rockPrefab;
        public Treasure treasurePrefab;
        public Trap trapPrefab;
        public Zombie zombiePrefab;
        public Skeleton skeletonPrefab;

        private List<Wall> _walls = new List<Wall>();
        private List<Exit> _exits = new List<Exit>();
        private List<Entity> _entities = new List<Entity>();

        public void InterpretRoom(Room room)
        {
            ClearRoom();

            LayoutWalls(room.Exits);

            if (room.RoomId != 0) // Room 0 will not have any entities
            {
                for (int i = 0; i < room.Entities.Length; i++)
                {
                    LayoutEntity(room.Entities[i], i);
                }
            }
        }

        private void LayoutWalls(RoomExit[] exits)
        {
            for (int i = 0; i < exits.Length; i++)
            {
                if (!exits[i].IsExit)
                {
                    Wall newWall = Instantiate(wallPrefab) as Wall;

                    newWall.PositionWall(exits[i].Position);

                    _walls.Add(newWall);
                }
                else
                {
                    Exit newExit = Instantiate(exitPrefab) as Exit;

                    newExit.PositionWall(exits[i].Position, (int)exits[i].NeighbouringRoom);

                    _exits.Add(newExit);
                }
            }
        }

        private void LayoutEntity(RoomEntity entity, int index)
        {
            switch (entity.Type)
            {
                case EntityType.Rock:
                    _entities.Add(Instantiate(rockPrefab) as Rock);
                    break;
                case EntityType.Coin:
                    break;
                case EntityType.Treasure:
                    _entities.Add(Instantiate(treasurePrefab) as Treasure);
                    break;
                case EntityType.Trap:
                    _entities.Add(Instantiate(trapPrefab) as Trap);
                    break;
                case EntityType.Zombie:
                    _entities.Add(Instantiate(zombiePrefab) as Zombie);
                    break;
                case EntityType.Skeleton:
                    _entities.Add(Instantiate(skeletonPrefab) as Skeleton);
                    break;
                default:
                    break;
            }

            _entities[index].Initialize(entity.Position);
            _entities[index].entityProperties.maxHealth = entity.Health;
        }

        private void ClearRoom()
        {
            if (_walls.Count() > 0)
            {
                foreach (var wall in _walls)
                {
                    Destroy(wall.gameObject);
                }

                _walls.Clear();
            }

            if (_exits.Count() > 0)
            {
                foreach (var exit in _exits)
                {
                    Destroy(exit.gameObject);
                }

                _exits.Clear();
            }

            if (_entities.Count() > 0)
            {
                foreach (var entity in _entities)
                {
                    Destroy(entity.gameObject);
                }

                _entities.Clear();
            }
        }
    }
}
