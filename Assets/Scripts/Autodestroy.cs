using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autodestroy : MonoBehaviour
{
    public float time = 1;
    public Transform follow;
    void Update()
    {
        if(follow != null)
        {
            transform.position = follow.transform.position;
        }
        time -= Time.deltaTime;
        if (time <= 0)
            Destroy(this.gameObject);
    }
}
