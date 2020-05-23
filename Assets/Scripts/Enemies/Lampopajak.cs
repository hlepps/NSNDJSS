using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.AI;

public class Lampopajak : MonoBehaviour
{
    GameObject player;
    NavMeshAgent nav;
    Animator animator;

    public AudioSource audioPlayer;
    public AudioSource bonesPlayer;
    public AudioSource footsteps;
    public AudioClip clip;
    public AudioClip bones;

    public GameObject whereToLook;

    public float range = 20;
    public bool startFollowing;
    public float speed = 10;
    Vignette vignette = null;
    ChromaticAberration ca = null;
    Grain grain = null;

    void Start()
    {
        player = GameObject.Find("Player");
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Facade.instance.postProcessVolume.profile.TryGetSettings(out vignette);
        Facade.instance.postProcessVolume.profile.TryGetSettings(out ca);
        Facade.instance.postProcessVolume.profile.TryGetSettings(out grain);
    }

    bool once;
    float timer;

    public bool anotherTrigger;
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        if (distance < range || anotherTrigger)
        {
            if (!once)
            {
                bonesPlayer.PlayOneShot(bones);
                once = true;
            }


            GetComponent<Animator>().SetTrigger("trigger");
            transform.LookAt(player.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            startFollowing = true;

            
            
            if (distance < 1.5f)
                SceneManager.LoadScene(1);
        }

        if (startFollowing)
        {
            grain.intensity.value += Time.deltaTime / 4;
            ca.intensity.value += Time.deltaTime / 4;
            vignette.intensity.value = (1 / (distance - 1)) * 2;

            GetComponent<Animator>().SetTrigger("trigger");
            if (!audioPlayer.isPlaying)
                audioPlayer.PlayOneShot(clip);
            
            timer += Time.deltaTime;
            transform.LookAt(player.transform);

            if (timer > 3.525f)
            {
                nav.destination = player.transform.position;
                footsteps.mute = false;
            }
            else if (timer < 2f)
            {
                Facade.instance.playerCamera.LookAt(whereToLook.transform);
                Facade.instance.player.Stun(0.05f);
            }
        }
    }
}
