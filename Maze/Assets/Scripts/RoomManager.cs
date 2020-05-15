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
        public Coin coinPrefab;
        public Treasure treasurePrefab;
        public Trap trapPrefab;
        public Zombie zombiePrefab;
        public Skeleton skeletonPrefab;

        private List<Wall> _walls = new List<Wall>();
        private List<Exit> _exits = new List<Exit>();
        private List<Entity> _entities = new List<Entity>();
        private List<CoinByRoom> _coinsByRoom = new List<CoinByRoom>();

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

            foreach (CoinByRoom coinByRoom in _coinsByRoom.Where(x => x.RoomId == room.RoomId))
            {
                Coin roomCoin = Instantiate(coinPrefab) as Coin;
                
                roomCoin.Initialize(new EntityPosition 
                {
                    x = coinByRoom.PositionX,
                    y = coinByRoom.PositionY
                });

                _entities.Add(roomCoin);
            }
        }

        public void AddCoin(int roomId, float xPosition, float yPosition)
        {
            Coin coin = Instantiate(coinPrefab) as Coin;
            
            coin.Initialize(new EntityPosition 
            {
                x = xPosition,
                y = yPosition
            });

            _entities.Add(coin);

            _coinsByRoom.Add(new CoinByRoom
            {
                RoomId = roomId,
                PositionX = xPosition,
                PositionY = yPosition
            });
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
                    _entities.Add(Instantiate(coinPrefab) as Coin);
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
            _entities[index].gameObject.SetActive(entity.Active);
        }

        public void ClearRoom()
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
