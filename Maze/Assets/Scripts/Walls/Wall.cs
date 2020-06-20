using MazeGame.MazeGeneration;
using UnityEngine;

namespace MazeGame
{
    public class Wall : MonoBehaviour
    {
        public ExitPosition exitPosition;

        public void PositionWall(ExitPosition exitPosition)
        {
            this.name = $"Wall {exitPosition}";
            this.exitPosition = exitPosition;

            switch (exitPosition)
            {
                case ExitPosition.North:
                    this.transform.position = new Vector2(0.5f, 5.5f);
                    break;
                case ExitPosition.East:
                    this.transform.position = new Vector2(9.5f, 0.5f);
                    break;
                case ExitPosition.South:
                    this.transform.position = new Vector2(0.5f, -4.5f);
                    break;
                case ExitPosition.West:
                    this.transform.position = new Vector2(-8.5f, 0.5f);
                    break;
                default:
                    break;
            }
        }
    }
}
