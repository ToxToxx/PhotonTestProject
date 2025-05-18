using UnityEngine;
using Photon.Pun;

namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PhotonView))]
    public class PlayerController : MonoBehaviourPun
    {
        [SerializeField] private float _moveSpeed = 5f;
        private Rigidbody _playerRb;

        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            // Управляем только своим игроком
            if (!photonView.IsMine) return;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(h, 0f, v).normalized;
            Vector3 move = dir * _moveSpeed * Time.deltaTime;

            _playerRb.MovePosition(_playerRb.position + move);
        }
    }
}
