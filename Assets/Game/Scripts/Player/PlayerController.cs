using UnityEngine;
using Photon.Pun;

namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PhotonView))]
    public class PlayerController : MonoBehaviourPun
    {
        [SerializeField] private float moveSpeed = 5f;
        private Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // Управляем только своим игроком
            if (!photonView.IsMine) return;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(h, 0f, v).normalized;
            Vector3 move = dir * moveSpeed * Time.deltaTime;

            rb.MovePosition(rb.position + move);
        }
    }
}
