using System;
using TicTacToeGame.Domain.Models;
namespace TicTacToeGame.Domain.Services
{
    public interface IGameService
    {
        PlayerMark LocalMark { get; }
        PlayerMark CurrentTurn { get; }
        event Action OnGameStart;
        event Action<int, PlayerMark> OnMoveProcessed;
        event Action<PlayerMark?> OnGameEnd; // null = draw
        void Initialize(PlayerMark localMark);
        void MakeMove(int cellIndex);
        void ProcessRemoteMove(int cellIndex, PlayerMark mark);
    }
}
