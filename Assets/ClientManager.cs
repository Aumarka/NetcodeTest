using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientManager : MonoBehaviour
{
    [SerializeField] TMP_Text currentRubbishText;
    [SerializeField] InfoManager infoManager;

    public void SetInfoManager(InfoManager infoManager)
    {
        this.infoManager = infoManager;
    }

    public void Update()
    {
        if(infoManager != null)
        {
            currentRubbishText.text = infoManager.currentRubbishID.ToString();
        }
    }
}
