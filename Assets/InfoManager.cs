using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InfoManager : NetworkBehaviour
{
    public string currentRubbish;

    public override void OnNetworkSpawn()
    {

        if ((IsOwner && !IsHost) || (!IsOwner && IsHost))
        {
            this.gameObject.SetActive(false);
        }
        if (!IsOwner)
        {
            this.enabled = false;
        }
    }

    public void ChangeCurrentRubbish(string newRubbish)
    {
        currentRubbish = newRubbish;
    }
}
