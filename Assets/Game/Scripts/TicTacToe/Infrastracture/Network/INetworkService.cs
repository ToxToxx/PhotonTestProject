using System;
using TicTacToeGame.Domain.Models;
using UnityEngine;


namespace TicTacToeGame.Infrastructure.Network
{
    public interface INetworkService
    {
        void Connect();
        void JoinOrCreateRoom(int maxPlayers);
        void SendMove(int cellIndex, PlayerMark mark);
        event Action<int, PlayerMark> OnMoveReceived;
        event Action OnPlayersReady;
    }
}
