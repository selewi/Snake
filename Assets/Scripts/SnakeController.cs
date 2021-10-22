using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace Snake
{
    [RequireComponent (typeof (Collider))]
    public class SnakeController : MonoBehaviour
    {
        [Header ("Parameters")]
        [SerializeField] private int _startingSize = 9;
        [SerializeField] private float _timeBetweenMovements = 1.0f; // in seconds

        [Header ("References")]
        [SerializeField] private Transform _pieceContainerTransform = null;
        [SerializeField] private GameObject _piecePrefab = null;

        private Vector2Int _currentFacingDirection = Vector2Int.right;
        private Vector2Int _desiredFacingDirection = Vector2Int.right;
        private float _nextMovementTime = 0.0f;
        private List<SnakePieceController> _pieces = null;

        public UnityAction OnPickupCollision = null;
        public UnityAction OnObstacleCollision = null;

        public Vector2Int[] OccupiedPositions => _pieces.Select (x => x.CurrentGridPosition).ToArray ();

        private void Start ()
        {
            for (int i = 0; i < _startingSize; i++)
            {
                Grow ();
                Move ();
            }
        }

        void Update ()
        {
            if (Time.time > _nextMovementTime)
            {
                Move ();
            }
        }

        public void TryUpdateDirection (Vector2Int newDirection)
        {
            if (newDirection == -_currentFacingDirection)
            {
                // prevent from turning back
                return;
            }

            _desiredFacingDirection = newDirection;
        }

        private void Move ()
        {
            _currentFacingDirection = _desiredFacingDirection;
            _nextMovementTime = Time.time + _timeBetweenMovements;
            _pieces[0].UpdatePosition (_pieces[0].CurrentGridPosition + _currentFacingDirection);
        }

        private void Grow ()
        {
            SnakePieceController newPiece = Instantiate (_piecePrefab, _pieceContainerTransform).GetComponent<SnakePieceController> ();

            if (_pieces == null)
            {
                _pieces = new List<SnakePieceController> ();
                _pieces.Add (newPiece);
                newPiece.OnCollisionEnter = HandleCollisionOnHead;
            } else
            {
                _pieces[_pieces.Count - 1].AddPieceToTail (newPiece);
                _pieces.Add (newPiece);
            }
        }

        private void HandleCollisionOnHead (Collision2D collision)
        {
            if (collision.gameObject.CompareTag ("Pickup"))
            {
                HandlePickupCollision ();
            } else
            {
                HandleObstacleCollision ();
            }
        }

        private void HandlePickupCollision ()
        {
            Grow ();
            OnPickupCollision?.Invoke ();
        }

        private void HandleObstacleCollision ()
        {
            OnObstacleCollision?.Invoke ();
        }
    }
}