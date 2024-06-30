using System;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour { 
    [SerializeField] private int maxHealthPoints = 5;
    [SerializeField] private int difficultyRamp = 1;
    
    [SerializeField] private int _healthPoint = 0;
     
    private Enemy _enemy;
    
    private void OnEnable() {
        _healthPoint = maxHealthPoints;
    }

    private void Start() {
        _enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other) {
        ProcessHit();
    }
    
    private void ProcessHit() {
        _healthPoint--;
        
        if (_healthPoint <= 0) {
            _enemy.RewardGold();
            maxHealthPoints += difficultyRamp;
            gameObject.SetActive(false);
        }
    }
}
