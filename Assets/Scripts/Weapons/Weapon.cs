using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Weapon : MonoBehaviour
{
    public float damage;
    public abstract void HandleWeapon();

    
}
