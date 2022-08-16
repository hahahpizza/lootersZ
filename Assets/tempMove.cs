using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempMove : MonoBehaviour
{
    float speed = 5.0f;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w"))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
         if(Input.GetKey("s"))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
         if(Input.GetKey("d"))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
         if(Input.GetKey("a"))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}
