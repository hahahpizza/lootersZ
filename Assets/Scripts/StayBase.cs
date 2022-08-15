using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StayBase : MonoBehaviour
{
    GameManager _gameManager;
    public int[] safe_looters;
    public int[] _looters;
    public string _name;
    private void Awake()
    {
        _looters = new int[4];
        safe_looters = new int[4];
    }
    void Start()
    {
        _name = this.gameObject.name;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SetMoney(int[] looters)
    {
        this._looters = looters;
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            obj.SetActive(false);
            DestroyImmediate(obj);
        }
    }

    public void SetMoney(int n)
    {
        this._looters = new int[4] { 0,0,0,0 };
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            obj.SetActive(false);
            DestroyImmediate(obj);
        }
    }

    public void AddLooters(int[] looters)
    { 
        for(int i = 0; i < 4; i++)
        {
            _looters[i] += looters[i];
        }
    } 
    public int[] GetLooters()
    {
        return this._looters;
    }
    public int[] GetSafeLooters()
    {
        return safe_looters;
    }
    public int GetSafeMoney()
    {
        int[] prices = _gameManager.GetPrices();
        int res = 0;
        for (int i = 0; i < 4; i++)
        {
            res += safe_looters[i] * prices[i];
        }
        return res;
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

    public int GetAllMoney()
    { 
        return GetMoney() + GetSafeMoney();
    }
    public void SetSafeLooters()
    {
        for(int i = 0; i < 4; i++)
        {
            safe_looters[i] += _looters[i];
            _looters[i] = 0;
        } 
        //string playerName = _gameManager.getPlayerFromStore(_name);
        //string from = _name;
        //string to = playerName; 
        //_gameManager.ChangeParent(from, to);
    }
     
    void OnMouseDown()
    {
        string to = _name;
       _gameManager.OnMouseEventByClickObjs(to); 
    }
}
