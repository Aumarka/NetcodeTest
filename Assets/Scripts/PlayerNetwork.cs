using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> _netState = new(writePerm: NetworkVariableWritePermission.Owner);


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
                SendGameInfoToHostServerRPC();
            }
        }
    }

    [ServerRpc]
    public void SendGameInfoToHostServerRPC()
    {
        Debug.Log("Client is interacting with Host Player Object");
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
