using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance { get; private set; }

    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject aimingUI;
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private TextMeshProUGUI populationText;
    [SerializeField]
    private TextMeshProUGUI magazineText;
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private Image magazineBar;
    [SerializeField]
    private Image hpBar;

    private bool menuActive = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;
    }

    private void OnEnable()
    {
        NetworkEvents.gethit += HP;
    }

    private void OnDisable()
    {
        NetworkEvents.gethit -= HP;
    }

    public void Aiming(bool _YN)
    {
        if (_YN)
        {
            aimingUI.SetActive(true);
        }
        else
        {
            aimingUI.SetActive(false);
        }
    }

    public bool Menu(InputData _ID)
    {
        if(_ID.escPressed)
            menuActive = !menuActive;

        if (menuActive)
        {
            menuUI.SetActive(true);
            return true;
        }
        else
        {
            menuUI.SetActive(false);
            return false;
        }
    }

    public void Magazine(int _amount, int _max = 30)
    { 
        magazineText.text = _amount.ToString();
        magazineBar.fillAmount = (float)_amount / (float)_max;
    }

    public void Population(int _count)
    { 
        populationText.text = _count.ToString();
    }

    void HP(int _amount)
    {
        hpText.text = _amount.ToString();
        hpBar.fillAmount = (float)_amount / 100;

        if (_amount <= 0)
            GameOver();
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    void ReSpwn()
    {
        gameOverUI.SetActive(false);
    }
}
