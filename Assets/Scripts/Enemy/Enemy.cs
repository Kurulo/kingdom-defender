using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int goldReward = 20;
    public int goldPenalty = 20;

    private Bank _bank;

    private void Start() {
        _bank = FindObjectOfType<Bank>();
    }
    
    public void RewardGold() {
        if (_bank == null) { return; }
        _bank.AddMoney(goldReward);
    }
    
    public void StealGold() {
        if (_bank == null) { return; }
        _bank.SpendMoney(goldReward);
    }
}
