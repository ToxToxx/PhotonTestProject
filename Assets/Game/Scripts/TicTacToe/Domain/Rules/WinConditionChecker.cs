using System.Linq;
using TicTacToeGame.Domain.Models;
namespace TicTacToeGame.Domain.Rules
{
    public static class WinConditionChecker
    {
        private static readonly int[,] Lines = {
            {0,1,2},{3,4,5},{6,7,8},
            {0,3,6},{1,4,7},{2,5,8},
            {0,4,8},{2,4,6}
        };

        public static bool CheckWin(GameBoard board, PlayerMark mark)
        {
            return Lines
                .Cast<int[]>()
                .Any(line =>
                    board.Cells[line[0]].Mark == mark &&
                    board.Cells[line[1]].Mark == mark &&
                    board.Cells[line[2]].Mark == mark
                );
        }

        public static bool IsDraw(GameBoard board) =>
            board.Cells.All(c => !c.IsEmpty);
    }
}
