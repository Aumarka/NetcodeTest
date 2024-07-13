using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class DisplayIPAddress : MonoBehaviour
{
    void Start()
    {
        string localIP = GetLocalIPAddress();
        if (localIP != null)
        {
            Debug.Log("Local IP Address: " + localIP);
        }
        else
        {
            Debug.Log("Unable to retrieve the local IP address.");
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
                    Debug.Log(ip.ToString());
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