using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerNetworkSetup : NetworkBehaviour
{
    public Behaviour[] toDisable;
    void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour b in toDisable)
            {
                b.enabled = false;
            }
            
        }
        else
        {
            foreach (Behaviour b in toDisable)
            {
                b.enabled = true;
            }

            if (Facade.instance.playerCamera.vcam.Follow == null)
                Facade.instance.playerCamera.vcam.Follow = GetComponent<Player>().head;
            if (Facade.instance.player == null)
                Facade.instance.player = GetComponent<Player>();
            if (GetComponent<Player>().name == "")
            {
                GetComponent<Player>().name = Facade.instance.gameManager.localPlayerName;
                GetComponent<Player>().gameObject.name = Facade.instance.gameManager.localPlayerName;
            }
            if (Facade.instance.gameManager.localPlayerID == "")
                Facade.instance.gameManager.localPlayerID = GetComponent<NetworkIdentity>().netId.ToString();
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        GetComponent<Player>().id = GetComponent<NetworkIdentity>().netId.ToString();
        Debug.Log("Registering  id " + GetComponent<Player>().id);
        GameManager.RegisterPlayer(GetComponent<Player>().id, GetComponent<Player>());

    }

    void Update()
    {
        if (Facade.instance.playerCamera.vcam.Follow == null)
            Facade.instance.playerCamera.vcam.Follow = GetComponent<Player>().head;
        if (Facade.instance.player == null)
            Facade.instance.player = GetComponent<Player>();
        if (GetComponent<Player>().name == "")
        {
            GetComponent<Player>().name = Facade.instance.gameManager.localPlayerName;
            GetComponent<Player>().gameObject.name = Facade.instance.gameManager.localPlayerName;
        }
        if (Facade.instance.gameManager.localPlayerID == "")
            Facade.instance.gameManager.localPlayerID = GetComponent<NetworkIdentity>().netId.ToString();
    }


    public void OnDisable()
    {
        GameManager.UnRegisterPlayer(GetComponent<NetworkIdentity>().netId.ToString());
        GetComponent<Player>().DestroyWeapon();
    }
}
