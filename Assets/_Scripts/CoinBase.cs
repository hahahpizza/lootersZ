using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CoinBase : MonoBehaviour
{
    GameManager _gameManager;
    int[] _looters;
    string to;

    private void Awake()
    {
        _looters = new int[4];
    }
    void Start()
    {
        to = this.gameObject.name;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }  

    public void SetLooters(int[] looters)
    {
        this._looters = looters;
    }

    public int[] GetLooters()
    {
        return this._looters;
    }
    public int GetMoney()
    {
        int[] prices = _gameManager.GetPrices();
        int res = 0;
        for (int i = 0; i < 4; i++)
        {
            res += _looters[i] * prices[i];
        }
        return res; 
    }
    
    void OnMouseDown()
    { 
        _gameManager.OnMouseEventByClickObjs(to);
    }
}
