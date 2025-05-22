using System;
using TicTacToeGame.Domain.Models;
using TicTacToeGame.Domain.Rules;

namespace TicTacToeGame.Domain.Services
{
    public class GameService : IGameService
    {
        private readonly GameBoard board = new GameBoard();
        public PlayerMark LocalMark { get; private set; }
        public PlayerMark CurrentTurn { get; private set; }

        public event Action OnGameStart;
        public event Action<int, PlayerMark> OnMoveProcessed;
        public event Action<PlayerMark?> OnGameEnd;

        public void Initialize(PlayerMark localMark)
        {
            LocalMark = localMark;
            CurrentTurn = PlayerMark.X;
            OnGameStart?.Invoke();
        }

        public void MakeMove(int cellIndex)
        {
            if (CurrentTurn != LocalMark) return;
            ApplyMove(cellIndex, LocalMark, isLocal: true);
        }

        public void ProcessRemoteMove(int cellIndex, PlayerMark mark)
            => ApplyMove(cellIndex, mark, isLocal: false);

        private void ApplyMove(int index, PlayerMark mark, bool isLocal)
        {
            board.Cells[index].SetMark(mark);
            OnMoveProcessed?.Invoke(index, mark);

            if (WinConditionChecker.CheckWin(board, mark))
            {
                OnGameEnd?.Invoke(mark);
                return;
            }
            if (WinConditionChecker.IsDraw(board))
            {
                OnGameEnd?.Invoke(null);
                return;
            }

            CurrentTurn = mark == PlayerMark.X ? PlayerMark.O : PlayerMark.X;
        }
    }
}
