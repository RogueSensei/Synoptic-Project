using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame.Entities
{
    public class Coin : Entity
    {
        
    }

    public class CoinByRoom
    {
        public int RoomId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
    }
}

