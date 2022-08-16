
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string log = "-------- for debug ---------\r\n";
    public GameObject[] players;
    public GameObject[] coins;
    public GameObject[] boards;
    public GameObject[] safezones;
    public int[] moneyArr;
    List<Vector3> abs_board_positions;
    List<Vector3> abs_coin_positions;
    List<Vector3> abs_sz_positions;
    bool _exit = false;
    int signals = 0;
    int round = 0;
    int round_limit = 5;
    bool _isStart = false;
    
    string[] froms;
    string[] tos;
    string[] whos;     
    float _tim = 0.0f;
    string log2;
    float hSliderValue;

    List<int[]> lootersArr ;
    bool[] isCollids;

    private void Start()
    {
        abs_board_positions = new List<Vector3>();
        abs_coin_positions = new List<Vector3>();
        abs_sz_positions = new List<Vector3>();      
        whos = new string[4] { "bot1", "bot2", "bot3", "user" };
        froms = new string[4];
        tos = new string[4];
        lootersArr = new List<int[]>();
        isCollids = new bool[4] { false, false, false, false };

        foreach (GameObject player in players)
        {
            Transform trans = player.transform;
            abs_board_positions.Add(trans.position);
        }
        for (int i = 0; i < 4; i++)
        {
            GameObject coin = coins[i];
            Transform trans = coin.transform;
            Vector3 pos = trans.position;
            pos.y = abs_board_positions[0].y;
            abs_coin_positions.Add(pos);
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject safezone = safezones[i];
            Transform trans = safezone.transform;
            Vector3 pos = trans.position;
            pos.y = abs_board_positions[0].y;
            abs_sz_positions.Add(pos);
        }
        SetCoinLooters();//init 
    }

    public int[] GetPrices()
    {
        return moneyArr;
    }
    int GetRandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }

    int[] GetRandomInts(int min, int max)
    {
        int[] ns = new int[4];
        ns[0] = GetRandomInt(min, max);
        while (true)
        {
            int n = GetRandomInt(min, max);
            if (n != ns[0])
            {
                ns[1] = n;
                break;
            }
        }
        while (true)
        {
            int n = GetRandomInt(min, max);
            if (n != ns[0] && n != ns[1])
            {
                ns[2] = n;
                break;
            }
        }
        while (true)
        {
            int n = GetRandomInt(min, max);
            if (n != ns[0] && n != ns[1] && n != ns[2])
            {
                ns[3] = n;
                break;
            }
        }
        return ns;
    }


    void SetCoinLooters()
    {
        GameObject.Find("coin1").GetComponent<CoinBase>().SetLooters(new int[4] { 1, 0, 0, 0 });
        GameObject.Find("coin2").GetComponent<CoinBase>().SetLooters(new int[4] { 0, 1, 0, 0 });
        GameObject.Find("coin3").GetComponent<CoinBase>().SetLooters(new int[4] { 0, 0, 1, 0 });
        GameObject.Find("coin4").GetComponent<CoinBase>().SetLooters(new int[4] { 0, 0, 0, 1 });

        int[] rs = GetRandomInts(1, 5);
        for (int i = 1; i < 5; i++)
        {
            int r = rs[i - 1];
            Vector3 pos = abs_coin_positions[r - 1];
            pos.y = -2.0f;
            GameObject.Find("coin" + i).transform.position = pos;
        }
    }
 
     

    public int Count()
    {
        round++;
        if (round > round_limit)
        {
            round = round_limit;
            _exit = true;
        }
        return round;
    }

    protected string GetCollidePlayers(string s1, string s2)
    {
        int n1 = GetIndexPlayer(s1);
        int n2 = GetIndexPlayer(s2);
        if (n2 > n1)
            return "\n Collide: " + s1 + " + " + s2;
        else
            return "\n Collide: " + s2 + " + " + s1;
    }

    protected int GetIndexPlayer(string name)
    {
        for (int i = 0; i < 4; i++)
        {
            string s = whos[i];
            if (name == s) return i;
        }
        return 0;
    }

    public string GetBaseNameByPlayer(string userName)
    {
        switch (userName)
        {
            case "bot1": return "bd1";
            case "bot2": return "bd2";
            case "bot3": return "bd3";
            case "user": return "bd4";
            default: return string.Empty;
        }
    }

    public List<GameObject> GetChildrens(GameObject parent)
    {
        List<GameObject> objs = new List<GameObject>();
        int cnt = parent.transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            Transform tf = parent.transform.GetChild(i);
            GameObject obj = tf.gameObject;
            objs.Add(obj);
        }
        return objs;
    }

    public string GetPlayerByStore(string storeName)
    {
        switch (storeName)
        {
            case "bd1": return "bot1";
            case "bd2": return "bot2";
            case "bd3": return "bot3";
            case "bd4": return "user";
            default: return string.Empty;
        }
    }
    protected List<GameObject> GetChildrens(string parentName)
    {
        GameObject parent = GameObject.Find(parentName);
        List<GameObject> objs = new List<GameObject>();
        int cnt = parent.transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            Transform tf = parent.transform.GetChild(i);
            GameObject obj = tf.gameObject;
            objs.Add(obj);
        }
        return objs;
    }


    public void ChangeParent(string from, string to)
    {
        GameObject from_ = GameObject.Find(from);
        GameObject to_ = GameObject.Find(to);
        List<GameObject> childs = GetChildrens(from_);
        int cnt = childs.Count;

        for (int i = 0; i < cnt; i++)
        {
            GameObject child = childs[i];
            //if (child.CompareTag(compareTag))
            {
                Vector3 v3 = child.transform.position;
                v3.x = child.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f);
                v3.z = child.transform.position.z + UnityEngine.Random.Range(-0.5f, 0.5f);
                v3.y = -1.2f;
                child.transform.position = v3;
                child.transform.SetParent(to_.transform, true);
            }
        }
    }
    public void ChangeParent1(string from, string to, string compareTag)
    {
        GameObject from_ = GameObject.Find(from);
        GameObject to_ = GameObject.Find(to);
        List<GameObject> childs = GetChildrens(from_);
        int cnt = childs.Count;
        if (cnt > 1)
        {
            for (int i = 0; i < cnt; i++)
            {
                GameObject child = childs[i];
                if (child.CompareTag(compareTag))
                {
                    Vector3 v3 = child.transform.position;
                    v3.x = child.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f);
                    v3.z = child.transform.position.z + UnityEngine.Random.Range(-0.5f, 0.5f);
                    v3.y = -1.2f;
                    child.transform.position = v3;
                    child.transform.SetParent(to_.transform, true);
                }
            }
        }
    }

    public void ChangeParent2(string from, string to, string compareTag)
    {
        GameObject from_ = GameObject.Find(from);
        GameObject to_ = GameObject.Find(to);
        List<GameObject> childs = GetChildrens(from_);
        int cnt = childs.Count;
        if (cnt > 1)
        {
            for (int i = 0; i < cnt; i++)
            {
                GameObject child = childs[i];
                if (child.CompareTag(compareTag))
                {
                    //Vector3 v3 = child.transform.position;
                    //v3.y = -1.2f;
                    //v3.x = v3.x - 5.0f;
                    
                    Vector3 v3 = to_.transform.position;
                    v3.x = to_.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f);
                    v3.z = to_.transform.position.z + UnityEngine.Random.Range(-0.5f, 0.5f);
                    v3.y = to_.transform.position.y;
                    child.transform.position = v3; 
                    child.transform.SetParent(to_.transform, true);
                }
            }
        }
    }

     

    public string GetPlayerBySafezone(string safezone)
    {
        switch (safezone)
        {
            case "safezone1": return "bot1";
            case "safezone2": return "bot2";
            case "safezone3": return "bot3";
            case "safezone4": return "user";
            default: return string.Empty;
        }
    }
    public void AnimationPlay(string who, string type)
    { 
        string animObjName = GetAnimObjByName(who);
        GameObject animObj = GameObject.Find(animObjName);
        Animation anim = animObj.GetComponent<Animation>();
        anim.Play("Armature|" + type);
    }
    

    int Looters2Money(int[] looters)
    {
        int res = 0;
        for (int i = 0; i < 4; i++)
        {
            int cnt = looters[i];
            res = cnt * moneyArr[i];
        }
        return res;
    }

    public bool IsPlayer(string playerName )
    {
        switch(playerName)
        {
            case "bot1": case "bot2": case "bot3": case "user": return true;
            default: return false;
        }
    }
   
    private void FixedUpdate()
    {
        if( _isStart )
        {
            _tim += Time.deltaTime; 
            if ( _tim > 1.5f )
            { 
                for (int i = 0; i < 4; i++)
                {
                    string from_ = froms[i];
                    string to_ = tos[i];
                    string who_ = whos[i];
                    bool isColl_ = isCollids[i];
                    int[] looters = lootersArr[i];
                    AnimationPlay(who_, "JumpFalling");
                    string baseName = GetBaseNameByPlayer(who_);
                    // int[] looters = GameObject.Find(baseName).GetComponent<StayBase>().GetLooters();

                    if(isColl_) looters = new int[4]{0,0,0,0};
                    GameObject.Find(who_).GetComponent<Action>().RunBack(to_, from_, looters);
                }
                SetCoinLooters();
                isCollids = new bool[4]{false, false, false, false}; 
                lootersArr = new List<int[]>();
                signals = 0;
                _tim = 0;
                _isStart = false;
            }
        }
    }
   

    string GetStayBaseBySafeBase(string safebase)
    {
        switch(safebase)
        {
            case "safezone1": return "bd1";
            case "safezone2": return "bd2";
            case "safezone3": return "bd3";
            case "safezone4": return "bd4";
            default: return string.Empty;
        }
    }

    public void OnArrivedBasePlatform(string who)
    {
        string storeName = this.GetBaseNameByPlayer(who);
        ChangeParent1(who, storeName, "Coin");
    }
 
 
  public void OnArrivedTargetPlatform(string who, string from, string to, bool isCollid)
    {  
        AnimationPlay(who, "JumpLanding");
        log += "\n" + who + ">" + to + "(" + isCollid + ")";
        if(to.Contains("safezone"))
        {
            string staybase = GetStayBaseBySafeBase(to);
            GameObject.Find(staybase).GetComponent<StayBase>().SetSafeLooters();
        } 
        signals++;
        int index = GetIndexPlayer(who);
        froms[index] = from;
        tos[index] = to;        
        isCollids[index] = isCollid;
        if (signals != 4) return;  
        for (int i = 0; i < 4; i++)
        { 
            string to_ = tos[i];
            string who_ = whos[i];
            bool is_Coll = isCollids[i]; 
            int[] looters = new int[4] { 0, 0, 0, 0 };
            if (!is_Coll)
            {
                GameObject to_obj = GameObject.Find(to_);
                if (to_.Contains("bd"))
                {
                    // looters = to_obj.GetComponent<StayBase>().GetLooters();
                    to_obj.GetComponent<StayBase>().StealedBy(who_); 

                }
                else if (to_.Contains("coin"))
                {
                    looters = to_obj.GetComponent<CoinBase>().GetLooters();
                }
                
                int money = Looters2Money(looters);                 
                if (money == 0)
                    AnimationPlay(who_, "Lose");
                else
                {
                    if (money > 0 && money < 5000) AnimationPlay(who_, "HappyIdle");
                    else if(money >= 5000 && money < 10000)
                        AnimationPlay(who_, "HappyIdle" + GetRandomInt(1, 5));                     
                    else if (money >= 10000) AnimationPlay(who_, "Win");
                }
                GameObject.Find("bd" + (i + 1)).GetComponent<StayBase>().AddLooters(looters);
                lootersArr.Add(looters);
            }
            else
            {
                lootersArr.Add(new int[4] { 0, 0, 0, 0 });
                int random = GetRandomInt(1, 5);
                AnimationPlay(who_, "Fight" + random);
            }
        }        
        _tim = 0;
        _isStart = true;
    }

    public Vector3 GetPos(string name)
    {
        Vector3 pos;
        switch (name)
        {
            case "coin1":
                pos = GameObject.Find(name).transform.position;
                pos.y = abs_coin_positions[0].y;
                return pos;

            case "coin2":
                pos = GameObject.Find(name).transform.position;
                pos.y = abs_coin_positions[0].y;
                return pos;
            case "coin3":
                pos = GameObject.Find(name).transform.position;
                pos.y = abs_coin_positions[0].y;
                return pos;
            case "coin4":
                pos = GameObject.Find(name).transform.position;
                pos.y = abs_coin_positions[0].y;
                return pos;

            case "bd1": return abs_board_positions[0];
            case "bd2": return abs_board_positions[1];
            case "bd3": return abs_board_positions[2];
            case "bd4": return abs_board_positions[3];
            case "safezone1": return abs_sz_positions[0];
            case "safezone2": return abs_sz_positions[1];
            case "safezone3": return abs_sz_positions[2];
            case "safezone4": return abs_sz_positions[3];
            default: return Vector3.zero;
        }
    }

    public string GetAnimObjByName(string name_)
    {
        switch (name_)
        {
            case "bot1": return "ch1";
            case "bot2": return "ch2";
            case "bot3": return "ch3";
            case "user": return "ch4";
            default: return string.Empty;
        }
    }
 
    public void OnMouseEventByClickObjs(string to)
    {
         
        this.Count();
        if (_exit) return;
        froms[0] = "bd1";
        froms[1] = "bd2";
        froms[2] = "bd3"; 
        froms[3] = "bd4";
        tos[0] = this.GetEnableTarget(1);
        tos[1] = this.GetEnableTarget(2);
        tos[2] = this.GetEnableTarget(3); 
        tos[3] = to;         
        bool[] isColls = new bool[4] { false, false, false, false };
        for (int i = 0; i < 4; i++)
        {
            string to1 = tos[i];             
            for (int j = 0; j < 4; j++)
            {                 
                string to2 = tos[j];
                if (i != j && to1 == to2)
                {            
                    isColls[i] = true;
                    isColls[j] = true;
                } 
            }
        }         
        for(int i = 0; i < 4; i++)
            players[i].GetComponent<Action>().RunToTarget(froms[i], tos[i], isColls[i]);        
        
    }

    public float GetShortDis(string from)
    { 
        float min = 99999.99f;
        foreach (GameObject player in players)
        {
            if (player.name != from)
            {
                float dis = Vector3.Distance(player.transform.position, GameObject.Find(from).transform.position);
                if (dis < min)
                    min = dis;
            }
        }
        return min;
    }
    public string GetEnableTarget(int botNum)
    {
        int n = GetRandomInt(1, 10);
        switch (n)
        {
            case 1: return "coin1";
            case 2: return "coin2";
            case 3: return "coin3";
            case 4: return "coin4";
            case 5: return "bd1";
            case 6: return "bd2";
            case 7: return "bd3";
            case 8: return "bd4";
            case 9: return "safezone" + botNum;
            default: return string.Empty;
        }
    }
 
    void OnGUI()
    {
        bool debug = true;
        GUI.color = Color.white;
        int wide = Screen.width - 200, high = Screen.height - 100;

        if (!_exit)
        {
            hSliderValue = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 50, 50, 100, 30), hSliderValue, 1.0F, 10.0F);
            if (GUI.Button(new Rect(Screen.width / 2 - 35, 10, 70, 30), "Format"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }

        else
        {
            GUI.Box(new Rect(Screen.width / 2 - wide / 2, Screen.height / 2 - high / 2, wide, high), "");
            wide = 100; high = 50;
            if (GUI.Button(new Rect(Screen.width / 2 - wide / 2, Screen.height / 2 - high / 2, wide, high), "End, Again?"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                _exit = false;
            }
        }

        if (!debug) return;
        //int[] money = new int[4];
        GUI.TextField(new Rect(5, 5, 150, Screen.height - 10), log);

        int money = 0;
        int all_money = 0;
        int safe_money = 0;
        string str = "Round: " + round + "\n\n";
        GameObject bd1 = GameObject.Find("bd1");
        all_money = bd1.GetComponent<StayBase>().GetAllMoney();
        money = bd1.GetComponent<StayBase>().GetMoney();
        safe_money = bd1.GetComponent<StayBase>().GetSafeMoney();
        str += "bot1:" + all_money + "(safe:" + safe_money + ",money:" + money + ")\n";

        GameObject bd2 = GameObject.Find("bd2");
        all_money = bd2.GetComponent<StayBase>().GetAllMoney();
        money = bd2.GetComponent<StayBase>().GetMoney();
        safe_money = bd2.GetComponent<StayBase>().GetSafeMoney();
        str += "bot2:" + all_money + "(safe:" + safe_money + ", money:" + money + ")\n";

        GameObject bd3 = GameObject.Find("bd3");
        all_money = bd3.GetComponent<StayBase>().GetAllMoney();
        money = bd3.GetComponent<StayBase>().GetMoney();
        safe_money = bd3.GetComponent<StayBase>().GetSafeMoney();
        str += "bot3:" + all_money + "(safe:" + safe_money + ",money:" + money + ")\n";

        GameObject bd4 = GameObject.Find("bd4");
        all_money = bd4.GetComponent<StayBase>().GetAllMoney();
        money = bd4.GetComponent<StayBase>().GetMoney();
        safe_money = bd4.GetComponent<StayBase>().GetSafeMoney();
        str += "user:" + all_money + "(safe:" + safe_money + ",money:" + money + ")\n";

        money = GameObject.Find("coin1").GetComponent<CoinBase>().GetMoney();
        str += "\ncoin1: " + money + "\n";

        money = GameObject.Find("coin2").GetComponent<CoinBase>().GetMoney();
        str += "coin2: " + money + "\n";

        money = GameObject.Find("coin3").GetComponent<CoinBase>().GetMoney();
        str += "coin3: " + money + "\n";

        money = GameObject.Find("coin4").GetComponent<CoinBase>().GetMoney();
        str += "coin4: " + money + "\n";

        GUI.TextField(new Rect(Screen.width - 200, 5, 200 - 5, Screen.height - 10), str);
    }
}
