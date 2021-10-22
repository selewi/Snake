using UnityEngine;
using UnityEngine.Events;

namespace Snake
{
    public class InputManager : MonoBehaviour
    {
        public UnityAction<Vector2Int> OnMoveAxisKeyPress = null;

        [SerializeField] private KeyCode _moveUpKeyCode = KeyCode.UpArrow;
        [SerializeField] private KeyCode _moveDownKeyCode = KeyCode.DownArrow;
        [SerializeField] private KeyCode _moveLeftKeyCode = KeyCode.LeftArrow;
        [SerializeField] private KeyCode _moveRightKeyCode = KeyCode.RightArrow;

        public static InputManager Instance { get; private set; }

        private void Awake ()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning ("There's more than one InputManager in the scene");
            }

            Instance = this;
        }

        private void Update ()
        {
            if (Input.GetKeyDown (_moveUpKeyCode))
            {
                OnMoveAxisKeyPress?.Invoke (Vector2Int.up);
                return;
            }

            if (Input.GetKeyDown (_moveDownKeyCode))
            {
                OnMoveAxisKeyPress?.Invoke (Vector2Int.down);
                return;
            }

            if (Input.GetKeyDown (_moveLeftKeyCode))
            {
                OnMoveAxisKeyPress?.Invoke (Vector2Int.left);
                return;
            }

            if (Input.GetKeyDown (_moveRightKeyCode))
            {
                OnMoveAxisKeyPress?.Invoke (Vector2Int.right);
            }
        }
    }
}
