using UnityEngine;
using UnityEngine.UI;

namespace TicTacToeGame.Presentation.Views
{
    public class CellView : MonoBehaviour
    {
        public int CellIndex;
        [SerializeField] private Text label;
        public Button Button => GetComponent<Button>();
        public void DisplayMark(string mark) => label.text = mark;
    }
}
