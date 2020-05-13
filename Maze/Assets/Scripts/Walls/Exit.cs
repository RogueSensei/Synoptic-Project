using MazeGame.MazeGeneration;
using UnityEngine;

namespace MazeGame
{
    public class Exit : Wall
    {
        public int exitRoomId;

        public void PositionWall(ExitPosition exitPosition, int exitRoomId)
        {
            this.exitRoomId = exitRoomId;

            base.PositionWall(exitPosition);
            this.name = $"Exit {exitPosition}";

            switch (exitPosition)
            {
                case ExitPosition.North:
                    this.transform.rotation = Quaternion.identity;
                    break;
                case ExitPosition.East:
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                    break;
                case ExitPosition.South:
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case ExitPosition.West:
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
                default:
                    break;
            }
        }
    }
}
