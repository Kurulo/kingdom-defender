using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour {
    [SerializeField] [Range(0f, 5f)] private float speed = 1f;

    private Enemy _enemy;
    private GridManager _gridManager;
    private PathFinder _pathFinder;
    
    private List<Node> _path = new List<Node>();

    private void OnEnable() {
        ReturnToStart();
        RecalculatePath(true);
    }

    private void Awake() {
        GetComponents();
    }

    private void RecalculatePath(bool resetPath) {
        Vector2Int coordinates = new Vector2Int();
        
        if (resetPath) {
            coordinates = _pathFinder.StartCoordinates;
        } else {
            coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
        }
        StopAllCoroutines();
        _path.Clear();
        _path = _pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }
    
    private void ReturnToStart() {
        transform.position = _gridManager.GetPositionFromCoordinates(_pathFinder.StartCoordinates);
    }
    
    private void FinishPath() {
        _enemy.StealGold();
        gameObject.SetActive(false);
    }
    
    private IEnumerator FollowPath() {
        for(int i = 1; i < _path.Count; i++) {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = _gridManager.GetPositionFromCoordinates(_path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);
            
            while (travelPercent < 1f) {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        
        FinishPath();
    }

    private void GetComponents() {
        _enemy = GetComponent<Enemy>();
        _gridManager = FindObjectOfType<GridManager>();
        _pathFinder = FindObjectOfType<PathFinder>();
    }
}
