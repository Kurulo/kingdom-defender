using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour {
    [SerializeField] private Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }
    [SerializeField] private Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }
    
    private Node _currentSearchNode;
    private Node _startNode;
    private Node _destinationNode;

    private GridManager _gridManager;
    
    private Vector2Int[] _directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    
    private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();
    private Dictionary<Vector2Int, Node> _reached = new Dictionary<Vector2Int, Node>();

    private Queue<Node> _frontien = new Queue<Node>();

    private void Awake() {
        _gridManager = FindObjectOfType<GridManager>();
        
        if (_gridManager != null) {
            _grid = _gridManager.Grid;
            StartSetNodes();
        }
    }

    private void Start() {
        GetNewPath();
    }   
    
    public List<Node> GetNewPath() {
        return GetNewPath(startCoordinates);
    }
    
    public List<Node> GetNewPath(Vector2Int coordinates) {
        _gridManager.ResetNodes();
        BreathFirstSearch(coordinates);
        return BuildPath();
    }
        
    public bool WillBlockPath(Vector2Int coordinates) {
        if (_grid.ContainsKey(coordinates)) {
            bool previousState = _grid[coordinates].isWalkable;
            
            _grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            _grid[coordinates].isWalkable = previousState;
            
            if (newPath.Count <= 1) {
                GetNewPath();
                return true;
            }

            return false;
        }

        return false;
    }
    
    public void NotifyReceivers() {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }

    private void ExploreNeighbors() {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int direction in _directions) {
            Vector2Int neighborCoordinates = _currentSearchNode.coordinates + direction;
            
            if (_grid.ContainsKey(neighborCoordinates)) {
                neighbors.Add(_grid[neighborCoordinates]);
            }
        }

        foreach (Node neighbor in neighbors) {
            if (!_reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable) {
                neighbor.connectedTo = _currentSearchNode;
                _reached.Add(neighbor.coordinates, neighbor);
                _frontien.Enqueue(neighbor);
            }
        }
    }
    
    private void BreathFirstSearch(Vector2Int coordinates) {
        _startNode.isWalkable = true;
        _destinationNode.isWalkable = true;
        
        _frontien.Clear();
        _reached.Clear();
        
        bool isRunning = true;
        
        _frontien.Enqueue(_grid[coordinates]);
        _reached.Add(coordinates, _grid[coordinates]);

        while (_frontien.Count > 0 && isRunning) {
            _currentSearchNode = _frontien.Dequeue();
            _currentSearchNode.isExplored = true;
            ExploreNeighbors();
            
            if (_currentSearchNode.coordinates == destinationCoordinates) {
                isRunning = false;
            }
        }
    }
    
    private void StartSetNodes() {
        _startNode = _gridManager.Grid[startCoordinates];
        _destinationNode = _gridManager.Grid[destinationCoordinates];
    }
    
    private List<Node> BuildPath() {
        List<Node> path = new List<Node>();
        Node currentNode = _destinationNode;
        
        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null) {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }
        
        path.Reverse();

        return path;
    }
}
