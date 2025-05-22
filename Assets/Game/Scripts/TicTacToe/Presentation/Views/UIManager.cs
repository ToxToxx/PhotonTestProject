using TicTacToeGame.Domain.Models;
using TMPro;
using UnityEngine;

namespace TicTacToeGame.Presentation.Views
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statusText;

        public void ShowConnecting() =>
            statusText.text = "🔄 Подключение к серверу...";

        public void ShowWaitingForOpponent() =>
            statusText.text = "⌛ Ожидание второго игрока...";

        public void ShowGameStarted(PlayerMark localMark) =>
            statusText.text = $"✅ Игра началась! Ты — {localMark}. Ходит X.";

        public void ShowYourTurn() =>
            statusText.text = "🎯 Твой ход";

        public void ShowOpponentTurn() =>
            statusText.text = "🕹️ Ход противника";

        public void ShowWin() =>
            statusText.text = "🎉 Ты победил!";

        public void ShowLoss() =>
            statusText.text = "😞 Ты проиграл.";

        public void ShowDraw() =>
            statusText.text = "🤝 Ничья.";


    }
}
