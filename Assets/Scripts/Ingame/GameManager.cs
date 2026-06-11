using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public struct InputData
{
    public float vert;
    public float hori;
    public float mouseHori;
    public float mouseVert;
    public bool jumpPressed;
    public bool firePressed;
    public bool aimPressed;
    public bool escPressed;
    public bool reloadPressed;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }


    enum GameState
    { 
        OnPlay,
        Dead,
        MENU,
    }

    private readonly InputChecker IC = new InputChecker();
    private InputData ID = new InputData();
    private GameState state;

    [SerializeField]
    private CamController Cam;
    [SerializeField]
    private PlayerController PC;
    [SerializeField]
    private UIController UC;
    [SerializeField]
    private MiniMap MM;

    private bool aimOn;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
    }

    void Update()
    {
        PreExecute();

        UpdateExecute();
    }

    private void FixedUpdate()
    {
        FixedUpdateExecute();
    }

    private void LateUpdate()
    {
        LateUpdateExecute();
    }

    void PreExecute()
    {
        ServerUpdater.instance.CheckTask();

        BTNCheck();

        MenuEvent();
    }

    void UpdateExecute()
    {
        if (state != GameState.OnPlay)
            return;

        AimEvent();

        //PC.GroundCheck();

        PC.Turn(ID);

        PC.Fire(ID);
        PC.ReLoad(ID);
    }

    void FixedUpdateExecute()
    {
        if (state != GameState.OnPlay)
            return;

        PC.Move(ID);
        //PC.Jump(ID);
        PC.UpdateToServer(ID);
    }

    void LateUpdateExecute()
    {
        Cam.CameraUpdate();
        MM.SetMiniMap();
    }

    void BTNCheck()
    {
        ID.hori = IC[BTN_FLOAT.Horizontal];
        ID.vert = IC[BTN_FLOAT.Vertical];
        
        ID.mouseHori = IC[BTN_FLOAT.MouseHori];
        ID.mouseVert = IC[BTN_FLOAT.MouseVert];

        ID.jumpPressed = IC[BTN_BOOL.Jump];
        ID.firePressed = IC[BTN_BOOL.Fire];
        ID.aimPressed = IC[BTN_BOOL.Aiming];
        ID.escPressed = IC[BTN_BOOL.ESC];
        ID.reloadPressed = IC[BTN_BOOL.ReLoad];
    }

    void MenuEvent()
    {
        if (UC.Menu(ID))
        {
            state = GameState.MENU;
        }
        else if (state == GameState.MENU)
        {
            state = GameState.OnPlay;
        }
    }

    void AimEvent()
    {
        if (aimOn == ID.aimPressed)
            return;

        aimOn = ID.aimPressed;


        UC.Aiming(aimOn);
        Cam.Aiming(aimOn);
        PC.Aiming(aimOn);
    }

    void GameOver()
    {
        state = GameState.Dead;
    }

    void ReGame()
    { 
    
    }
}
