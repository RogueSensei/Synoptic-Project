using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MazeGame.MazeGeneration
{
    [Serializable]
    public enum EntityType
    {
        Rock,
        Coin,
        Treasure,
        Trap,
        Zombie,
        Skeleton
    }

    public static class MazeEntities
    {
        private static List<EntityPosition> _gridPositions = new List<EntityPosition>();
        private static Count _trapCount = new Count(0, 1);
        private static Count _threatCount = new Count(0, 2);
        private static Count _treasureCount = new Count(1, 3);
        private static Count _rockCount = new Count(5, 9);

        public static RoomEntity[] GenerateRoomEntities()
        {
            List<RoomEntity> newEntities = new List<RoomEntity>();

            Debug.Log("Generating room entities");

            InitializeList();

            foreach (var entity in entitityTypes)
            {
                newEntities.AddRange(LayoutEntitiesAtRandom(entity));
            }

            return newEntities.ToArray();
        }

        private static void InitializeList()
        {
            _gridPositions.Clear();

            float xPosition = minActiveX;

            for (int x = 0; x <= maxActiveX - minActiveX; x++)
            {
                float yPosition = minActiveY;

                for (int y = 0; y <= maxActiveY - minActiveY; y++)
                {
                    _gridPositions.Add(new EntityPosition { x = xPosition, y = yPosition });

                    yPosition += 1;
                }
                xPosition += 1;
            }
        }

        private static List<RoomEntity> LayoutEntitiesAtRandom(EntityType entityType)
        {
            // Random random = new Random();

            int health = 1;
            Count count = new Count(0, 0);

            switch (entityType)
            {
                case EntityType.Rock:
                    health = 10;
                    count = _rockCount;
                    break;
                case EntityType.Treasure:
                    health = 1;
                    count = _treasureCount;
                    break;
                case EntityType.Trap:
                    health = 1;
                    count = _trapCount;
                    break;
                case EntityType.Zombie:
                    health = 2;
                    count = _threatCount;
                    break;
                case EntityType.Skeleton:
                    health = 3;
                    count = _threatCount;
                    break;
                default:
                    break;
            }

            int entityCount = Random.Range(count.minimum, count.maximum + 1);

            List<RoomEntity> entities = new List<RoomEntity>();

            for (int i = 0; i < entityCount; i++)
            {
                entities.Add(new RoomEntity
                {
                    Type = entityType,
                    Health = health,
                    Position = RandomPosition(),
                    Active = true
                });
            }

            return entities;
        }

        private static EntityPosition RandomPosition()
        {
            // Random random = new Random();
            int randomIndex = Random.Range(0, _gridPositions.Count);

            EntityPosition randomPostion = _gridPositions[randomIndex];
            _gridPositions.RemoveAt(randomIndex);

            return randomPostion;
        }

        private static float minActiveX = -6.5f;
        private static float maxActiveX = 7.5f;
        private static float minActiveY = -2.5f;
        private static float maxActiveY = 3.5f;

        private static EntityType[] entitityTypes = // Excluding Coin since these will be dropped by player
        {
            EntityType.Rock,
            EntityType.Treasure,
            EntityType.Trap,
            EntityType.Zombie,
            EntityType.Skeleton
        };
    }

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
}
