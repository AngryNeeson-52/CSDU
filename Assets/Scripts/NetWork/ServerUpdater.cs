using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ServerUpdater : MonoBehaviour
{
    public static ServerUpdater instance { get; private set; }

    public bool connected = true;
    public ConcurrentQueue<Action> TaskRequest = new ConcurrentQueue<Action>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void CheckTask()
    {
        while(TaskRequest.TryDequeue(out Action result))
        {
            result?.Invoke();
        }
    }
}
