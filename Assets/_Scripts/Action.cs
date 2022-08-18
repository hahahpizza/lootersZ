using System;
 
using UnityEngine;
public class Action : MonoBehaviour
{
    public GameObject[] looters;
    public Transform looterPos;
    GameManager _gameManager;
    bool _isStart;
    Vector3 _start;
    Vector3 _end;
    float _time = 0;
    string _from, _to;
    bool _isAgain = false;
    bool _defence; 
    string log = string.Empty;
    string _name;
    bool _isCollid = false;
    string _collider;
    bool _isFall = false;

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

    float high = 6.0f;
    float flySpeed = 1.0f;
    void Update()
    {
        if (_isStart)
        {
            _time += Time.deltaTime;
            Vector3 pos;
            pos = Parabola(_start, _end, high, _time * flySpeed);
            transform.position = pos;

            if (!_isFall)
            {
                if (pos.y <= _end.y && pos.x == _end.x && pos.z == _end.z)
                {
                    _gameManager.AnimationPlay(_name, "Idle");
                    transform.position = _end;

                    if (_collider != null)
                    {
                        GameObject coll = GameObject.Find(_collider);
                        Vector3 pos1 = _end;
                        float dis = 3.0f;
                        pos1.x = _end.x - dis;
                        pos1.z = _end.z + 0;
                        this.gameObject.transform.position = pos1;
                        Vector3 pos2 = coll.transform.position;
                        pos2.x = coll.transform.position.x + dis;
                        pos2.z = coll.transform.position.z - 0;
                        coll.transform.position = pos2;
                    }

                    _isStart = false;
                    if (!_isAgain)
                        _gameManager.OnArrivedTargetPlatform(this.gameObject.name, _from, _to, _isCollid);
                    else
                        _gameManager.OnArrivedBasePlatform(this.gameObject.name);
                }
            }
            else if (_isFall)
            { 
                if (pos.y < -100.0f && pos.x == _end.x && pos.z == _end.z)
                { 
                    transform.position = _gameManager.GetPos(_to);
                    _gameManager.AnimationPlay(_name, "Idle+Flex2");
                    _isFall = false;
                    _isStart = false; 
                }
            }
            
        }
    }
   
    public bool IsDefence()
    {
        return _defence;
    }


    public void RunToTarget(string from, string to, bool isCollid, string collider)
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
        _collider = collider;
        _start = _gameManager.GetPos(from);
        _end = _gameManager.GetPos(to);

        if (isCollid)
        {
            GameObject coll = GameObject.Find(collider);
            //float angle = Vector3.Angle(this.transform.forward, coll.transform.forward);
            //print(angle);

            if (coll.transform.position.x < this.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
                coll.transform.eulerAngles = new Vector3(0, -90, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, -90, 0);
                coll.transform.eulerAngles = new Vector3(0, 90, 0);
            }

        }

        _isAgain = false;
        _isStart = true;
    }

    public void RunBack(string from, string to, int[] looters_)
    {
        _time = 0;
        _from = from;
        _to = to;
        _start = _gameManager.GetPos(from);
        _end = _gameManager.GetPos(to);
        if (from.Contains("safezone"))
        {
            string who = _gameManager.Safebase2Player(from);
            _gameManager.ChangeParent2(who, from, "Coin");
        }
        for (int i = 0; i < 4; i++)
        {
            int cnt = looters_[i];
            for (int j = 0; j < cnt; j++)
            {
                Vector3 v3 = looterPos.position;
                v3.x = looterPos.position.x + UnityEngine.Random.Range(-0.5f, 0.5f);
                v3.z = looterPos.position.z + UnityEngine.Random.Range(-0.5f, 0.5f);
                GameObject clone = GameObject.Instantiate(looters[i], v3, Quaternion.identity, this.transform) as GameObject;
                float scale = 1 + j * 0.1f;
                clone.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        _isAgain = true;
        _isStart = true;
    }


    public void RunBack(string from, string to )
    {
        _time = 0;
        _from = from;
        _to = to;
        _start = _gameManager.GetPos(from);
        _end = _gameManager.GetFallPos(from);
           
        _isFall = true;
        _isStart = true;
    }


    private void OnCollisionStay(Collision collision)
    {
        bool isplayer = _gameManager.IsPlayer(collision.gameObject.name);
        if (isplayer)
        { 
            Vector3 rot = collision.transform.eulerAngles;
            rot.y = 180 + collision.transform.eulerAngles.y;
            // this.transform.eulerAngles = rot; 
        }
    } 

}
