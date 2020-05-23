using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonLightScript : MonoBehaviour
{
    private Light light = null;

    void Start()
    {
        light = this.GetComponent<Light>();
    }


    void Update()
    {
        Shader.SetGlobalVector("Ligh_tDir", -this.transform.forward);
        //Debug.Log(Shader.GetGlobalVector("_ToonLightDirection"));
    }
}
