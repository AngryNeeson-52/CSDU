using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private Button startBTN;
    [SerializeField]
    private Button endBTN;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private float infoTime = 3f;


    private bool clickOnce = true;
    private float currentTime = 0f;


    private void Awake()
    {
        startBTN.onClick.AddListener(TryStartGame);
        endBTN.onClick.AddListener(QuitGame);
    }

    private void OnEnable()
    {
        NetworkEvents.nickname += WaitResult;
    }

    private void OnDisable()
    {
        NetworkEvents.nickname -= WaitResult;
    }

    private void Update()
    {
        if (currentTime < infoTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            infoText.text = "";
        }

        ServerUpdater.instance.CheckTask();
    }

    void WaitResult(bool _TF)
    {
        if (_TF)
        {
            LoadManager.instance.TrySceneChange(SceneList.InGame);
        }
        else
        {
            InfoText("NickName is Already Exist");
            clickOnce = true;
        }
    }

    void InfoText(string _str)
    {
        currentTime = 0f;
        infoText.text = _str;
    }

    void TryStartGame()
    {
        if (!clickOnce)
            return;

        clickOnce = false;

        string nick = nameText.text.Replace("\u200B", "").Trim();

        if (nick.Length > 12)
        {
            InfoText("Name should be less then 12 char");
            clickOnce = true;
            return;
        }

        if (string.IsNullOrWhiteSpace(nick))
        {
            ClientSender.NickNameSend($"Player {Session.instance.id}");
        }
        else
        {
            /*
            foreach (char c in nick)
            {
                Debug.Log($"char: {(int)c}");
            }
            */

            ClientSender.NickNameSend(nick);
        }
    }

    void QuitGame()
    {
        if (!clickOnce)
            return;

        clickOnce = false;

        Session.instance.Disconnect();
        Application.Quit();
    }
}
