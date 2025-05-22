namespace TicTacToeGame.Domain.Models
{
    public class Cell
    {
        public int Index { get; }
        public PlayerMark Mark { get; private set; } = PlayerMark.None;
        public bool IsEmpty => Mark == PlayerMark.None;

        public Cell(int index) { Index = index; }

        public void SetMark(PlayerMark mark)
        {
            if (IsEmpty) Mark = mark;
        }
    }
}
