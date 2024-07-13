using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerChecker : MonoBehaviour
{
    NetworkManager networkManager;

    private void Start()
    {
        networkManager = GetComponent<NetworkManager>();

        if(networkManager != NetworkManager.Singleton)
        {
            Destroy(gameObject);
        }
    }
}
