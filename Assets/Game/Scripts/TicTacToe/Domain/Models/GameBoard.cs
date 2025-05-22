using System.Collections.Generic;
namespace TicTacToeGame.Domain.Models
{
    public class GameBoard
    {
        public IReadOnlyList<Cell> Cells { get; }
        public GameBoard()
        {
            var cells = new List<Cell>();
            for (int i = 0; i < 9; i++) cells.Add(new Cell(i));
            Cells = cells;
        }
    }
}
