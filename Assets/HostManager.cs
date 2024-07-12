using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    [SerializeField] InfoManager infoManager;

    public void SetInfoManager(InfoManager infoManager)
    {
        this.infoManager = infoManager;
    }

    public void IncreaseRubbishID()
    {
        if(infoManager != null)
        {
            infoManager.currentRubbishID++;
        }
    }
}
