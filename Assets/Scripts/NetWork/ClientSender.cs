using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public static class ClientSender
{
    static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Session.instance.tcp.SendData(_packet);
    }

    static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Session.instance.udp.SendData(_packet);
    }

    public static void NickNameSend(string _name)
    {
        using (Packet packet = new Packet((int)ClientPackets.NickNameCheck))
        {
            packet.Write(_name);

            SendTCPData(packet);
        }
    }

    public static void PlayersRequest()
    {
        using (Packet packet = new Packet((int)ClientPackets.PlayersRequest))
        {
            SendTCPData(packet);
        }
    }

    public static void Move(float _hori, float _vert, float _yaw)
    {
        using (Packet packet = new Packet((int)ClientPackets.Move))
        {
            packet.Write(_hori);
            packet.Write(_vert);
            packet.Write(_yaw);

            SendUDPData(packet);
        }
    }

    public static void Fire(Vector3 _muzzle)
    {
        using (Packet packet = new Packet((int)ClientPackets.Fire))
        {
            packet.Write(Session.instance.snapShot);
            packet.Write(_muzzle);

            SendUDPData(packet);
        }
    }

    public static void Respawn()
    {
        using (Packet packet = new Packet((int)ClientPackets.Resapwn))
        {


            SendTCPData(packet);
        }
    }

    public static void QuitGame()
    {
        using (Packet packet = new Packet((int)ClientPackets.QuitGame))
        {


            SendTCPData(packet);
        }
    }
}
