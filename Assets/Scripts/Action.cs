using System;
 
using UnityEngine;
public class Action : MonoBehaviour
{
    public GameObject[] looters;
    public Transform looterPos;
    GameManager _gameManager;
    public bool _isStart;
    public Vector3 _start;
    public Vector3 _end;
    public float _time = 0;
    public string _from, _to;
    public bool _isAgain = false;
    public bool _defence; 
    public string log = string.Empty;
    public string _name;
    bool _isCollid = false;

    void Start()
    {
        _name = this.gameObject.name;
        _defence = false;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        _start = transform.position;        
    }

    float Func(float x, float height)
    {
        return -4 * height * x * x + 4 * height * x;
    }
    public Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        var mid = Vector3.Lerp(start, end, t);
        return new Vector3(mid.x, Func(t, height) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
      
    void Update()
    {
        if (_isStart)
        {
            _time += Time.deltaTime;
            Vector3 pos;
            pos = Parabola(_start, _end, 7.0f, _time / 1);
            transform.position = pos;

            if (pos.y <= _end.y && pos.x == _end.x && pos.z == _end.z)//
            {
                _gameManager.AnimationPlay(_name, "Idle");
                pos = _end;
                float dy = 0;
                pos.y = _end.y + dy;
                transform.position = pos;
                _isStart = false;

                if (!_isAgain)
                    _gameManager.OnArrivedTargetPlatform(this.gameObject.name, _from, _to, _isCollid);

                else
                {
                    _gameManager.OnArrivedBasePlatform(this.gameObject.name);  
                }
                //_isAgain ^= true;
            }
        }
    }
   
    public bool IsDefence()
    {
        return _defence;
    }
    
    public void RunToTarget(string from, string to, bool isCollid)
    {
        _gameManager.AnimationPlay(_name, "JumpFalling"); 
        if (to.Contains("safezone"))
        {
            string playerName = _gameManager.GetPlayerByStore(from);
            _gameManager.ChangeParent(from, playerName);
        }
        _gameManager.log = "";
        _defence = (from == to);
             
        _time = 0;
        _from = from;
        _to = to;
        _isCollid = isCollid;
        _start = _gameManager.GetPos(from);
        _end = _gameManager.GetPos(to);

        
        if (isCollid)
        {
            //float dis = _gameManager.GetShortDis(_name);
            float range = 2.00f;
            Vector3 pos;
            pos.x = _end.x + UnityEngine.Random.Range(-range, range);
            pos.y = _end.y;
            pos.z = _end.z + UnityEngine.Random.Range(-range, range);
            _end = pos;
        }
        
        _isAgain = false;
        _isStart = true;
    }

    public void RunBack(string from, string to, int[] mArr)
    {
        //print(from);
        _time = 0;
        _from = from;
        _to = to; 
        _start = _gameManager.GetPos(from);       
        _end = _gameManager.GetPos(to);
        if(from.Contains("safezone"))
        {
            string who = _gameManager.GetPlayerBySafezone(from);
            _gameManager.ChangeParent2(who, from, "Coin");
        }
        for (int i = 0; i < 4; i++)
        {
            int cnt = mArr[i];
            for (int j = 0; j < cnt; j++)
            {
                Vector3 v3 = looterPos.position;
                v3.x = looterPos.position.x + UnityEngine.Random.Range(-1.5f, 1.5f);
                v3.z = looterPos.position.z + UnityEngine.Random.Range(-1.5f, 1.5f);
                GameObject clone = GameObject.Instantiate(looters[i], v3, Quaternion.identity, this.transform) as GameObject;
                float scale = 1 + j * 0.1f;                
                clone.transform.localScale = new Vector3(scale, scale, scale);                 
            }             
        }  
        _isAgain = true;
        _isStart = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        bool isplayer = _gameManager.IsPlayer(collision.gameObject.name);
        if (isplayer)
        {
            //transform.LookAt(collision.transform, Vector3.up);
            Vector3 rot = collision.transform.eulerAngles;
            rot.y = 180 + collision.transform.eulerAngles.y;
            this.transform.eulerAngles = rot;


        }
    } 

}
