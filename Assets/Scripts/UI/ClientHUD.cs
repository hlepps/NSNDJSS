using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ClientHUD : MonoBehaviour
{
    public NetworkManager networkManager;

    public Text name;
    public Text hostname;

    public GameObject HUD;

    public void Connect()
    {
        networkManager.networkAddress = hostname.text;
        Facade.instance.gameManager.localPlayerName = name.text;
        networkManager.networkPort = 2137;
        networkManager.StartClient();
        DisableHUD();
    }

    public void Host()
    {
        Facade.instance.gameManager.localPlayerName = name.text;
        networkManager.networkPort = 2137;
        networkManager.StartHost();
        DisableHUD();
    }

    public void DisableHUD()
    {
        HUD.SetActive(false);
    }
}
