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
        private bool gameActive = false;

        public event Action OnGameStart;
        public event Action<int, PlayerMark> OnMoveProcessed;
        public event Action<PlayerMark?> OnGameEnd;

        public void Initialize(PlayerMark localMark)
        {
            LocalMark = localMark;
            CurrentTurn = PlayerMark.X;
            gameActive = true;
            OnGameStart?.Invoke();
        }

        public void MakeMove(int cellIndex)
        {
            if (!gameActive || CurrentTurn != LocalMark)
                return;
            ApplyMove(cellIndex, LocalMark, isLocal: true);
        }

        public void ProcessRemoteMove(int cellIndex, PlayerMark mark)
        {
            if (!gameActive)
                return;
            ApplyMove(cellIndex, mark, isLocal: false);
        }

        private void ApplyMove(int index, PlayerMark mark, bool isLocal)
        {
            if (!board.Cells[index].IsEmpty)
                return;

            board.Cells[index].SetMark(mark);
            OnMoveProcessed?.Invoke(index, mark);

            if (WinConditionChecker.CheckWin(board, mark))
            {
                gameActive = false; // <--- блокируем игру!
                OnGameEnd?.Invoke(mark);
                return;
            }
            if (WinConditionChecker.IsDraw(board))
            {
                gameActive = false;
                OnGameEnd?.Invoke(null);
                return;
            }
            CurrentTurn = mark == PlayerMark.X ? PlayerMark.O : PlayerMark.X;
        }
    }
}
