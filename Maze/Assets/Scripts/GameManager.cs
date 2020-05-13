using MazeGame.Entities;
using MazeGame.MazeGeneration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace MazeGame
{
    public class GameManager : MonoBehaviour
    {
        public Player playerPrefab;
        public MazeGenerator mazeGeneratorPrebab;

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
            if (_inputActions.Player.enabled && !_player.isMoving)
                _player.RegisterAction(InterpretPlayerAction());
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

            _roomManager.InterpretRoom(room);
        }

        public void EnablePlayerInput()
        {
            _inputActions.Enable();
        }

        public void DisablePlayerInput()
        {
            _inputActions.Disable();
        }

        /// <summary>
        /// Interprets Input into an enum form
        /// </summary>
        /// <returns></returns>
        private PlayerAction InterpretPlayerAction()
        {
            var playerInput = _inputActions.Player;

            if (playerInput.North.triggered)
            {
                return PlayerAction.MoveNorth;
            }
            else if (playerInput.East.triggered)
            {
                return PlayerAction.MoveEast;
            }
            else if (playerInput.South.triggered)
            {
                return PlayerAction.MoveSouth;
            }
            else if (playerInput.West.triggered)
            {
                return PlayerAction.MoveWest;
            }
            else
            {
                return PlayerAction.None;
            }
        }
    }
}
