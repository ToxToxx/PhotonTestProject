using TicTacToeGame.Domain.Models;
using TMPro;
using UnityEngine;

namespace TicTacToeGame.Presentation.Views
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statusText;

        public void ShowConnecting() =>
            statusText.text = "Connecting...";

        public void ShowWaitingForOpponent() =>
            statusText.text = "Waiting...";

        public void ShowGameStarted(PlayerMark localMark) =>
            statusText.text = $"Game begin! You — {localMark}. Turn X.";

        public void ShowYourTurn() =>
            statusText.text = "Your turn";

        public void ShowOpponentTurn() =>
            statusText.text = "Enemy turn";

        public void ShowWin() =>
            statusText.text = "You won";

        public void ShowLoss() =>
            statusText.text = "You lose";

        public void ShowDraw() =>
            statusText.text = "Draw.";


    }
}
