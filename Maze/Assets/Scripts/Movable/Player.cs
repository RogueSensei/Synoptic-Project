using MazeGame.MazeGeneration;
using MazeGame.Moving;
using System.Collections;
using UnityEngine;

namespace MazeGame.Entities
{
    public class Player : Movable
    {
        public int wealth;
        public bool isMoving;
        public float coolDown;

        private void Awake()
        {
            wealth = 0;
            isMoving = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            string tag = collision.tag;
            GameManager gameManager = FindObjectOfType<GameManager>();
            switch (tag)
            {
                case "Treasure":
                    wealth += 10;
                    collision.gameObject.SetActive(false);
                    gameManager.UpdateWealthText();
                    gameManager.UpdateRoomEntity(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
                    break;
                case "Exit":
                    if (!isMoving)
                    {
                        Exit exit = collision.gameObject.GetComponent<Exit>();

                        gameManager.DisablePlayerInput();

                        ExitPosition roomDirection = exit.exitPosition.GetOpposite();

                        this.gameObject.SetActive(false);
                        gameManager.EnterRoom(roomDirection, exit.exitRoomId);
                    }
                    break;
                default:
                    break;
            }
        }

        private void EnterRoom(ExitPosition exitPosition)
        {
            switch (exitPosition)
            {
                case ExitPosition.North:
                    SetLocation(0.5f, 4.5f);
                    break;
                case ExitPosition.East:
                    SetLocation(8.5f, 0.5f);
                    break;
                case ExitPosition.South:
                    SetLocation(0.5f, -3.5f);
                    break;
                case ExitPosition.West:
                    SetLocation(-7.5f, 0.5f);
                    break;
                default:
                    break;
            }
        }

        public void RegisterAction(PlayerAction action)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            int horizontal = 0;
            int vertical = 0;

            switch (action)
            {
                case PlayerAction.MoveNorth:
                    vertical++;
                    break;
                case PlayerAction.MoveEast:
                    horizontal++;
                    break;
                case PlayerAction.MoveSouth:
                    vertical--;
                    break;
                case PlayerAction.MoveWest:
                    horizontal--;
                    break;
                default:
                    break;
            }

            // Stops diagonal movement
            if (horizontal != 0)
                vertical = 0;

            isMoving = true;
            AttemptMove<Entity>(horizontal, vertical);
            isMoving = false;

            if(gameManager.gameState == GameState.TurnInProgress)
            {
                StartCoroutine(Wait());
            }
        }

        private IEnumerator Wait()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            
            gameManager.DisablePlayerInput();
            yield return new WaitForSeconds(coolDown);
            gameManager.gameState = GameState.EnemyTurn;
        }

        public void SetLocation(float x, float y)
        {
            this.transform.position = new Vector2(x, y);
        }

        protected override void OnCantMove<T>(T component)
        {
            Entity entity = component as Entity;
            GameManager gameManager = FindObjectOfType<GameManager>();

            switch(entity.entityProperties.name)
            {
                case "Rock":
                    // gameManager.gameState = GameState.PlayerTurn;
                    break;
                case "Trap":
                    wealth -=5;
                    gameManager.UpdateWealthText();
                    break;
                default:
                    break;
            }
        }
    }
    
    public enum PlayerAction
    {
        None,
        MoveNorth,
        MoveEast,
        MoveSouth,
        MoveWest,
        DropCoin,
        Escape
    }
}
