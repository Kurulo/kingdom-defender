using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return _grid; } }

    private float _snapMoveX; 
    private float _snapMoveZ; 

    private void Awake() {
        CreateGrid();
    }

    private void Start() {
        _snapMoveX = UnityEditor.EditorSnapSettings.move.x;
        _snapMoveZ = UnityEditor.EditorSnapSettings.move.z;
    }

    public Node GetNode(Vector2Int coordinates) {
        if (_grid.ContainsKey(coordinates)) {
            return _grid[coordinates];
        }

        return null;
    }
    
    public void BlocNode(Vector2Int coordinates) {
        if (_grid.ContainsKey(coordinates)) {
            _grid[coordinates].isWalkable = false;
        }
    }
    
    public void ResetNodes() {
        foreach (KeyValuePair<Vector2Int, Node> entry in _grid) {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }
    
    public Vector2Int GetCoordinatesFromPosition(Vector3 position) {
        Vector2Int coordinates = new Vector2Int();

        coordinates.x = Mathf.RoundToInt(position.x / _snapMoveX);
        coordinates.y = Mathf.RoundToInt(position.z / _snapMoveZ);

        return coordinates;
    }
    
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates) {
        Vector3 position = new Vector3();
        
        position.x = coordinates.x * _snapMoveX;
        position.z = coordinates.y * _snapMoveZ;

        return position;
    } 
    
    private void CreateGrid() {
        for (int x = 0; x < gridSize.x; x++) {
            for (int y = 0; y < gridSize.y; y++) {
                Vector2Int coordinates = new Vector2Int(x, y);
                _grid.Add(coordinates, new Node(coordinates, true));
                Debug.Log(_grid[coordinates].coordinates + " - " + _grid[coordinates].isWalkable);
            }
        }
    }
}
