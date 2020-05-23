using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAnimationController : NetworkBehaviour
{
    [SyncVar] public bool walking = false;
    [SyncVar] public bool running = false;

    public Animator animator;
    
    public void Set(bool _walk, bool _run)
    {
        walking = _walk;
        running = _run;
    }

    void Update()
    {
        animator.SetBool("walk", walking);
        animator.SetBool("run", running);
        //
    }
}
