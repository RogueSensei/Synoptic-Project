using MazeGame.MazeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MazeGame
{
    public class MainMenu : MonoBehaviour
    {
        private PlayerInput _inputActions;

        private void Awake()
        {
            _inputActions = new PlayerInput();
        }

        void Update()
        {
            if (_inputActions.Player.enabled)
            {
                var playerInput = _inputActions.Player;

                if (playerInput.Action.triggered)
                {
                    Debug.Log("New game");
                    MazeManager.DeleteMaze();
                    StartGame();
                }
                else if (playerInput.Drop.triggered)
                {
                    Debug.Log("Load game");
                    StartGame();
                }
                else if (playerInput.Cancel.triggered)
                {
                    Application.Quit();
                    Debug.Log("Quit!");
                }
            }
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}

