using TicTacToeGame.Domain.Models;
using System;

namespace TicTacToeGame.Domain.Rules
{
    public static class WinConditionChecker
    {
        // Теперь jagged array, каждая строка — это int[3]
        private static readonly int[][] Lines = new[]
        {
            new[] {0,1,2},
            new[] {3,4,5},
            new[] {6,7,8},
            new[] {0,3,6},
            new[] {1,4,7},
            new[] {2,5,8},
            new[] {0,4,8},
            new[] {2,4,6}
        };

        public static bool CheckWin(GameBoard board, PlayerMark mark)
        {
            if (mark == PlayerMark.None)
                return false;   

            foreach (var line in Lines)
            {
                if (board.Cells[line[0]].Mark == mark &&
                    board.Cells[line[1]].Mark == mark &&
                    board.Cells[line[2]].Mark == mark)
                    return true;
            }
            return false;
        }

        public static bool IsDraw(GameBoard board)
        {
            // Ничья — когда нет ни одной пустой клетки
            foreach (var cell in board.Cells)
                if (cell.IsEmpty) return false;
            return true;
        }
    }
}
