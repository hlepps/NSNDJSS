using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsPool : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> weapons;
    void Awake()
    {
        foreach(GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
    }

    public GameObject GetWeapon(int id)
    {
        return weapons[id];
    }
}
