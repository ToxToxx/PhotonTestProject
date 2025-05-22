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

            view.Button.onClick.AddListener(() =>
            {
                gameService.MakeMove(view.CellIndex);
                gameController.UpdateTurnStatusExternal();
            });

            gameService.OnMoveProcessed += (idx, mark) =>
            {
                if (idx == view.CellIndex)
                    view.DisplayMark(mark);
            };

            gameService.OnMoveProcessed += (idx, mark) =>
            {
                if (gameService.LocalMark == mark)
                    networkService.SendMove(idx, mark);
            };
        }
    }
}
