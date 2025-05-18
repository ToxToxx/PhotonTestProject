using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace Game
{
    public class PhotonLauncher : MonoBehaviourPunCallbacks
    {
        [Header("UI")]
        [SerializeField] private TMP_InputField _nickInputField;
        [SerializeField] private Button _connectButton;

        private void Awake()
        {
            _connectButton.interactable = false;
            _nickInputField.onValueChanged.AddListener((s) =>
                _connectButton.interactable = !string.IsNullOrWhiteSpace(s)
            );
            _connectButton.onClick.AddListener(OnConnectClicked);
        }

        private void OnConnectClicked()
        {
            PhotonNetwork.NickName = _nickInputField.text.Trim();
            if (string.IsNullOrEmpty(PhotonNetwork.NickName))
                PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);

            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("✔ Connected as " + PhotonNetwork.NickName);
        }
    }
}
