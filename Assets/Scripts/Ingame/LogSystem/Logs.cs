using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Logs : MonoBehaviour
{
    private bool Once = true;

    private Coroutine coroutine;


    public void DestroyThis()
    {
        if (!Once)
            return;

        Once = false;

        StopCoroutine(coroutine);
        Destroy(this.gameObject);
    }

    public virtual void SetLog(string a)
    {
        this.gameObject.SetActive(true);
        coroutine = StartCoroutine(ShowCoroutine());
    }

    public virtual void SetLog(string a, string b)
    {
        this.gameObject.SetActive(true);
        coroutine = StartCoroutine(ShowCoroutine());
    }

    IEnumerator ShowCoroutine()
    {
        yield return new WaitForSeconds(3f);

        DestroyThis();

        yield break;
    }
}
