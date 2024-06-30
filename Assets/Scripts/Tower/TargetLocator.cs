using System;
using UnityEngine;

public class TargetLocator : MonoBehaviour {
    [SerializeField] private Transform weapon;
    [SerializeField] private ParticleSystem projectileParticles;
    
    [SerializeField] private float range = 15f;
    
    private Transform _target;

    private void Update() {
        FindClosestTarget();
        AimWeapon();
    }
    
    private void FindClosestTarget() {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (var enemy in enemies) {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            
            if (targetDistance < maxDistance) {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        _target = closestTarget;
    }
    
    private void AimWeapon() {
        float targetDistance = Vector3.Distance(transform.position, _target.transform.position);

        weapon.LookAt(_target);
        
        if (targetDistance > range) {
            Attack(false);
        } else {
            Attack(true);
        }
    }
    
    private void Attack(bool isActive) {
        var emission = projectileParticles.emission;
        emission.enabled = isActive;
    }
}
