using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;

public class Facade : MonoBehaviour
{
    public static Facade instance;
    
    public GameManager gameManager;
    public PostProcessVolume postProcessVolume;
    public Player player;
    public WeaponsPool weaponsPool;
    public FirstPersonCamera playerCamera;
    public NetworkManager networkManager;
    public SkinManager skinManager;

    void Start()
    {
        instance = this;
    }
    
}
