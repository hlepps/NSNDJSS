using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public List<Skin> skins = new List<Skin>();


    public Skin GetRandomSkin()
    {
        return skins[Random.Range(0, skins.Count)];
    }

    public Skin GetSkin(int n)
    {
        return skins[n];
    }
}
