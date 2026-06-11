using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField]
    private Camera minimapCam;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Vector3 mapPos = new Vector3(-200f, 0, 0);

    public void SetMiniMap()
    {
        minimapCam.gameObject.transform.position = player.position + mapPos;
        minimapCam.transform.rotation =
            Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
