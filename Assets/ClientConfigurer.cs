using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;

public class ClientConfigurer : MonoBehaviour
{
    [SerializeField] NetworkManager m_NetworkManager;

    UnityTransport m_Transport;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button connectButton;
    [SerializeField] Button disconnectButton;

    string m_ConnectAddress = "127.0.0.1";

    bool clientRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        m_NetworkManager = GameObject.FindObjectOfType<NetworkManager>();
        m_NetworkManager.OnClientStarted+= ClientStarted;
        m_NetworkManager.OnClientStopped += ClientStopped;

        ConfigureSessionUI();
    }

    private void OnDestroy()
    {
        m_NetworkManager.OnClientStarted -= ClientStarted;
        m_NetworkManager.OnClientStopped -= ClientStopped;
    }

    // Update is called once per frame
    void Update()
    {
        m_Transport = (UnityTransport)m_NetworkManager.NetworkConfig.NetworkTransport;
    }

    public void JoinSession()
    {
        if (inputField.text != null)
        {
            Debug.Log(inputField.text);
            string joinString = inputField.text;
            m_ConnectAddress = joinString;
        }
        else
        {
            m_ConnectAddress = "127.0.0.1";
        }

        m_Transport.SetConnectionData(m_ConnectAddress, 7777);

        m_NetworkManager.StartClient();
        ConfigureSessionUI();
        Debug.Log("Successfully Connected To Network Session!");
    }

    public void DisconnectFromSession()
    {
        m_NetworkManager.Shutdown();
        Debug.Log("Network Session Stopped!");
    }

    void ClientStarted()
    {
        clientRunning = true;
    }

    void ClientStopped(bool serverStopped)
    {
        clientRunning = false;

        ConfigureSessionUI();
    }

    void ConfigureSessionUI()
    {
        Debug.Log(clientRunning);

        if (clientRunning)
        {
            connectButton.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
            disconnectButton.gameObject.SetActive(true);
        }
        else
        {
            connectButton.gameObject.SetActive(true);
            inputField.gameObject.SetActive(true);
            disconnectButton.gameObject.SetActive(false);
        }
    }
}
