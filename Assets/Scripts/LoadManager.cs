using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneList
{ 
    Menu,
    InGame,
}

[System.Serializable]
public class SceneDic
{ 
    public SceneList sceneList;
    public string name;
}

public class LoadManager : MonoBehaviour
{
    public static LoadManager instance;

    [SerializeField]
    private CanvasGroup cg;
    [SerializeField]
    private TextMeshProUGUI loadingText;
    [SerializeField]
    private Image loadingBar;
    [SerializeField]
    private List<SceneDic> sceneDic;

    Dictionary<SceneList, string> sceneList = new Dictionary<SceneList, string>();

    CancellationTokenSource cts;

    private bool isChanging = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        MakeDic();
        ScreenOn();
        loadingText.text = "Loading...";
    }

    void MakeDic()
    {
        for (int i = 0; i < sceneDic.Count; i++)
        {
            sceneList.Add(sceneDic[i].sceneList, sceneDic[i].name);
        }
    }

    public void ConnectedServer(int _id)
    {
        loadingText.text = $"Connected... {_id} Player";
        loadingBar.fillAmount = 1f;

        StartCoroutine(Clear());
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(2f);
        ScreenOff();
        yield break;
    }

    void ScreenOn()
    {
        cg.alpha = 1f;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    void ScreenOff()
    {
        cg.alpha = 0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

    public void TrySceneChange(SceneList _scene)
    {
        if (isChanging)
            return;

        isChanging = true;

        ScreenOn();

        loadingText.text = "Loading...";

        if (!sceneList.TryGetValue(_scene, out string sceneName))
        {
            loadingText.text = "씬이 존재하지 않습니다.";
            return;
        }


        SceneChange(sceneName).Forget();
    }   


    async UniTask SceneChange(string _sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_sceneName);

        while (!op.isDone)
        {
            loadingBar.fillAmount = op.progress;

            await UniTask.Yield();
        }

        loadingBar.fillAmount = 1f;

        await UniTask.Delay(500);

        isChanging = false;

        ScreenOff();
    }
}
