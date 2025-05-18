using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace Game
{
    [RequireComponent(typeof(PhotonView))]
    public class ChatManager : MonoBehaviourPun
    {
        [Header("UI References")]
        [SerializeField] private RectTransform _contentTransform;     
        [SerializeField] private GameObject _messageItemPrefab;     
        [SerializeField] private TMP_InputField _inputField;      
        [SerializeField] private Button _sendButton;                 

        private void Awake()
        {
            if (_contentTransform == null) Debug.LogError("[ChatManager] _contentTransform not assigned!");
            if (_messageItemPrefab == null) Debug.LogError("[ChatManager] _messageItemPrefab not assigned!");
            if (_inputField == null) Debug.LogError("[ChatManager] _inputField not assigned!");
            if (_sendButton == null) Debug.LogError("[ChatManager] _sendButton not assigned!");
        }

        private void OnEnable()
        {
            _sendButton.onClick.AddListener(OnSendClicked);
        }

        private void OnDisable()
        {
            _sendButton.onClick.RemoveListener(OnSendClicked);
        }

        private void OnSendClicked()
        {
            if (string.IsNullOrWhiteSpace(_inputField.text)) return;

            photonView.RPC(
                nameof(ReceiveMessage),
                RpcTarget.All,
                PhotonNetwork.NickName,
                _inputField.text
            );

            _inputField.text = string.Empty;
            _inputField.ActivateInputField();
        }

        [PunRPC]
        private void ReceiveMessage(string sender, string message)
        {
            if (_messageItemPrefab == null || _contentTransform == null) return;

            var go = Instantiate(_messageItemPrefab, _contentTransform);
            var tmp = go.GetComponent<TMP_Text>() ?? go.GetComponentInChildren<TMP_Text>();
            if (tmp == null)
            {
                Debug.LogError("[ChatManager] TMP_Text component not found on message prefab!");
                return;
            }

            tmp.text = $"[{sender}]: {message}";
            StartCoroutine(ScrollToBottom());
        }

        private IEnumerator ScrollToBottom()
        {
            yield return null;
            var scroll = _contentTransform.GetComponentInParent<ScrollRect>();
            if (scroll != null)
                scroll.verticalNormalizedPosition = 0f;
        }
    }
}
