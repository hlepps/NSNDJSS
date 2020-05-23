using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukglock : Weapon
{
    public AudioClip clip;
    public GameObject explosionPrefab;
    public Transform explosionPlace;
    public GameObject gunhole;

    public override void HandleWeapon()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject x = GameObject.Instantiate(explosionPrefab);
            x.transform.position = explosionPlace.position;
            x.GetComponent<Autodestroy>().follow = explosionPlace;
            GetComponent<AudioSource>().PlayOneShot(clip);

            
            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.Raycast(Facade.instance.playerCamera.transform.position, Facade.instance.playerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(Facade.instance.playerCamera.transform.position, Facade.instance.playerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                Player p = hit.transform.gameObject.GetComponent<Player>();
                if(p != null && p.id != Facade.instance.gameManager.localPlayerID)
                {
                    Facade.instance.player.CmdShoot(p.id, damage, Facade.instance.gameManager.localPlayerID);
                }
                else
                {
                    GameObject.Instantiate(gunhole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                }
            }
            
        }
    }
}
