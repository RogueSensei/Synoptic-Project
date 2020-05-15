using System;
using UnityEngine;

namespace MazeGame.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public EntityProperties entityProperties;

        public void Initialize(MazeGeneration.EntityPosition position)
        {
            this.SetLocation(new Vector2(position.x, position.y));

            var location = GetLocation();

            this.name = $"{entityProperties.name} ({location.x}, {location.y})";
        }

        public Vector2 GetLocation()
        {
            return this.transform.position;
        }

        public void SetLocation(Vector2 location)
        {
            this.transform.position = location;
        }

        public void SetLocation(float x, float y)
        {
            this.transform.position = new Vector2(x, y);
        }
    }

    [Serializable]
    public struct EntityProperties
    {
        public string name;
        public int maxHealth;
        public bool isHostile;
    }
}
