// Assets/Scripts/Presentation/Views/CellView.cs
using TicTacToeGame.Domain.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToeGame.Presentation.Views
{
    public class CellView : MonoBehaviour
    {
        public int CellIndex;            // Задаётся в инспекторе
        public Button Button => GetComponent<Button>();
        [SerializeField] private TextMeshProUGUI label;

        public void DisplayMark(PlayerMark mark)
        {
            if (mark == PlayerMark.None)
                return;

            label.text = mark.ToString(); // "X" или "O"
            Button.interactable = false;  // больше кликов не позволит
        }
    }
}
