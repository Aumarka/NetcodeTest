using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InfoNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<InfoNetworkData> _infoState = new(writePerm: NetworkVariableWritePermission.Owner);
    public InfoManager _infoManager;

    private void Awake()
    {
        _infoManager = GetComponent<InfoManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            _infoState.Value = new InfoNetworkData()
            {
                RubbishID = _infoManager.currentRubbishID
            };
        }
        else
        {
            _infoManager.currentRubbishID = _infoState.Value.RubbishID;
        }
    }

    struct InfoNetworkData : INetworkSerializable
    {
        private int _rubbishID;

        internal int RubbishID
        {
            get => _rubbishID;

            set
            {
                _rubbishID = value;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _rubbishID);
        }
    }
}
