using System.Collections;
using UnityEngine;

namespace MazeGame.Moving
{
    public abstract class Movable : MonoBehaviour
    {
        public float moveTime = 0.1f;
        public LayerMask blockingLayer;

        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody2D;
        private float _inverseMoveTime;

        protected virtual void Start()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _inverseMoveTime = 1f / moveTime;
        }

        /// <summary>
        /// Check if movement is possible
        /// </summary>
        protected bool Move(int xDirection, int yDirection, out RaycastHit2D hit2D)
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

        /// <summary>
        /// Make smooth movement
        /// </summary>
        protected IEnumerator SmoothMovement(Vector3 targetPosition)
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

        /// <summary>
        /// Attempt to move
        /// </summary>
        protected virtual void AttemptMove<T>(int xDirection, int yDirection)
            where T : Component
        {
            RaycastHit2D hit2D;
            bool canMove = Move(xDirection, yDirection, out hit2D);

            if (hit2D.transform == null)
                return;

            // Get the component that was hit
            T hitComponent = hit2D.transform.GetComponent<T>();

            if (!canMove && hitComponent != null)
                OnCantMove(hitComponent); // Act on the hit component
        }

        /// <summary>
        /// Act on component if cannot move
        /// </summary>
        protected abstract void OnCantMove<T>(T component)
            where T : Component;
    }
}
