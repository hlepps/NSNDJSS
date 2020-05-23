using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetSkin : NetworkBehaviour
{

    [SyncVar]
    public int skinNumber = -1;
    [SyncVar]
    public int textureNumber = -1;

    Texture txt;

    public SkinnedMeshRenderer head, body;

    void Update()
    {
        if (skinNumber == -1)
        {
            skinNumber = Random.Range(0, Facade.instance.skinManager.skins.Count);
            textureNumber = Random.Range(0, Facade.instance.skinManager.skins[skinNumber].textures.Count);
            txt = Facade.instance.skinManager.skins[skinNumber].textures[textureNumber];
            head.sharedMesh = Facade.instance.skinManager.skins[skinNumber].head;
            body.sharedMesh = Facade.instance.skinManager.skins[skinNumber].mesh;
            head.material.SetTexture("_MainTex", txt);
            body.material.SetTexture("_MainTex", txt);
        }


        if (!isLocalPlayer)
        {
            
        }
        else
        {
            
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        
    }
}
