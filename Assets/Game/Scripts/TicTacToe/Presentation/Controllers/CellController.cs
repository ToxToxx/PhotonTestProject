using TicTacToeGame.Domain.Services;
using TicTacToeGame.Infrastructure.Network;
using TicTacToeGame.Presentation.Views;
using UnityEngine;

namespace TicTacToeGame.Presentation.Controllers
{
    public class CellController : MonoBehaviour
    {
        [SerializeField] private CellView view;
        public void Initialize(
            IGameService gameService,
            INetworkService networkService)
        {
            view.Button.onClick.AddListener(() =>
            {
                gameService.MakeMove(view.CellIndex);
            });

            gameService.OnMoveProcessed += (idx, mark) =>
            {
                if (idx == view.CellIndex)
                    view.DisplayMark(mark.ToString());
            };

            // Отправляем ход в сеть
            gameService.OnMoveProcessed += (idx, mark) =>
            {
                if (gameService.LocalMark == mark)
                    networkService.SendMove(idx, mark);
            };
        }
    }
}
