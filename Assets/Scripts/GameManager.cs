using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public float LoopTime = 3.0f;

    void Start()
    {
    }

    void Update()
    {
        LoopTime -= Time.deltaTime;

        if (LoopTime <= 0.0f)
        {
            NetworkManager manager = GameObject.FindObjectOfType<NetworkManager>();
            manager.ServerChangeScene(NetworkManager.networkSceneName);
        }
    }
}
