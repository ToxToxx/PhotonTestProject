using UnityEngine;
using TicTacToeGame.Domain.Services;
using TicTacToeGame.Infrastructure.Network;
using TicTacToeGame.Presentation.Views;
using Photon.Pun;
using TicTacToeGame.Domain.Models;

namespace TicTacToeGame.Presentation.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private PhotonNetworkService networkService;

        private IGameService gameService;

        void Awake()
        {
            // Простая «ручная» инициализация
            gameService = new GameService();
            networkService = GetComponent<PhotonNetworkService>();

            // Подписка на сетевые события
            networkService.OnPlayersReady += OnPlayersReady;
            networkService.OnMoveReceived += gameService.ProcessRemoteMove;

            // Инициализировать контроллеры ячеек
            foreach (var cellCtrl in boardView.CellControllers)
                cellCtrl.Initialize(gameService, networkService);

            // Сетевое подключение
            networkService.Connect();

            // Подписаться на события игры
            gameService.OnGameStart += () =>
                uiManager.SetStatus($"Ты — {gameService.LocalMark}, ходит X");

            gameService.OnMoveProcessed += (_, _) =>
                uiManager.SetStatus($"Ходит {gameService.CurrentTurn}");

            gameService.OnGameEnd += result =>
            {
                if (result == null) uiManager.SetStatus("Ничья");
                else if (result == gameService.LocalMark) uiManager.SetStatus("Ты победил!");
                else uiManager.SetStatus("Ты проиграл.");
            };
        }

        private void OnPlayersReady()
        {
            var localMark = PhotonNetwork.IsMasterClient ? PlayerMark.X : PlayerMark.O;
            gameService.Initialize(localMark);
        }
    }
}
