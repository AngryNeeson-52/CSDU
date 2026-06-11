using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void Fire()
    {
        anim.Play("Shoot_SingleShot_AR", -1, 0f);
    }

    public void ReLoad()
    {
        anim.SetTrigger("ReLoad");
    }

    public void Move(float _vert, float _hori)
    {
        anim.SetFloat("Vert", _vert);
        anim.SetFloat("Hori", _hori);
    }
}
