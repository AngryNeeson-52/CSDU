using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerInfo
{
    public int id;
    public Vector3 position;
    public float yaw;
}

public class SnapShot
{
    public List<PlayerInfo> enemys = new List<PlayerInfo>();
}
