using UnityEngine;
using UnityEngine.UI;

namespace TicTacToeGame.Presentation.Views
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text statusText;
        public void SetStatus(string msg) => statusText.text = msg;
    }
}
