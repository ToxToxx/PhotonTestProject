using System;
using TicTacToeGame.Domain.Models;
using TicTacToeGame.Domain.Rules;

namespace TicTacToeGame.Domain.Services
{
    public class GameService : IGameService
    {
        private GameBoard board = new GameBoard();
        public PlayerMark LocalMark { get; private set; }
        public PlayerMark CurrentTurn { get; private set; }
        private bool gameActive = false;

        public event Action OnGameStart;
        public event Action<int, PlayerMark> OnMoveProcessed;
        public event Action<PlayerMark?> OnGameEnd;
        public bool IsGameActive() => gameActive;

        public void Initialize(PlayerMark localMark)
        {
            board = new GameBoard(); // <--- сбрасываем поле для новой игры!
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

            // --- Сначала обработать ход, только потом событие!
            if (WinConditionChecker.CheckWin(board, mark))
            {
                gameActive = false;
                OnMoveProcessed?.Invoke(index, mark);
                OnGameEnd?.Invoke(mark);
                return;
            }
            if (WinConditionChecker.IsDraw(board))
            {
                gameActive = false;
                OnMoveProcessed?.Invoke(index, mark);
                OnGameEnd?.Invoke(null);
                return;
            }

            CurrentTurn = mark == PlayerMark.X ? PlayerMark.O : PlayerMark.X;
            OnMoveProcessed?.Invoke(index, mark);
        }

    }
}
