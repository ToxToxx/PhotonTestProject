using TicTacToeGame.Domain.Models;
using TicTacToeGame.Domain.Services;
using TicTacToeGame.Infrastructure.Network;
using TicTacToeGame.Presentation.Views;
using UnityEngine;

namespace TicTacToeGame.Presentation.Controllers
{
    public class CellController : MonoBehaviour
    {
        [SerializeField] private CellView view;
        private GameController gameController;

        public void Initialize(IGameService gameService, INetworkService networkService, GameController controller)
        {
            gameController = controller;

            // При клике делаем ход и сразу обновляем статус
            view.Button.onClick.AddListener(() =>
            {
                gameService.MakeMove(view.CellIndex);
                gameController.UpdateTurnStatusExternal();
            });

            // Показываем метку на клетке после хода
            gameService.OnMoveProcessed += (idx, mark) =>
            {
                if (idx == view.CellIndex)
                    view.DisplayMark(mark);

                // Тут не надо обновлять статус — это делает GameController
            };

            // Передача хода по сети только если локальный игрок
            gameService.OnMoveProcessed += (idx, mark) =>
            {
                if (gameService.LocalMark == mark)
                    networkService.SendMove(idx, mark);
            };
        }
    }
}
