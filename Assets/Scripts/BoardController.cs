using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Snake
{
    public class BoardController : MonoBehaviour
    {
        [Header ("Parameters")]
        [SerializeField] private float _gridSpacing = 1.0f;
        [SerializeField] private int _xSize = 48;
        [SerializeField] private int _ySize = 32;

        [Header ("References")]
        [SerializeField] private GameObject _wallGO = null;
        [SerializeField] private GameObject _pickupGO = null;

        private List<Vector2Int> _tilePositions;

        public static BoardController Instance { get; private set; }
        public int TileCount => _xSize * _ySize;
        public float GridSpacing => _gridSpacing;

        private void Awake ()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning ("There's more than one Board in the scene");
            }

            Instance = this;
        }

        private void Start ()
        {
            InstantiateBoundWalls ();
            InitializeTilePositionsArray ();
        }

        private void InitializeTilePositionsArray ()
        {
            _tilePositions = new List<Vector2Int> (_xSize * _ySize);
            for (int x = 0; x < _xSize; x++)
            {
                for (int y = 0; y < _ySize; y++)
                {
                    _tilePositions.Add (new Vector2Int (x, y));
                }
            }
        }

        private void InstantiateBoundWalls ()
        {
            for (int x = -1; x < _xSize + 1; x++)
            {
                InstantiateWall (x, -1);
                InstantiateWall (x, _ySize);
            }

            for (int y = -1; y < _ySize + 1; y++)
            {
                InstantiateWall (-1, y);
                InstantiateWall (_xSize, y);
            }
        }

        private void InstantiateWall (int xPos, int yPos)
        {
            Transform wallTransform = Instantiate (_wallGO, transform).transform;
            wallTransform.position = transform.position + new Vector3 (xPos, yPos) * _gridSpacing;
        }
        
        public void RandomizePickupPosition (Vector2Int[] nonAllowedPositions)
        {
            Vector2Int[] emptyTilePositions = _tilePositions.Except (nonAllowedPositions).ToArray ();
            _pickupGO.transform.position = (Vector2)transform.position + (Vector2)emptyTilePositions[Random.Range (0, emptyTilePositions.Length)] * _gridSpacing;
        }

        private void OnDrawGizmosSelected ()
        {
            for (int x = 0; x < _xSize; x++)
            {
                for (int y = 0; y < _ySize; y++)
                {
                    Gizmos.DrawCube (transform.position + new Vector3 (x, y) * _gridSpacing, Vector2.one * _gridSpacing / 2);
                }
            }
        }
    }
}