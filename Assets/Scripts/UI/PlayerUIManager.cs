using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Slider hpSlider;
    
    void Update()
    {
        if (Facade.instance.player != null)
        {
            hpSlider.value = Facade.instance.player.hp;
            hpSlider.maxValue = Facade.instance.player.maxHP;
        }
    }
}
