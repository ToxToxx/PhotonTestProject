using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Game
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private byte maxPlayersPerRoom = 4;

        void Start()
        {
            // Убеждаемся, что соединение установлено
            if (PhotonNetwork.IsConnected)
            {
                JoinRandomRoom();
            }
            else
            {
                Debug.LogWarning("Не подключены к Photon, ждем подключения...");
            }
        }

        public override void OnConnectedToMaster()
        {
            // После подключения – сразу пытаемся зайти в комнату
            JoinRandomRoom();
        }

        private void JoinRandomRoom()
        {
            Debug.Log("Попытка зайти в случайную комнату...");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Не удалось найти свободную комнату, создаём новую...");
            PhotonNetwork.CreateRoom(
                roomName: null,
                roomOptions: new RoomOptions { MaxPlayers = maxPlayersPerRoom }
            );
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Создана новая комната: " + PhotonNetwork.CurrentRoom.Name);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Зашли в комнату: " + PhotonNetwork.CurrentRoom.Name +
                      $" (Игроков в комнате: {PhotonNetwork.CurrentRoom.PlayerCount})");
            // Здесь можно инстанцировать игрока и перейти к следующему этапу
        }
    }
}
