using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class EnemyController : MonoBehaviour, IServerObject
{
    [SerializeField]
    private Transform bodyTr;
    [SerializeField]
    private Transform headTr;
    [SerializeField]
    private CharAnimator anim;

    public void SetPos(Vector3 _pos, float _yaw)
    {
        bodyTr.rotation = Quaternion.Euler(0, _yaw, 0);

        Vector3 move = _pos - bodyTr.position;

        if (move.sqrMagnitude > 0.001f)
        {
            move = move.normalized;

            float vert = Vector3.Dot(move, bodyTr.forward);

            float hori = Vector3.Dot(move, bodyTr.right);


            anim.Move(hori, vert);
        }
        else
        {
            anim.Move(0, 0);
        }

        bodyTr.position = _pos;
    }

    public void StopMove()
    {
        anim.Move(0, 0);
    }

    public void Fire()
    {
        anim.Fire();
    }

    public void ReLoad()
    {
        anim.ReLoad();
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
