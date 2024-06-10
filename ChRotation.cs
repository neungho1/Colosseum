using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChRotation : MonoBehaviour
{
    public Transform GetTransform;

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Invoke("Shield_up", 0.1f);
            
        }
        if(Input.GetMouseButtonUp(1))
        {
            Invoke("Shield_Down", 0.05f);
        }
    }

    void Shield_up()
    {
        GetTransform.localRotation = Quaternion.Euler(0f, 50, 0f);
    }

    void Shield_Down()
    {
        GetTransform.localRotation = Quaternion.Euler(0f, -50, 0f);
    }


    // Update is called once per frame
}
