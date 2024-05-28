using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class test2 : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ResultUI;
    public GameObject Result2UI;
    // Update is called once per frame
    void Update()
    {
        if (test.p)
        {
            ResultUI.SetActive(true);
        }
        else if (test.a)
        {
            Result2UI.SetActive(true);
        }

    }
}
