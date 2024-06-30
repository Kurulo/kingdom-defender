using System;
using UnityEngine;

public class Tile : MonoBehaviour {
    public Tower towerPrefab;
    
    [SerializeField] private bool isPlaceable = true;
    public bool IsPlaceable { get { return isPlaceable; } }

    private GridManager _gridManager;
    private PathFinder _pathFinder;
    
    private Vector2Int _coordinates = new Vector2Int();

    private void Awake() {
        _gridManager = FindObjectOfType<GridManager>();
        _pathFinder = FindObjectOfType<PathFinder>();
    }

    private void Start() {
        if (_gridManager != null) {
            _coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
            
            if (!isPlaceable) {
                _gridManager.BlocNode(_coordinates);
            }
        }
    }

    private void OnMouseDown() {
        bool isWalkable = _gridManager.GetNode(_coordinates).isWalkable;
        bool willBlockPath = _pathFinder.WillBlockPath(_coordinates);
        
        if (isWalkable && !willBlockPath) {
            bool isSuccessful =  towerPrefab.CreateTower(towerPrefab, transform.position);
            
            if (isSuccessful) {
                _gridManager.BlocNode(_coordinates);
                _pathFinder.NotifyReceivers();
            }
        }
    }
}
