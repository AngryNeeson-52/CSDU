using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Announce : MonoBehaviour
{
    [SerializeField]
    private int MaxCount = 5;
    [SerializeField]
    private Logs infolog;
    [SerializeField]
    private Logs killlog;
    [SerializeField]
    private GameObject board;

    private Queue<Logs> panels = new Queue<Logs>();

    private void OnEnable()
    {
        NetworkEvents.welcome += MakeInfoLog;
        NetworkEvents.dead += MakeKillLog;
    }
    private void OnDisable()
    {
        NetworkEvents.welcome -= MakeInfoLog;
        NetworkEvents.dead -= MakeKillLog;
    }

    void MakeKillLog(string _killer, string _deadPool, int _id)
    {
        LogCountCheck();

        Logs a = Instantiate(killlog, board.transform);

        a.SetLog(_killer, _deadPool);
    }

    void MakeInfoLog(string _name)
    {
        LogCountCheck();

        Logs a = Instantiate(infolog, board.transform);

        a.SetLog(_name);

        Debug.Log("¾Ë¶÷ ¶ä");
    }

    void LogCountCheck()
    {
        if (panels.Count < MaxCount)
            return;

        Logs a = panels.Dequeue();

        a.DestroyThis();
    }
}
