using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cinemachine;

[RequireComponent(typeof(PlayerAnimationController))]
public class Player : NetworkBehaviour
{
    public enum STATE
    {
        MOVING_ON_GROUND = 0,
        CLIMBING = 1,
        TALKING = 2
    };

    [Header("Moving")]
    public bool enableMovement = true;
    public float moveSpeed;
    public float runSpeed;
    public float jumpForce;
    public float gravityScale;
    public bool bobbing;
    private Vector3 slidingDirection;
    private bool sliding;
    private bool running;
    private bool walking;
    private CharacterController controller;
    private Vector3 moveDirection;
    private float newSpeed;
    private float speedLerp;

    [Header("Weapons")]
    public int weaponID;
    private Weapon weapon;
    private GameObject weaponGameObject;

    [Header("Stats")]
    [SyncVar] public float hp = 100;
    [SyncVar] public int kills;
    [SyncVar] public string name;
    [SyncVar] public string id;
    public float maxHP = 100;
    public float additionalSpeed;


    [Header("Effects")]
    private bool stun;
    private float stunTime;

    private float groundTime;
    private float timePassed;


    [Header("Objects")]
    public Transform head;
    public AudioSource footsteps;
    public PlayerAnimationController pac;
    public GameObject playerModel;
    public List<Renderer> toHideRenderer;
    GameObject cam;
    GameObject cinemachineCamera;

    void OnEnable()
    {
        SetDefaults();
        playerModel.SetActive(true);
    }

    public void SetDefaults()
    {
        hp = maxHP;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        timePassed = groundTime;
        this.transform.SetParent(null);
        cam = GameObject.Find("MainCamera");
        cinemachineCamera = Facade.instance.playerCamera.gameObject;
    }

    /*
    //sliding
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float angle = Vector3.Angle(Vector3.up, hit.normal);
        if (angle > 40f)
        {
            sliding = true;

            Vector3 normal = hit.normal;
            Vector3 c = Vector3.Cross(Vector3.up, normal);
            Vector3 u = Vector3.Cross(c, normal);
            slidingDirection = u * 4f;

        }
        else
        {
            sliding = false;
            slidingDirection = Vector3.zero;
        }
    }

    */
    
    //stun maker
    public void Stun(float time)
    {
        stun = true;
        stunTime = time;


    }

    public void Damage(float dmg)
    {
        hp -= dmg;
    }

    public void DestroyWeapon()
    {
        Destroy(weapon);
    }

    [Command]
    public void CmdShoot(string name, float damage, string sender)
    {
        Debug.Log(string.Format("{0} has been shoot", name), this.gameObject);
        Player p = GameManager.GetPlayer(name);
        p.Damage(damage);
        if(p.hp <= 0)
        {
            
            GameManager.GetPlayer(sender).kills++;
        }
    }
    
    [Command]
    public void CmdAnim(string name, bool _walk, bool _run, Quaternion rotation)
    {
        Player p = GameManager.GetPlayer(name);
        p.GetComponent<PlayerAnimationController>().Set(_walk, _run);
        p.playerModel.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }

    [Command]
    public void CmdJump(string name)
    {
        Player p = GameManager.GetPlayer(name);
        p.playerModel.GetComponent<Animator>().SetTrigger("jump");
    }

    

    private void Update()
    {
        if(hp <= 0)
        {
            var spawn = NetworkManager.singleton.GetStartPosition();
            GetComponent<CharacterController>().enabled = false;
            cam.transform.position = spawn.position;
            cinemachineCamera.transform.position = spawn.position;
            transform.position = spawn.position;
            GetComponent<CharacterController>().enabled = true;
            hp = maxHP;
        }
        if (GetComponent<PlayerNetworkSetup>().isClient)
        {
            foreach(Renderer r in toHideRenderer)
            {
                r.enabled = false;
            }
            //shoting
            if (weapon == null)
            {
                Destroy(weaponGameObject);
                weaponGameObject = Instantiate(Facade.instance.weaponsPool.GetWeapon(weaponID));
                weapon = weaponGameObject.GetComponent<Weapon>();
                weaponGameObject.SetActive(true);
                weaponGameObject.transform.SetParent(Facade.instance.playerCamera.weaponPlace.transform);
                weaponGameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
            else
                weapon.HandleWeapon();
        }
        else
        {
            foreach(Renderer r in toHideRenderer)
            {
                r.enabled = true;
            }
        }


        //stun checker
        if (stun)
        {
            stunTime -= Time.deltaTime;
            if (stunTime <= 0)
            {
                stun = false;
            }
        }
        if (enableMovement)
        {
            //not overwrite y
            float yStore = moveDirection.y;

            float frontBackMovement = Input.GetAxis("Vertical");
            if (frontBackMovement < 0)
                frontBackMovement /= 2;

            moveDirection = (cam.transform.forward * frontBackMovement) + (cam.transform.right * Input.GetAxis("Horizontal"));

            //sprinting
            if (Input.GetButton("Sprint") && !stun)
            {
                moveDirection = moveDirection.normalized * (runSpeed + additionalSpeed + newSpeed);
                running = true;
                cinemachineCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 55;
                cinemachineCamera.GetComponent<CamBobbing>().power = 6;
                footsteps.mute = false;
                footsteps.pitch = 2;
            }
            else
            {
                moveDirection = moveDirection.normalized * (moveSpeed + additionalSpeed + newSpeed);
                running = false;
                cinemachineCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 50;
                cinemachineCamera.GetComponent<CamBobbing>().power = 4;
                footsteps.mute = false;
                footsteps.pitch = 1;
            }

            //get back y without speed
            moveDirection.y = yStore;


            //when is grounded
            if (controller.isGrounded)
            {
                //moveDirection.y = 0;
                //gravityScale = 15;

                //any movement
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    walking = true;
                    //if during this frame player has jumped
                    if (!controller.isGrounded)
                    {
                        timePassed = groundTime;
                    }
                    
                        speedLerp = 1;

                }
                else
                {
                    walking = false;
                    running = false;
                    footsteps.mute = true;
                }


            }
            else
            {
                footsteps.mute = true;
            }



            //stun 
            if (stun)
                moveDirection = new Vector3(0, yStore, 0);
            

            //final
            moveDirection += slidingDirection;
            //controller.Move(new Vector3(moveDirection.x, 0, moveDirection.z) * Time.deltaTime);
            

            //jump
            if (Input.GetButtonDown("Jump") && !stun && controller.isGrounded)
            {
                CmdJump(id);
                //gravityScale = 3;
                moveDirection.y = Mathf.Sqrt(jumpForce * -2f * (gravityScale * Physics.gravity.y));
            }
            else
            {
                if (moveDirection.y < -20f)
                    moveDirection.y = -20f;
            }

            //gravity
            moveDirection += Physics.gravity * gravityScale * Time.deltaTime;

            
                

            //apply jump
            controller.Move(moveDirection * Time.deltaTime);

            //Debug.Log(moveDirection.y);

        }
        else
        {
            
        }
        if(Facade.instance.player != null)
            Facade.instance.player.CmdAnim(id, walking, running, Facade.instance.playerCamera.transform.rotation);
    }



}
