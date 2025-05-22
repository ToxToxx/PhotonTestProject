using UnityEngine;
using Photon.Pun;
using TicTacToeGame.Domain.Models;
using TicTacToeGame.Domain.Services;
using TicTacToeGame.Infrastructure.Network;
using TicTacToeGame.Presentation.Views;

namespace TicTacToeGame.Presentation.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private PhotonNetworkService networkService;

        private IGameService gameService;

        private void Awake()
        {
            // Подписки на сетевые события ДО подключения к серверу!
            networkService.JoinedRoom += () => uiManager.ShowWaitingForOpponent();
            networkService.GameStartSignal += OnGameStart;
            networkService.MoveReceived += OnRemoteMove;

            gameService = new GameService();

            // Инициализация всех ячеек и привязка кликов
            foreach (var cellCtrl in boardView.CellControllers)
                cellCtrl.Initialize(gameService, networkService, this);

            // Показываем статус подключения
            uiManager.ShowConnecting();

            // Запуск соединения с Photon
            networkService.Connect();
        }

        private void OnGameStart()
        {
            // Выбор метки (X у MasterClient, O у второго)
            var localMark = PhotonNetwork.IsMasterClient ? PlayerMark.X : PlayerMark.O;
            gameService.Initialize(localMark);
            uiManager.ShowGameStarted(localMark);

            // Подписка на событие хода
            gameService.OnMoveProcessed += (_, _) => UpdateTurnStatus();
            gameService.OnGameEnd += ShowEndStatus;

            // Сразу обновляем статус хода
            UpdateTurnStatus();
        }

        private void UpdateTurnStatus()
        {
            if (gameService.CurrentTurn == gameService.LocalMark)
                uiManager.ShowYourTurn();
            else
                uiManager.ShowOpponentTurn();
        }

        private void OnRemoteMove(int idx, PlayerMark mark)
        {
            gameService.ProcessRemoteMove(idx, mark);
            // Не нужно вручную вызывать UpdateTurnStatus — оно вызовется через OnMoveProcessed
        }

        private void ShowEndStatus(PlayerMark? winner)
        {
            if (winner == null)
                uiManager.ShowDraw();
            else if (winner == gameService.LocalMark)
                uiManager.ShowWin();
            else
                uiManager.ShowLoss();
        }


        // Делаем метод публичным, чтобы CellController мог обновить статус мгновенно после клика
        public void UpdateTurnStatusExternal()
        {
            UpdateTurnStatus();
        }
    }
}
