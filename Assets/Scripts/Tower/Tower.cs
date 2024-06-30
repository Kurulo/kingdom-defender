using System;
using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour {
    public int cost = 50;

    [SerializeField] private float buildDelay = 1f;
    
    private Bank _bank;

    private void Start() {
        StartCoroutine(Build());
    }

    public bool CreateTower(Tower tower, Vector3 position) {
        _bank = FindObjectOfType<Bank>();
        
        if (_bank == null) { return false; }
        
        if (_bank.CanSpendMoney(cost)) {
            _bank.SpendMoney(cost);
            Instantiate(tower, position, Quaternion.identity);
            return true;
        } 
        
        return false;
    }
    
    private IEnumerator Build() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
            
            foreach (Transform grandChild in child) {
                grandChild.gameObject.SetActive(false);
            }
        }
        
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach (Transform grandChild in child) {
                grandChild.gameObject.SetActive(true);
            }
        }
    }
}
