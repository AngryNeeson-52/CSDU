using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    [SerializeField]
    private PlayerController user;
    [SerializeField]
    private GameObject dummy;

    Dictionary<int, IServerObject> players = new Dictionary<int, IServerObject>();

    HashSet<int> updated = new HashSet<int>();

    int population;

    private void Awake()
    {
        players.Add(Session.instance.id, user);

        Debug.Log(Session.instance.id + "·Î ¼³Á¤µÊ");
    }

    private void OnEnable()
    {
        NetworkEvents.spawn += Spawn;
        NetworkEvents.snapshot += SnapShot;
        NetworkEvents.dead += Dead;

        ClientSender.PlayersRequest();
    }

    private void OnDisable()
    {
        NetworkEvents.spawn -= Spawn;
        NetworkEvents.snapshot -= SnapShot;
        NetworkEvents.dead -= Dead;
    }


    void Spawn(SnapShot _snapShot)
    {
        population = _snapShot.enemys.Count;
        UIController.instance.Population(population);

        for (int i = 0; i < _snapShot.enemys.Count; i++)
        {
            PlayerInfo info = _snapShot.enemys[i];

            if (players.ContainsKey(info.id))
                continue;

            GameObject p = Instantiate(dummy, info.position, Quaternion.identity);

            IServerObject s = p.GetComponent<IServerObject>();

            players.Add(info.id, s);
        }
    }

    void SnapShot(SnapShot _snapShot)
    {
        updated.Clear();

        for (int i = 0; i < _snapShot.enemys.Count; i++)
        {
            PlayerInfo info = _snapShot.enemys[i];

            updated.Add(info.id);

            if (players.ContainsKey(info.id))
            {
                players[info.id].SetPos(info.position, info.yaw);
            }
        }

        foreach (var p in players)
        {
            if (!updated.Contains(p.Key))
            {
                p.Value.StopMove();
            }
        }
    }

    void Fire()
    { 
        
    }

    void Dead(string _killer, string _deadPool, int _id)
    {
        players[_id].Kill();
        players.Remove(_id);
        
        population--;
        UIController.instance.Population(population);
    }
}
