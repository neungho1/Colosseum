using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    
    public Transform target;
    public Transform Me;
    public float A = 0.5f;
    
    private void OnTriggerEnter(Collider other)
    {
        Vector3 knockBackPos = target.position - Me.position * A;
    	target.position = Vector3.Lerp(target.position, knockBackPos, 0.05f);
    }

   
}
