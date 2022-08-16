  
using UnityEngine;
public class SafeBase : MonoBehaviour
{ 
    GameManager _gameManager;
    string to;

    void Start()
    {
        to = gameObject.name;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
   
    void OnMouseDown()
    {         
        _gameManager.OnMouseEventByClickObjs(to);
    }
}
