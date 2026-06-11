using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public static class NetworkEvents
{
    public static event Action<bool> nickname;

    public static event Action<string> welcome;

    public static event Action<SnapShot> spawn;

    public static event Action<SnapShot> snapshot;

    public static event Action<string, string, int> dead;

    public static event Action<int> gethit;

    public static event Action respawn;



    public static void NickNamePass(bool _TF)
    {
        nickname?.Invoke(_TF);
    }

    public static void Welcome(string _name)
    {
        welcome?.Invoke(_name);
    }

    public static void Spawn(SnapShot _snapShot)
    {
        spawn?.Invoke(_snapShot);
    }

    public static void SnapShot(SnapShot _snapShot)
    { 
        snapshot?.Invoke(_snapShot);
    }

    public static void DeadPool(string _killer, string _deadPool, int _id)
    {
        dead?.Invoke(_killer, _deadPool, _id);
    }

    public static void GetHit(int _hp)
    {
        gethit?.Invoke(_hp);
    }

    public static void Respawn()
    {
        respawn?.Invoke();
    }

}
