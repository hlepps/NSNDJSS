using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InwokacjaTrigger : MonoBehaviour
{
    public Lampopajak lampopajak;
    
    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col);
        if(col.gameObject.GetComponent<Player>())
        {
            lampopajak.anotherTrigger = true;
        }
    }
}
