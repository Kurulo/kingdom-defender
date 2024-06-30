using System;
using TMPro;
using UnityEngine;

public class Bank : MonoBehaviour {

    [Header("Bank Settings")]
    [SerializeField] private int startMoney = 150;
    [SerializeField] private int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    private SceneMenager _sceneManager;
    public TextMeshProUGUI _balanceText;
    
    private void Awake() {
        currentBalance = startMoney;
        _sceneManager = FindObjectOfType<SceneMenager>();

        ShowBalance();
    }

    public void AddMoney(int deposit) {
        currentBalance += deposit;
        ShowBalance();
    }
    
    public bool CanSpendMoney(int amount) {
        if (amount <= currentBalance && currentBalance != 0) {
            return true;
        } else {
            return false;
        }
    }
    
    public void SpendMoney(int amount) {
        currentBalance -= amount;
        ShowBalance();
        if (currentBalance < 0) {
            _sceneManager.ReloadScene();
        }
    }
    
    private void ShowBalance() {
        _balanceText.text = "GOLD: " + currentBalance;
    }
}
