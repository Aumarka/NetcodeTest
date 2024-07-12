using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InfoNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<InfoNetworkData> _infoState = new(writePerm: NetworkVariableWritePermission.Owner);
    private InfoManager _infoManager;

    private void Start()
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
                RubbishName = _infoManager.currentRubbish
            };
        }
        else
        {
            _infoManager.currentRubbish = _infoState.Value.RubbishName;
        }
    }

    struct InfoNetworkData : INetworkSerializable
    {
        private string _rubbishName;

        internal string RubbishName
        {
            get => _rubbishName;

            set
            {
                _rubbishName = value;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _rubbishName);
        }
    }
}
