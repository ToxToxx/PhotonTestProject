using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System;
using TicTacToeGame.Domain.Models;
using TicTacToeGame.Infrastructure.Config;

namespace TicTacToeGame.Infrastructure.Network
{
    public class PhotonNetworkService : MonoBehaviourPunCallbacks, INetworkService
    {
        [SerializeField] private PhotonSettings config;
        public event Action<int, PlayerMark> OnMoveReceived;
        public event Action OnPlayersReady;

        void Awake()
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = config.AppId;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = config.Region;
        }

        public void Connect() => PhotonNetwork.ConnectUsingSettings();

        public override void OnConnectedToMaster() => JoinOrCreateRoom(2);

        public void JoinOrCreateRoom(int maxPlayers)
        {
            var opts = new RoomOptions { MaxPlayers = (byte)maxPlayers };
            PhotonNetwork.JoinOrCreateRoom("TicTacToeRoom", opts, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                OnPlayersReady?.Invoke();
        }

        public void SendMove(int cellIndex, PlayerMark mark)
        {
            photonView.RPC(nameof(RPC_ReceiveMove), RpcTarget.Others, cellIndex, (int)mark);
        }

        [PunRPC]
        private void RPC_ReceiveMove(int index, int mark)
        {
            OnMoveReceived?.Invoke(index, (PlayerMark)mark);
        }
    }
}
