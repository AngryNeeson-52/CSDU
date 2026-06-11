using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PacketDispatcher
{
    public delegate void PacketHandler(Packet _packet);

    public static Dictionary<int, PacketHandler> packetHandlers = new Dictionary<int, PacketHandler>()
    {
        { (int)ServerPackets.Connected, Connected },
        { (int)ServerPackets.NickNameResult, NickNameResult },
        { (int)ServerPackets.PlayerUpdate, PlayerUpdate },
        { (int)ServerPackets.SnapShot, SnapShot },
        { (int)ServerPackets.GetHit, GetHit },
        { (int)ServerPackets.Dead, Dead },
    };

    public static void Connected(Packet _packet)
    {
        int id = _packet.ReadInt();

        Session.instance.id = id;

        Session.instance.udp.Connect(((IPEndPoint)Session.instance.tcp.socket.Client.LocalEndPoint).Port);

        LoadManager.instance.ConnectedServer(id);

        Debug.Log("통신완료");
    }

    public static void NickNameResult(Packet _packet)
    {
        bool accept = _packet.ReadBool();

        NetworkEvents.NickNamePass(accept);
    }

    public static void PlayerUpdate(Packet _packet)
    {
        SnapShot snapshot = new SnapShot();
        PlayerInfo p = new PlayerInfo();

        string name = _packet.ReadString();

        int count = _packet.ReadInt();

        for(int i = 0; i < count; i++)
        {
            p.id = _packet.ReadInt();
            p.position = _packet.ReadVector3();
            p.yaw = _packet.ReadFloat();

            snapshot.enemys.Add(p);
        }


        NetworkEvents.Welcome(name);
        NetworkEvents.Spawn(snapshot);
    }

    public static void SnapShot(Packet _packet)
    {
        SnapShot snapshot = new SnapShot();
        PlayerInfo p = new PlayerInfo();

        uint ver = _packet.ReadUInt();
        Session.instance.snapShot = ver;


        int count = _packet.ReadInt();

        for(int i = 0; i < count; i++)
        {
            p.id = _packet.ReadInt();
            p.position = _packet.ReadVector3();
            p.yaw = _packet.ReadFloat();

            snapshot.enemys.Add(p);
        }


        NetworkEvents.SnapShot(snapshot);
    }

    public static void GetHit(Packet _packet)
    {
        int hp = _packet.ReadInt();

        NetworkEvents.GetHit(hp);
    }

    public static void Dead(Packet _packet)
    {
        string killer = _packet.ReadString();
        string deadPool = _packet.ReadString();
        int id = _packet.ReadInt();

        NetworkEvents.DeadPool(killer, deadPool, id);
    }
}
