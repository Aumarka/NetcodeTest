using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;

public class HostConfigurer : MonoBehaviour
{
    [SerializeField] NetworkManager m_NetworkManager;

    UnityTransport m_Transport;

    public Button startSessionButton;
    public Button stopSessionButton;

    string m_ConnectAddress = "127.0.0.1";

    bool serverRunning = false;

    private void Awake()
    {
        m_NetworkManager = GameObject.FindObjectOfType<NetworkManager>();
        m_NetworkManager.OnServerStarted += ServerStarted;
        m_NetworkManager.OnServerStopped += ServerStopped;

        if (m_NetworkManager.IsHost)
        {
            serverRunning = true;
        }

        ConfigureSessionUI();
    }

    private void OnDestroy()
    {
        m_NetworkManager.OnServerStarted -= ServerStarted;
        m_NetworkManager.OnServerStopped -= ServerStopped;
    }

    // Update is called once per frame
    void Update()
    {
        m_Transport = (UnityTransport)m_NetworkManager.NetworkConfig.NetworkTransport;
    }

    public void StartNetworkSession()
    {
       
        string localIP = GetLocalIPAddress();
        if (localIP != null)
        {
            m_ConnectAddress = localIP;
        }
        else
        {
            m_ConnectAddress = "127.0.0.1";
        }

        m_Transport.SetConnectionData(m_ConnectAddress, 7777);

        m_NetworkManager.StartHost();
        ConfigureSessionUI();
        Debug.Log("Network Session Started!");
    }

    public void StopNetworkSession()
    {
        m_NetworkManager.Shutdown();
        Debug.Log("Network Session Stopped!");
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


    void ServerStarted()
    {
        serverRunning = true;
    }

    void ServerStopped(bool serverStopped)
    {
        serverRunning = false;

        ConfigureSessionUI();
    }

    void ConfigureSessionUI()
    {
        Debug.Log(serverRunning);

        if (serverRunning)
        {
            startSessionButton.gameObject.SetActive(false);
            stopSessionButton.gameObject.SetActive(true);
        }
        else
        {
            startSessionButton.gameObject.SetActive(true);
            stopSessionButton.gameObject.SetActive(false);
        }
    }
}
