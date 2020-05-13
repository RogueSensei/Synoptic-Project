using System;
using Random = UnityEngine.Random;

namespace MazeGame.MazeGeneration
{
    [Serializable]
    public enum ExitPosition
    {
        North,
        East,
        South,
        West
    }

    public static class ExitPositions
    {
        public static ExitPosition[] positions =
        {
            ExitPosition.North,
            ExitPosition.East,
            ExitPosition.South,
            ExitPosition.West
        };

        private static ExitPosition[] opposites =
        {
            ExitPosition.South,
            ExitPosition.West,
            ExitPosition.North,
            ExitPosition.East
        };

        public static ExitPosition GetOpposite(this ExitPosition exitPosition)
        {
            return opposites[(int)exitPosition];
        }

        public static ExitPosition GetRandomPostion()
        {
            int randomNumber = Random.Range(0, 3);

            return (ExitPosition)randomNumber;
        }

        public static ExitPosition GetRandomPostion(ExitPosition forbiddenPosition)
        {
            int forbiddenInt = (int)forbiddenPosition;
            int randomNumber = forbiddenInt;

            do
            {
                randomNumber = Random.Range(0, 3);

            } while (randomNumber == forbiddenInt);

            return (ExitPosition)randomNumber;
        }
    }
}
