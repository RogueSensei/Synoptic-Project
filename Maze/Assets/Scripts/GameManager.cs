using MazeGame.Entities;
using MazeGame.MazeGeneration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MazeGame
{
    public class GameManager : MonoBehaviour
    {
        public Player playerPrefab;
        public MazeGenerator mazeGeneratorPrebab;
        public Text wealthText;
        public Text roomText;
        public int currentRoomId;
        public GameState gameState = GameState.None;

        private Maze _maze;
        private List<Room> _rooms;
        private Player _player;
        private MazeGenerator _mazeGenerator;
        private PlayerInput _inputActions;
        private RoomManager _roomManager;

        private void Awake()
        {
            _inputActions = new PlayerInput();
            _roomManager = GetComponent<RoomManager>();

            _mazeGenerator = Instantiate(mazeGeneratorPrebab) as MazeGenerator;
            _mazeGenerator.name = "Maze Generator";
        }

        private void Start()
        {
            BeginGame();
            SpawnPlayer();
        }

        private void Update()
        {
            switch(gameState)
            {
                case GameState.None:
                    break;
                case GameState.PlayerTurn:
                    if (_inputActions.Player.enabled && !_player.isMoving)
                    {
                        var playerAction = InterpretPlayerAction();

                        if(playerAction == PlayerAction.DropCoin)
                        {
                            Debug.Log("Coin drop");
                            DropCoin(_player.transform.position.x, _player.transform.position.y);
                        }
                        else
                        {
                            _player.RegisterAction(playerAction);
                        }
                    }
                    break;
                case GameState.EnemyTurn:
                    if(CheckForEnemies())
                    {
                        EnemyTurn();
                    }
                    gameState = GameState.PlayerTurn;
                    break;
                case GameState.TurnInProgress:
                    break;
                default:
                    break;
            }
        }

        private void OnEnable()
        {
            EnablePlayerInput();
        }

        private void OnDisable()
        {
            DisablePlayerInput();
        }

        private void BeginGame()
        {
            _maze = _mazeGenerator.LoadMaze();

            _rooms = _maze.Rooms.ToList();

            _player = Instantiate(playerPrefab) as Player;
            _player.name = "Player";
        }

        public void SpawnPlayer()
        {
            _player.SetLocation(.5f, .5f);

            LoadRoom(0);
        }

        public void EnterRoom(ExitPosition exitPosition, int roomId)
        {
            int wealth = _player.wealth;
            Destroy(_player.gameObject);

            if (_maze.ExitRoomId == roomId)
            {
                Debug.Log("You win!");
                Debug.Log($"Wealth: {wealth}");
            }
            else
            {
                LoadRoom(roomId);

                _player = Instantiate(playerPrefab) as Player;

                _player.name = "Player";
                _player.wealth = wealth;

                switch (exitPosition)
                {
                    case ExitPosition.North:
                        _player.SetLocation(0.5f, 4.5f);
                        break;
                    case ExitPosition.East:
                        _player.SetLocation(8.5f, 0.5f);
                        break;
                    case ExitPosition.South:
                        _player.SetLocation(0.5f, -3.5f);
                        break;
                    case ExitPosition.West:
                        _player.SetLocation(-7.5f, 0.5f);
                        break;
                    default:
                        break;
                }

                EnablePlayerInput();
            }
        }

        public void LoadRoom(int roomId)
        {
            var room = _rooms.Where(x => x.RoomId == roomId).FirstOrDefault();

            currentRoomId = roomId;

            _roomManager.InterpretRoom(room);

            UpdateRoomText(roomId);

            gameState = GameState.PlayerTurn;
        }

        public void UpdateRoomEntity(float x, float y)
        {
            var entities = _rooms.Where(r => r.RoomId == currentRoomId).FirstOrDefault().Entities.ToList();

            var entity = entities.Where(e => e.Position.x == x && e.Position.y == y).FirstOrDefault();

            entity.Active = false;
        }

        public void DropCoin(float xPosition, float yPosition)
        {
            _roomManager.AddCoin(currentRoomId, xPosition, yPosition);

            gameState = GameState.EnemyTurn;
        }

        public void EnablePlayerInput()
        {
            _inputActions.Enable();
        }

        public void DisablePlayerInput()
        {
            _inputActions.Disable();
        }

        public void UpdateWealthText()
        {
            wealthText.text = $"Wealth: {_player.wealth}";
        }

        private void UpdateRoomText(int roomId)
        {
            roomId += 1; // Make user friendly

            roomText.text = $"Room: {roomId}";
        }

        private bool CheckForEnemies()
        {
            var entities = _rooms.Where(r => r.RoomId == currentRoomId).FirstOrDefault().Entities;

            if(entities != null)
            {
                var enemies = entities.ToList().Where(x => x.Type == EntityType.Zombie || x.Type == EntityType.Skeleton);

                return enemies.Count() > 0;
            }

            return false;
        }

        private void EnemyTurn()
        {
            Debug.Log("Let's go baby");
        }

        private PlayerAction InterpretPlayerAction()
        {
            var playerInput = _inputActions.Player;

            if (playerInput.North.triggered)
            {
                gameState = GameState.TurnInProgress;
                return PlayerAction.MoveNorth;
            }
            else if (playerInput.East.triggered)
            {
                gameState = GameState.TurnInProgress;
                return PlayerAction.MoveEast;
            }
            else if (playerInput.South.triggered)
            {
                gameState = GameState.TurnInProgress;
                return PlayerAction.MoveSouth;
            }
            else if (playerInput.West.triggered)
            {
                gameState = GameState.TurnInProgress;
                return PlayerAction.MoveWest;
            }
            else if (playerInput.Drop.triggered)
            {
                gameState = GameState.TurnInProgress;
                return PlayerAction.DropCoin;
            }
            else
            {
                return PlayerAction.None;
            }
        }
    }

    public enum GameState
    {
        None,
        PlayerTurn,
        EnemyTurn,
        TurnInProgress
    }

}
