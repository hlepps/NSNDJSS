using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBobbing : MonoBehaviour
{

    public float maxY, minY, power;
    public bool bobbing;

    private float lerp;
    private bool dir;

    private void Update() {
        if(bobbing)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Lerp(minY, maxY, lerp), transform.position.z);
            if(!dir)
                lerp += Time.deltaTime * power;
            if(dir)
                lerp -= Time.deltaTime * power;

            if(lerp >= 1)
                dir = true;
            if(lerp <= 0)
                dir = false;

        }
    }
}
