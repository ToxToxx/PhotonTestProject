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
            // Все подписки делаем ОДИН РАЗ
            networkService.JoinedRoom += () => uiManager.ShowWaitingForOpponent();
            networkService.GameStartSignal += OnGameStart;
            networkService.MoveReceived += OnRemoteMove;

            gameService = new GameService();
            gameService.OnMoveProcessed += (_, _) => UpdateTurnStatus();
            gameService.OnGameEnd += ShowEndStatus;

            foreach (var cellCtrl in boardView.CellControllers)
                cellCtrl.Initialize(gameService, networkService, this);

            uiManager.ShowConnecting();
            networkService.Connect();
        }

        private void OnGameStart()
        {
            // В начале каждой игры метку назначаем
            var localMark = PhotonNetwork.IsMasterClient ? PlayerMark.X : PlayerMark.O;
            gameService.Initialize(localMark);
            uiManager.ShowGameStarted(localMark);
            UpdateTurnStatus();
        }

        private void UpdateTurnStatus()
        {
            Debug.Log($"[TURN] LocalMark={gameService.LocalMark}, CurrentTurn={gameService.CurrentTurn}");

            if (!gameService.IsGameActive()) // Добавь такой геттер, чтобы не показывать статус после конца игры
                return;

            if (gameService.CurrentTurn == gameService.LocalMark)
                uiManager.ShowYourTurn();
            else
                uiManager.ShowOpponentTurn();
        }

        private void OnRemoteMove(int idx, PlayerMark mark)
        {
            gameService.ProcessRemoteMove(idx, mark);
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

        // Чтобы CellController мог обновить статус мгновенно после клика
        public void UpdateTurnStatusExternal()
        {
            UpdateTurnStatus();
        }
    }
}
