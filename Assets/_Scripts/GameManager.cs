
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string log = "-------- for debug ---------\r\n";
    public GameObject[] players;
    GameObject[] coins;
    public GameObject[] fall;
    public GameObject[] boards;
    public GameObject[] safezones;
    public int[] moneyArr;
    List<Vector3> abs_board_positions;
    List<Vector3> abs_coin_positions;
    List<Vector3> abs_sz_positions;
    bool _exit = false;
    int signals = 0;
    public int level = 1;
    int round = 0;
    int round_limit = 5;
    

    bool _isStart1;
    float _time1;

    string[] froms;
    string[] tos;
    string[] whos;     
    

    List<int[]> lootersArr ;
    bool[] isCollids;
    string[] _animations;
    int bestIndex;
     

    private void Start()
    {
        abs_board_positions = new List<Vector3>();
        abs_coin_positions = new List<Vector3>();
        abs_sz_positions = new List<Vector3>();
        whos = new string[level + 1];
        switch(level)
        {
            case 1: whos[0] = "bot3"; whos[1] = "user"; break;
            case 2: whos[0] = "bot2"; whos[1] = "bot3"; whos[2] = "user"; break;
            default: whos[0] = "bot1"; whos[1] = "bot2"; whos[2] = "bot3"; whos[3] = "user"; break; 
        }
         
        froms = new string[level + 1];
        tos = new string[level + 1];          
        _animations = new string[level + 1] ;

        isCollids = new bool[level + 1];
        for (int i = 0; i < level + 1; i++)
            isCollids[i] = false;
        lootersArr = new List<int[]>();
        signals = 0;
        bestIndex = -1;
        _time1 = 0;
        _isStart1 = false;
        coins = new GameObject[4];
        for(int i = 0; i < 4; i++)
            coins[i] = GameObject.Find("coin" + (i+1));
        foreach (GameObject player in players)
        {
            Transform trans = player.transform;
            abs_board_positions.Add(trans.position);
        }
        for (int i = 0; i < coins.Length; i++)
        {
            GameObject coin = coins[i];
            Transform trans = coin.transform;
            Vector3 pos = trans.position;
            pos.y = abs_board_positions[0].y; 
            abs_coin_positions.Add(pos);
        }

        for (int i = 0; i < level + 1; i++)
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

    int[] GetRandomFourInts(int min, int max)
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

        int[] rs = GetRandomFourInts(1, 5);
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
        for (int i = 0; i < level + 1; i++)
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
              
                Vector3 v3 = child.transform.position;
                v3.x = child.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f);
                v3.z = child.transform.position.z + UnityEngine.Random.Range(-0.5f, 0.5f);
                v3.y = -1.2f;
                child.transform.position = v3;
                child.transform.SetParent(to_.transform, true); 
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

     

    public string Safebase2Player(string safezone)
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
        if (anim.name != type) 
            anim.Play("Armature|" + type); 
        else
        {
            if (!anim.isPlaying)
                anim.Play("Armature|" + type);
        }
    }
    

    int Looters2Money(int[] looters)
    {
        int res = 0;
        for (int i = 0; i < 4; i++)
        {
            int cnt = looters[i];
            res += cnt * moneyArr[i];
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

    string Safebase2Staybase(string safebase)
    {
        switch (safebase)
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

    protected bool IsSelfStaybase(string who, string staybase)
    {
        return staybase == GetBaseNameByPlayer(who);
    }

    
    protected void Run2Base()
    {
        if (_isStart1)
        {
            _time1 += Time.deltaTime;
            for (int i = 0; i < level + 1; i++)
            {
                string who_ = whos[i];
                string animation = _animations[i];
                if (i == bestIndex) animation = "Win4";
                AnimationPlay(who_, animation);
            }
            if (_time1 > 3.0f)
            {
                for (int i = 0; i < level + 1; i++)
                {
                    string from_ = froms[i];
                    string to_ = tos[i];
                    string who_ = whos[i];
                    bool isColl_ = isCollids[i];
                    int[] looters_ = lootersArr[i];
                    AnimationPlay(who_, "JumpFalling");
                    if (isColl_ || !to_.Contains("coin")) looters_ = new int[4] { 0, 0, 0, 0 };
                    switch (level)
                    {
                        case 3:
                            switch (who_)
                            {
                                case "bot1":
                                    players[0].transform.localEulerAngles = new Vector3(0, 180, 0);
                                    break;
                                case "bot2":
                                    players[1].transform.localEulerAngles = new Vector3(0, 90, 0);
                                    break;
                                case "bot3":
                                    players[2].transform.localEulerAngles = new Vector3(0, 0, 0);
                                    break;
                                case "user":
                                    players[3].transform.localEulerAngles = new Vector3(0, -90, 0);
                                    break;
                            }
                            break;

                        case 2:
                            switch (who_)
                            {
                                case "bot2":
                                    players[0].transform.localEulerAngles = new Vector3(0, 180, 0);
                                    break;
                                case "bot3":
                                    players[1].transform.localEulerAngles = new Vector3(0, 90, 0);
                                    break;
                                case "user":
                                    players[2].transform.localEulerAngles = new Vector3(0, 0, 0);
                                    break; 
                            }
                            break;

                        case 1:
                            switch (who_)
                            {
                                case "bot3":
                                    players[0].transform.localEulerAngles = new Vector3(0, 180, 0);
                                    break;
                                case "user":
                                    players[1].transform.localEulerAngles = new Vector3(0, 90, 0);
                                    break;
                             
                            }
                            break;
                    }
                    if(!isColl_)
                        GameObject.Find(who_).GetComponent<Action>().RunBack(to_, from_, looters_);
                    else
                        GameObject.Find(who_).GetComponent<Action>().RunBack(to_, from_);
                }
                SetCoinLooters();

                isCollids = new bool[level + 1];
                for (int i = 0; i < level + 1; i++)
                    isCollids[i] = false;
                lootersArr = new List<int[]>();
                signals = 0;
                bestIndex = -1;
                _time1 = 0;
                _isStart1 = false;

            }
        }
    }
    private void FixedUpdate()
    {
         
        Run2Base();
    }  


  public void OnArrivedTargetPlatform(string who, string from, string to, bool isCollid)
    {  
        AnimationPlay(who, "JumpLanding");
        log += "\n" + who + ">" + to + "(" + isCollid + ")";
        if(to.Contains("safezone"))
        {
            string staybase = Safebase2Staybase(to);
            GameObject.Find(staybase).GetComponent<StayBase>().SetSafeLooters();
        } 
        signals++;
        int index = GetIndexPlayer(who);
        froms[index] = from;
        tos[index] = to;        
        isCollids[index] = isCollid;
        if (signals != level + 1) return;
        int max = -1;
        lootersArr = new List<int[]>();
        for (int i = 0; i < level + 1; i++)
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
                    if (!IsSelfStaybase(who_, to_))
                    {
                        looters = to_obj.GetComponent<StayBase>().GetLooters();
                        to_obj.GetComponent<StayBase>().StealedBy(who_);
                    }
                }
                else if (to_.Contains("coin"))
                    looters = to_obj.GetComponent<CoinBase>().GetLooters();
                switch (level)
                {
                    case 1:
                        GameObject.Find("bd" + (i + 3)).GetComponent<StayBase>().AddLooters(looters);
                        break;
                    case 2:
                        GameObject.Find("bd" + (i + 2)).GetComponent<StayBase>().AddLooters(looters);
                        break;
                    default: 
                        GameObject.Find("bd" + (i + 1)).GetComponent<StayBase>().AddLooters(looters);
                        break;
                }
            }
            lootersArr.Add(looters);
            int money = Looters2Money(looters);
            if (money > max)
                max = money;
            string animation = string.Empty;
            if (is_Coll) animation = "Fight" + (i + 1);
            else
            {
                if (money == 0)
                {
                    if (IsSelfStaybase(who_, to_) || to_.Contains("safezone")) animation = "Idle5";
                    else animation = "Lose3";
                }
                else if (money > 0 && money < 500) animation = "HappyIdle";
                else if (money >= 500 && money < 1000)
                {
                    animation = "HappyIdle" + (i + 1);
                    if (animation == "HappyIdle1") animation = "HappyIdle2";
                }
                else if (money >= 1000) animation = "Win";
            }
            _animations[i] = animation;
        }         
        for (int i = 0; i < level + 1; i++)
        { 
            int[] looters = lootersArr[i];
            int money = Looters2Money(looters);
            if (money == max && money > 0)
            {
                bestIndex = i;
                break;
            }
        }
        _time1 = 0;
        _isStart1 = true;
    }

    public Vector3 GetFallPos(string from)
    {
        Vector3 pos;
        switch(from)
        {
            case "coin1":                 
                pos = fall[0].transform.position;
                break;
            case "coin2":
                pos = fall[1].transform.position;
                break;
            case "coin3":
                pos = fall[2].transform.position;
                break;
            case "coin4":
                pos = fall[3].transform.position;
                break;
            case "bd1":
                pos =  fall[4].transform.position;
                break;
            case "bd2":
                pos = fall[5].transform.position;
                break;
            case "bd3":
                pos = fall[6].transform.position;
                break;
            case "bd4":
                pos = fall[7].transform.position;
                break;
            default:
                pos = Vector3.zero;
                break;
        }
        float range = 5.0f; 
        float x = Random.Range(-range, range); 
        float z = Random.Range(-range, range);
        pos.x = pos.x + x;
        pos.z = pos.z + z;
        return pos;

    }

    public Vector3 GetPos(string target)
    { 
        int index = 0;
        Vector3 pos;
        if (target.Contains("coin"))
        {
            pos = GameObject.Find(target).transform.position;
            pos.y = abs_coin_positions[0].y;
            pos.z = pos.z + 1.0f; 
            return pos;
        }
        else if (target.Contains("bd"))
        {
            switch (level)
            {
                case 1:
                    if(target == "bd3")
                        return abs_board_positions[0];
                    else return abs_board_positions[1];
                case 2:
                    index = System.Convert.ToInt32(target.Remove(0, 2));
                    return abs_board_positions[index - 2];
                default:
                    index = System.Convert.ToInt32(target.Remove(0, 2));
                    return abs_board_positions[index - 1];
            }
        }
        else if (target.Contains("safezone"))
        {
            switch (level)
            {
                case 1:
                    if(target=="safezone3")
                    return abs_sz_positions[0];
                    else return abs_sz_positions[1];
                case 2:
                    index = System.Convert.ToInt32(target.Remove(0, 8));
                    return abs_sz_positions[index - 2];
                default:
                    index = System.Convert.ToInt32(target.Remove(0, 8));
                    return abs_sz_positions[index - 1];
            }
        }
        return Vector3.zero; 
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
 
    protected string GetBestTarget(int index)
    {
        string from = "bd" + index;
        while (true)
        {
            string to = this.GetEnableTarget(index);
            if (to.Contains("coin")) 
                return to; 
            else if (to.Contains("safezone"))
            {
                if (GameObject.Find(from).GetComponent<StayBase>().GetMoney() > 0) 
                    return to; 
            }
            else if (to.Contains("bd"))
            {
                if (GameObject.Find(to).GetComponent<StayBase>().GetMoney() > 0) 
                    return to; 
            }
        }
    }
    public void OnMouseEventByClickObjs(string to)
    { 
        this.Count();
        if (_exit) return;
        switch (level)
        {
            case 1:                 
                froms[0] = "bd3"; tos[0] = this.GetBestTarget(3);                 
                froms[1] = "bd4"; tos[1] = to;
                break;
            case 2: 
                froms[0] = "bd2";
                froms[1] = "bd3";
                froms[2] = "bd4";                
                tos[0] = this.GetBestTarget(2);
                tos[1] = this.GetBestTarget(3);
                tos[2] = to;
                break;
            default:
                froms[0] = "bd1";
                froms[1] = "bd2";
                froms[2] = "bd3";
                froms[3] = "bd4";                
                tos[0] = this.GetBestTarget(1);
                tos[1] = this.GetBestTarget(2);
                 
                tos[2] = this.GetBestTarget(3);
                 
                tos[3] = to;
                break;
        }       

        bool[] isColls = new bool[level + 1];
        for (int i = 0; i < level + 1; i++)
            isColls[i] = false; 
        
        string[] colliders = new string[level + 1];
        for (int i = 0; i < level + 1; i++)
        {
            string to1 = tos[i];             
            for (int j = 0; j < level + 1; j++)
            {                 
                string to2 = tos[j];
                if (i != j && to1 == to2)
                {            
                    isColls[i] = true;
                    colliders[i] = whos[j];
                    isColls[j] = true; 
                } 
            }
        }
        for(int i = 0; i < players.Length; i++)
            players[i].GetComponent<Action>().RunToTarget(froms[i], tos[i], isColls[i], colliders[i]);
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
    protected string GetEnableTarget(int botNum)
    {
        int n = GetRandomInt(1, coins.Length + (level + 1) + 1 + 1);     
        if (level == 1)
        {
            switch (n)
            {
                case 1: return "coin1";
                case 2: return "coin2";
                case 3: return "coin3";
                case 4: return "coin4";
                case 5: return "bd3";
                case 6: return "bd4";
                case 7: return "safezone" + botNum;
                default: return string.Empty;
            }
        }
        else if (level == 2)
        {  
            switch (n)
            {
                case 1: return "coin1";
                case 2: return "coin2";
                case 3: return "coin3";
                case 4: return "coin4";
                case 5: return "bd2";
                case 6: return "bd3";
                case 7: return "bd4";
                case 8: return "safezone" + botNum;
                default: return string.Empty;
            }
        }
        else 
        { 
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
    }

    bool debug = false;
    void OnGUI()
    { 
        GUI.color = Color.white;
        int wide = Screen.width - 200, high = Screen.height - 100; 

        if (_exit)
        {
            GUI.Box(new Rect(Screen.width / 2 - wide / 2, Screen.height / 2 - high / 2, wide, high), "");
            wide = 100; high = 50;
            if (GUI.Button(new Rect(Screen.width / 2 - wide / 2, Screen.height / 2 - high / 2, wide, high), "End, Next Level?"))
            {
                int lv = level + 1;
                if (lv > 3) lv = 3;
                UnityEngine.SceneManagement.SceneManager.LoadScene(lv);
                _exit = false;
            }
        }

        //hSliderValue = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 50, 50, 100, 30), hSliderValue, 1.0F, 10.0F);
        if (GUI.Button(new Rect(35, 1, 60, 20), "AGAIN"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        if (GUI.Button(new Rect(100, 1, 60, 20), "LOG"))
        {
            debug ^= true;
        }

        if (!debug) return; 

        int money = 0;
        int all_money = 0;
        int safe_money = 0;
        string str = "Round: " + round + "\n\n";
        GameObject bd1 = GameObject.Find("bd1");
        if (bd1)
        {
            all_money = bd1.GetComponent<StayBase>().GetAllMoney();
            money = bd1.GetComponent<StayBase>().GetMoney();
            safe_money = bd1.GetComponent<StayBase>().GetSafeMoney();
            str += "bot1:" + all_money + "(safe:" + safe_money + ",money:" + money + ")\n";
        } 
         GameObject bd2 = GameObject.Find("bd2");
        if (bd2)
        {
            all_money = bd2.GetComponent<StayBase>().GetAllMoney();
            money = bd2.GetComponent<StayBase>().GetMoney();
            safe_money = bd2.GetComponent<StayBase>().GetSafeMoney();
            str += "bot2:" + all_money + "(safe:" + safe_money + ", money:" + money + ")\n";
        }
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

        GUI.TextField(new Rect(1, 22, 200, Screen.height - 23), str + "" + log);
    }
}
