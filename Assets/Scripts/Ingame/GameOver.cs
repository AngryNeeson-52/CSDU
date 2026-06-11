using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private Button respawn;
    [SerializeField]
    private Button exit;

    private void Awake()
    {
        respawn.onClick.AddListener(ReSpawnCall);
        exit.onClick.AddListener(ExitCall);
    }

    void ReSpawnCall()
    { 

    }

    void ExitCall()
    {
        Debug.Log("나 나갈래...");
        LoadManager.instance.TrySceneChange(SceneList.Menu);
    }
}
