using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> _netState = new(writePerm: NetworkVariableWritePermission.Owner);

    public TMP_Text gameText;
    public int calls = 0;

    private void Start()
    {
        gameText = GameObject.FindGameObjectWithTag("GameText").GetComponent<TMP_Text>();
        calls = 0;
    }

    private Vector3 _vel;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;

    private void Update()
    {
        if (IsOwner)
        {
            _netState.Value = new PlayerNetworkData()
            {
                Position = transform.position
            };
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, _netState.Value.Position, ref _vel, _cheapInterpolationTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsClient && IsOwner && !IsServer)
            {
                Debug.Log("Attempting To Send Game Info To Host");
                SendGameInfoToHostServerRPC(SystemInfo.deviceName);
            }
        }
    }

    [ServerRpc]
    public void SendGameInfoToHostServerRPC(string clientName)
    {
        Debug.Log($"Client '{clientName}' is interacting with Host");
        gameText.text = $"Client '{clientName}' is interacting with Host + Call: " + calls;
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _z;

        internal Vector3 Position
        {
            get => new Vector3(_x, 0, _z);

            set
            {
                _x = value.x;
                _z = value.z;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T: IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _z);
        }
    }
}
