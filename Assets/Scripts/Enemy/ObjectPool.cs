using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPool : MonoBehaviour {
    public GameObject enemy;

    [Range(0, 50)] public int poolSize = 5;
    [Range(0.1f, 20f)] public float spawnRate = 1f;
    public bool randomRate = false;

    private GameObject[] pool;

    private void Awake() {
        PopulatePool();
    }

    private void Start() {
        GetComponents();
        StartCoroutine(SpawnEnemy());
    }
    
    private IEnumerator SpawnEnemy() {
        while (true) {
            EnableObjectInPool();
            if (!randomRate) {
                yield return new WaitForSeconds(spawnRate);
            } else {
                yield return new WaitForSeconds(Random.Range(1f, 4f));
            }
        }
    }
    
    private void PopulatePool() {
        pool = new GameObject[poolSize];
        
        for (int i = 0; i < pool.Length; i++) {
            pool[i] = Instantiate(enemy, transform);
            pool[i].SetActive(false);
        }
    }
    
    private void EnableObjectInPool() {
        for (int i = 0; i < pool.Length; i++) {
            if (pool[i].activeInHierarchy == false) {
                pool[i].SetActive(true);
                return;
            }
        }
    }
    
    private void GetComponents() {
        
    }
}
