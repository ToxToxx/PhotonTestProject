using UnityEngine;
namespace TicTacToeGame.Infrastructure.Config
{
    [CreateAssetMenu(fileName = "PhotonSettings", menuName = "TicTacToeGame/PhotonSettings")]
    public class PhotonSettings : ScriptableObject
    {
        public string AppId;
        public string Region;
    }
}
