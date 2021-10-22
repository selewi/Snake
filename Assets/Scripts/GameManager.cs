using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snake
{
    public class GameManager : MonoBehaviour
    {
        private SnakeController _snakeController;

        private void Awake ()
        {
            _snakeController = FindObjectOfType<SnakeController> ();
        }

        private void Start ()
        {
            _snakeController.OnObstacleCollision += HandleSnakeObstacleCollision;
            _snakeController.OnPickupCollision += HandleSnakePickupCollision;
            InputManager.Instance.OnMoveAxisKeyPress += _snakeController.TryUpdateDirection;
            BoardController.Instance.RandomizePickupPosition (_snakeController.OccupiedPositions);
        }

        private void HandleSnakeObstacleCollision ()
        {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        }

        private void HandleSnakePickupCollision ()
        {
            Vector2Int[] occupiedPositions = _snakeController.OccupiedPositions;
            if (occupiedPositions.Length == BoardController.Instance.TileCount)
            {
                Debug.Log ("You won!");
                return;
            }
            BoardController.Instance.RandomizePickupPosition (occupiedPositions);
        }
    }
}