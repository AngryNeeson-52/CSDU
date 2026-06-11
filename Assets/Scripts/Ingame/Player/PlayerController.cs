using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IServerObject
{
    enum PState
    { 
        Basic,
        Reloading,
        Shooting,
        Dead,
    }

    [SerializeField]
    private Transform bodyTr;
    [SerializeField]
    private Transform headTr;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private CharAnimator anim;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float mouseSpeed = 70f;
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float rayCastDist = 1.1f;

    [SerializeField]
    private float fireRate = 0.1f;
    [SerializeField]
    private float jumpRate = 0.4f;
    [SerializeField]
    private float reloadTime = 1f;
    [SerializeField]
    private int magazine = 30;

    private PState pstate = PState.Basic;

    Coroutine coroutine = null;

    private float yaw;
    private float pitch;

    private bool canJump = true;

    private float currentJump = 0f;

    private int currentMagazine = 30;

    public void SetPos(Vector3 _pos, float _yaw)
    {
        bodyTr.position = _pos;
    }

    public void StopMove()
    { 
    
    }

    public void Kill()
    { 
    
    }

    public void GroundCheck()
    {
        Debug.DrawRay(bodyTr.transform.position, Vector3.down, Color.red);

        if (Physics.Raycast(bodyTr.transform.position, Vector3.down, rayCastDist, ground))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    public void Turn(InputData _ID)
    {
        if (_ID.mouseVert == 0 && _ID.mouseHori == 0)
            return;

        yaw += _ID.mouseHori * mouseSpeed;
        pitch -= _ID.mouseVert * mouseSpeed;

        pitch = Mathf.Clamp(pitch, -60f, 60f);


        bodyTr.rotation = Quaternion.Euler(0f, yaw, 0f);

        headTr.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public void Move(InputData _ID)
    {
        if (_ID.hori != 0 || _ID.vert != 0)
        {
            Vector3 dir = bodyTr.rotation * new Vector3(_ID.hori, 0, _ID.vert);

            if (dir.sqrMagnitude > 1f)
                dir.Normalize();
            
            rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

            anim.Move(_ID.hori, _ID.vert);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    public void UpdateToServer(InputData _ID)
    {
        if (_ID.hori != 0 || _ID.vert != 0 || _ID.mouseHori != 0 || _ID.mouseVert != 0)
        {
            Vector3 a = bodyTr.rotation.eulerAngles;

            ClientSender.Move(_ID.hori, _ID.vert, a.y);
        }
    }

    public void Jump(InputData _ID)
    {
        currentJump += Time.fixedDeltaTime;

        if (!_ID.jumpPressed || !canJump)
            return;

        if (currentJump >= jumpRate)
        {
            currentJump = 0;
            rb.AddForce(bodyTr.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void Aiming(bool _YN)
    {
        if (_YN)
        {
            mouseSpeed = 20f;
        }
        else
        {
            mouseSpeed = 70f;
        }
    }

    public void Fire(InputData _ID)
    {
        if (!_ID.firePressed)
            return;

        if (pstate == PState.Basic && currentMagazine > 0)
        {
            currentMagazine--;
            UIController.instance.Magazine(currentMagazine, magazine);

            StateChanger(fireRate, PState.Shooting);

            anim.Fire();

            ClientSender.Fire(headTr.forward);



            pitch -= 1f;
            headTr.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }


    public void ReLoad(InputData _ID)
    {
        if (!_ID.reloadPressed)
            return;

        if (pstate == PState.Basic)
        {
            currentMagazine = magazine;
            UIController.instance.Magazine(magazine, currentMagazine);

            StateChanger(reloadTime, PState.Reloading);

            anim.ReLoad();
        }
    }

    void StateChanger(float _time, PState _change)
    {
        pstate = _change;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(StateBacker(_time));
    }

    IEnumerator StateBacker(float _time)
    {
        yield return new WaitForSeconds(_time);

        pstate = PState.Basic;

        yield break;
    }
}
