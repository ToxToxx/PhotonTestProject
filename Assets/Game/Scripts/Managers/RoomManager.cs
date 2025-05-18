using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Game
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        [Header("Photon Settings")]
        [SerializeField] private byte _maxPlayersPerRoom = 4;

        [Header("Prefabs")]
        [Tooltip("Перетащите сюда ваш PlayerPrefab из проекта")]
        [SerializeField] private GameObject _playerPrefab;

        private void Start()
        {
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
                roomOptions: new RoomOptions { MaxPlayers = _maxPlayersPerRoom }
            );
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Создана новая комната: " + PhotonNetwork.CurrentRoom.Name);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"Зашли в комнату: {PhotonNetwork.CurrentRoom.Name} " +
                      $"(Игроков в комнате: {PhotonNetwork.CurrentRoom.PlayerCount})");
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(-2f, 2f),
                1f,
                Random.Range(-2f, 2f)
            );

            PhotonNetwork.Instantiate(
                _playerPrefab.name,
                spawnPos,
                Quaternion.identity
            );
        }
    }
}
