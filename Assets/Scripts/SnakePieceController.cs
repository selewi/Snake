using UnityEngine;
using UnityEngine.Events;

namespace Snake
{
    [RequireComponent (typeof (Collider2D))]
    public class SnakePieceController : MonoBehaviour
    {
        public UnityAction<Collision2D> OnCollisionEnter = null;

        private SnakePieceController _nextPiece = null;

        public Vector2Int CurrentGridPosition { get; private set; } = Vector2Int.zero;

        public void UpdatePosition (Vector2Int newPosition)
        {
            if (_nextPiece != null)
            {
                _nextPiece.UpdatePosition (CurrentGridPosition);
            }

            CurrentGridPosition = newPosition;
            transform.position = (Vector2)CurrentGridPosition * BoardController.Instance.GridSpacing;
        }

        public void AddPieceToTail (SnakePieceController tailPiece)
        {
            _nextPiece = tailPiece;
            tailPiece.UpdatePosition (CurrentGridPosition);
        }

        private void OnCollisionEnter2D (Collision2D collision)
        {
            OnCollisionEnter?.Invoke (collision);
        }
    }
}