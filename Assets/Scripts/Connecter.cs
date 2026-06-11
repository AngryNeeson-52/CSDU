using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Connecter : MonoBehaviour
{
    Session session;

    private void Start()
    {
        session = new Session();
    }
}
