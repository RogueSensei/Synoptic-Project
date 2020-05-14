using System.Collections;
using UnityEngine;

namespace MazeGame.Entities
{
    public class Threat : Entity
    { 
        public float moveTime = 0.1f;
        public LayerMask blockingLayer;

        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody2D;
        private float _inverseMoveTime;
        private Player _player;

        private void Start()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();

            _boxCollider = GetComponent<BoxCollider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _inverseMoveTime = 1f / moveTime;

            _player = FindObjectOfType<Player>();

            gameManager.enemies.Add(this);
        }

        public void MoveThreat()
        {
            int xDirection = 0;
            int yDirection = 0;

            // Check if player is in the same column
            if(Mathf.Abs(_player.transform.position.x - this.transform.position.x) < float.Epsilon)
            {
                // If below, move down and vice versa
                yDirection = _player.transform.position.y > this.transform.position.y ? 1 : -1;
            }
            else
            {
                xDirection = _player.transform.position. x > this.transform.position.x ? 1 : -1;
            }

            AttemptMove<Player>(xDirection, yDirection);
        }

        private bool Move(int xDirection, int yDirection, out RaycastHit2D hit2D)
        {
            Vector2 start = this.transform.position;
            Vector2 end = start + new Vector2(xDirection, yDirection);

            this._boxCollider.enabled = false; // Disable own box collider so we don't accidently trigger it
            hit2D = Physics2D.Linecast(start, end, blockingLayer);
            this._boxCollider.enabled = true;

            if (hit2D.transform == null)
            {
                StartCoroutine(SmoothMovement(end));
                return true;
            }

            return false;
        }

        private IEnumerator SmoothMovement(Vector3 targetPosition)
        {
            float sqrRemainingDistance = (this.transform.position - targetPosition).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon)
            {
                Vector3 newPostition = Vector3.MoveTowards(_rigidbody2D.position, targetPosition, _inverseMoveTime * Time.deltaTime);

                _rigidbody2D.MovePosition(newPostition);
                sqrRemainingDistance = (transform.position - targetPosition).sqrMagnitude;

                yield return null;
            }
        }

        private void AttemptMove<T>(int xDirection, int yDirection)
            where T : Component
        {
            RaycastHit2D hit2D;
            bool canMove = Move(xDirection, yDirection, out hit2D);

            if (hit2D.transform == null)
                return;

            // Get the component that was hit
            T hitComponent = hit2D.transform.GetComponent<T>();

            if (!canMove && hitComponent != null)
                OnCantMove<Component>(hitComponent); // Act on the hit component
        }

        private void OnCantMove<T>(T component)
            where T : Component
        {
            Player player = component as Player;
            GameManager gameManager = FindObjectOfType<GameManager>();

            player.wealth -= 5;
            gameManager.UpdateWealthText();
        }
    }
}
