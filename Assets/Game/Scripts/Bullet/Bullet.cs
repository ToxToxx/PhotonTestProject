using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class Bullet : MonoBehaviourPun
    {
        private float _lifeTime;

        public void Initialize(float lifeTime)
        {
            _lifeTime = lifeTime;
            StartCoroutine(DestroyAfter());
        }

        private IEnumerator DestroyAfter()
        {
            yield return new WaitForSeconds(_lifeTime);
            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!photonView.IsMine) return;
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
