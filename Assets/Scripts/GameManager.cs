using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string localPlayerName;
    public string localPlayerID;
    public Text text;
    public static Dictionary<string, Player> playerDictionary = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _name, Player _player)
    {
        if (!playerDictionary.ContainsKey(_name))
        {
            playerDictionary.Add(_name, _player);
            _player.transform.name = _name;
        }
    }

    public static void UnRegisterPlayer(string _name)
    {
        playerDictionary.Remove(_name);
    }

    public static Player GetPlayer(string _name)
    {
        return playerDictionary[_name];
    }

    public void Update()
    {
        text.text = "";
        foreach (string s in playerDictionary.Keys)
        {
            text.text += playerDictionary[s].gameObject.name +  ":" + playerDictionary[s].kills + "\n";
        }
    }
}
