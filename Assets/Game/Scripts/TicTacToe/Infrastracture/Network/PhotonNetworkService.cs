using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TicTacToeGame.Domain.Models;
using TicTacToeGame.Infrastructure.Config;

namespace TicTacToeGame.Infrastructure.Network
{
    [RequireComponent(typeof(PhotonView))]
    public class PhotonNetworkService : MonoBehaviourPunCallbacks, INetworkService
    {
        private const string RoomName = "TicTacToeRoom";
        private const int MaxPlayersCount = 2;

        [SerializeField] private PhotonSettings config;

        public event Action JoinedRoom;
        public event Action GameStartSignal;
        public event Action<int, PlayerMark> MoveReceived;

        private void Awake()
        {
            if (config == null)
            {
                Debug.LogError("[PhotonNetworkService] PhotonSettings не назначен");
                return;
            }

            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = config.AppId;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = config.Region;
        }

        public void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinOrCreateRoom(int maxPlayers)
        {
            PhotonNetwork.JoinOrCreateRoom(
                RoomName,
                new RoomOptions { MaxPlayers = (byte)maxPlayers },
                TypedLobby.Default
            );
        }

        public override void OnConnectedToMaster()
        {
            JoinOrCreateRoom(MaxPlayersCount);
        }

        // ВАЖНО: override!
        public override void JoiningRoom()
        {
            Debug.Log("Ты действительно в комнате!");
            // Вся твоя логика запуска после входа в комнату — здесь.
            JoinedRoom?.Invoke();
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersCount && PhotonNetwork.IsMasterClient)
                SignalGameStart();
        }


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"[PNS] PlayerEntered: {newPlayer.NickName} ({PhotonNetwork.CurrentRoom.PlayerCount}/{MaxPlayersCount})");
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersCount && PhotonNetwork.IsMasterClient)
                SignalGameStart();
        }

        public void SignalGameStart()
        {
            photonView.RPC(nameof(RPC_GameStart), RpcTarget.All);
        }

        [PunRPC]
        private void RPC_GameStart()
        {
            Debug.Log("[PNS] Получен сигнал старта игры!");
            GameStartSignal?.Invoke();
        }

        public void SendMove(int cellIndex, PlayerMark mark)
        {
            photonView.RPC(nameof(RPC_ReceiveMove), RpcTarget.Others, cellIndex, (int)mark);
        }

        [PunRPC]
        private void RPC_ReceiveMove(int cellIndex, int markValue)
        {
            MoveReceived?.Invoke(cellIndex, (PlayerMark)markValue);
        }
    }
}
