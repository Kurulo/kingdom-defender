using System;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour {
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color blockedColor = Color.red;
    [SerializeField] private Color exploreColor = Color.yellow;
    [SerializeField] private Color pathColor = new Color(1f, 0.5f, 0f);
    
    private TextMeshPro _lable;
    private Vector2Int _coordinates = new Vector2Int();
    private Transform _parentTransform;

    private GridManager _gridManager;

    private void Awake() {
        GetComponents();
        
        _lable.enabled = true;
        
        DisplayCoordinates();
        UpdateObjectName();
    }

    private void Update() {
        if (!Application.isPlaying) {
            DisplayCoordinates();
            UpdateObjectName();
            _lable.enabled = true;
        }
        
        SetLabelColor();
        ToggleLabels();
    }
    
    private void ToggleLabels() {
        if (Input.GetKeyDown(KeyCode.C)) {
            _lable.enabled = !_lable.IsActive();
        }
    }
     
    private void SetLabelColor() {
        if (_gridManager == null) { return; }

        Node node = _gridManager.GetNode(_coordinates);

        if (node == null) { return; }
        
        if (!node.isWalkable) {
            _lable.color = blockedColor;
        } else if (node.isPath) {
            _lable.color = pathColor;
        } else if (node.isExplored) {
            _lable.color = exploreColor;
        } else {
            _lable.color = defaultColor;
        }
    }
    
    private void DisplayCoordinates() {
        float snapMoveX = UnityEditor.EditorSnapSettings.move.x;
        float snapMoveZ = UnityEditor.EditorSnapSettings.move.z;
            
        _coordinates.x = Mathf.RoundToInt(_parentTransform.position.x / snapMoveX);
        _coordinates.y = Mathf.RoundToInt(_parentTransform.position.z / snapMoveZ);

        _lable.text = $"{_coordinates.x}, {_coordinates.y}";
    }

    private void UpdateObjectName() {
        _parentTransform.name = _coordinates.ToString();
    }
    
    private void GetComponents() {
        _gridManager = FindObjectOfType<GridManager>();
        _lable = GetComponent<TextMeshPro>();
        _parentTransform = transform.parent;
    }
}
