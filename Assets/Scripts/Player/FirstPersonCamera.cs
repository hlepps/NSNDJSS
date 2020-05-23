using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FirstPersonCamera : MonoBehaviour
{
    public GameObject weaponPlace;
    public bool activated;
    public float horizontalSpeed;
    public float verticalSpeed;
    public Cinemachine.CinemachineVirtualCamera vcam;


    float xRotation;
    float yRotation;

    public void Awake()
    {
        vcam = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    public void LookAt(Transform target)
    {
        transform.LookAt(target);
    }

    
    void Update()
    {
        if(Facade.instance.player != null)
        {
            activated = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            activated = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if(activated)
        {
            /*
            float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            Facade.instance.player.transform.Rotate(Vector3.up * mouseX);
            */
            
            Vector3 rot = transform.rotation.eulerAngles + new Vector3(Input.GetAxis("Mouse Y") * -verticalSpeed, Input.GetAxis("Mouse X") * horizontalSpeed, 0);
            if (rot.x < 270 && rot.x > 90)
            {
                if (rot.x < 180)
                    rot.x = 90;
                else
                    rot.x = 270;
            }
            transform.rotation = Quaternion.Euler(rot);
            
        }
    }
}
