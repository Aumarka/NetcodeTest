using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoManager : NetworkBehaviour
{
    public static InfoManager instance;

    public int currentRubbishID;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += InitaliseInfoManager;
    }

    public override void OnNetworkSpawn()
    {
        InitaliseInfoManager();
    }

    public void InitaliseInfoManager()
    {
        // Ensures only host's info manager is active on both host and clients
        // Deactivates InfoManager Game Object if:
        // Game instance is the owner but not the host or if Game Instance is not the owner but is the host
        if ((IsOwner && !IsHost) || (!IsOwner && IsHost))
        {
            this.gameObject.SetActive(false);
        }

        //Link's hosts info manager object to the host manager
        if (IsOwner && IsHost)
        {
            GameObject.FindGameObjectWithTag("HostManager").GetComponent<HostManager>().SetInfoManager(this);
        }

        // Links client's clone of host info manager object to the client manager
        if (!IsOwner && !IsHost)
        {
            GameObject.FindGameObjectWithTag("ClientManager").GetComponent<ClientManager>().SetInfoManager(this);
        }
    }

    public void InitaliseInfoManager(Scene scene, LoadSceneMode mode)
    {
        // Ensures only host's info manager is active on both host and clients
        // Deactivates InfoManager Game Object if:
        // Game instance is the owner but not the host or if Game Instance is not the owner but is the host
        if ((IsOwner && !IsHost) || (!IsOwner && IsHost))
        {
            this.gameObject.SetActive(false);
        }

        //Link's hosts info manager object to the host manager
        if (IsOwner && IsHost)
        {
            GameObject.FindGameObjectWithTag("HostManager").GetComponent<HostManager>().SetInfoManager(this);
        }

        // Links client's clone of host info manager object to the client manager
        if (!IsOwner && !IsHost)
        {
            GameObject.FindGameObjectWithTag("ClientManager").GetComponent<ClientManager>().SetInfoManager(this);
        }
    }

    public void ChangeCurrentRubbish(int newRubbishID)
    {
        currentRubbishID = newRubbishID;
    }
}
