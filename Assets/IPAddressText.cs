using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using TMPro;

public class IPAddressText : MonoBehaviour
{
    TMP_Text ipText;
    void Start()
    {
        ipText = GetComponent<TMP_Text>();

        string localIP = GetLocalIPAddress();
        if (localIP != null)
        {
            ipText.text = localIP;
        }
        else
        {
            ipText.text = "Unable to retrieve the local IP address.";
        }
    }

    string GetLocalIPAddress()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught while retrieving IP address: " + ex.Message);
        }

        return null;
    }
}
