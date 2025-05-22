using System;
using TicTacToeGame.Domain.Models;

namespace TicTacToeGame.Infrastructure.Network
{
    public interface INetworkService
    {
        void Connect();
        void JoinOrCreateRoom(int maxPlayers);
        void SendMove(int cellIndex, PlayerMark mark);

        event Action JoinedRoom;                // Вошли в комнату (может быть 1 игрок)
        event Action GameStartSignal;           // Сигнал от мастера: стартуем игру!
        event Action<int, PlayerMark> MoveReceived;
    }
}
