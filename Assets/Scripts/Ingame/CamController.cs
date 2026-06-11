using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private Camera weaponCam;
    [SerializeField]
    private Transform mainTr;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    private float aimFOV = 20f;
    [SerializeField]
    private float normFov = 70f;


    private void Awake()
    {
        if (mainCam == null)
            mainCam = Camera.main;

        if (mainTr == null)
            mainTr = Camera.main.transform;

        if (mainCam == null)
            Debug.Log("camera 참조 null");

        if (mainTr == null)
            Debug.Log("camera 참조 null");

        if (player == null)
            Debug.Log("camera - player 참조 null");
    }

    public void CameraUpdate()
    {
        mainTr.transform.position = 
            player.transform.position 
            + player.transform.right * offset.x 
            + player.transform.forward * offset.z 
            + player.transform.up * offset.y;


        mainTr.transform.rotation = player.transform.rotation;
    }

    public void Aiming(bool _YN)
    {
        if (_YN)
        {
            Camera.main.fieldOfView = aimFOV;
            weaponCam.enabled = false;
        }
        else
        {
            Camera.main.fieldOfView = normFov;
            weaponCam.enabled = true;
        }
    }
}
