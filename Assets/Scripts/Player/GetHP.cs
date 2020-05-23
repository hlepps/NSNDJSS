using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetHP : MonoBehaviour
{
    public Player player;
    public TextMeshPro text;
    public void Update()
    {
        text.text = player.hp.ToString();
        transform.LookAt(Facade.instance.player.transform);
    }
}
