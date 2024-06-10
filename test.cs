using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public static bool p;
    public static bool a;
    void Start()
    {
        p = false;
        a = false;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            p = true;
        }
        else if (col.gameObject.tag == "Pc")
        {
            a = true;
        }
        else if (col.gameObject.tag == "Agent")
        {
            a = true;
        }
            
    }
    
}
