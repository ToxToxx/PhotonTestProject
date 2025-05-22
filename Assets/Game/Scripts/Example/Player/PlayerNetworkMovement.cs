using UnityEngine;
using Photon.Pun;

namespace Game
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerNetworkMovement : MonoBehaviourPun, IPunObservable
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;

        private Vector3 _networkPosition;
        private Vector3 _syncStartPosition;
        private float _syncTime;
        private float _syncDelay;

        private void Start()
        {
            if (!photonView.IsMine)
            {
                _networkPosition = transform.position;
                _syncStartPosition = transform.position;
            }
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                Vector3 dir = new Vector3(h, 0f, v).normalized;
                transform.Translate(dir * _moveSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                _syncTime += Time.deltaTime;
                if (_syncDelay > 0f)
                {
                    float t = _syncTime / _syncDelay;
                    transform.position = Vector3.Lerp(_syncStartPosition, _networkPosition, t);
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                _syncStartPosition = transform.position;
                _networkPosition = (Vector3)stream.ReceiveNext();
                _syncTime = 0f;

                _syncDelay = Time.time - (float)info.SentServerTime;
                if (_syncDelay < 0f) _syncDelay = 0f;
            }
        }
    }
}
