using UnityEngine;
using Photon.Pun;

namespace Game
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerShooting : MonoBehaviourPun
    {
        [Header("Prefabs")]
        [Tooltip("Перетащите сюда ваш Bullet Prefab из Assets/Resources")]
        [SerializeField] private GameObject _bulletPrefab;

        [Header("Fire Settings")]
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _bulletSpeed = 15f;
        [SerializeField] private float _bulletLifeTime = 3f;

        private void Update()
        {
            if (!photonView.IsMine) return;

            if (Input.GetButtonDown("Fire1"))
                Shoot();
        }

        private void Shoot()
        {
            // используем имя префаба из инспектора
            var bulletGO = PhotonNetwork.Instantiate(
                _bulletPrefab.name,
                _firePoint.position,
                _firePoint.rotation
            );

            // задаём скорость и время жизни
            var rb = bulletGO.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = _firePoint.forward * _bulletSpeed;

            var bullet = bulletGO.GetComponent<Bullet>();
            bullet?.Initialize(_bulletLifeTime);
        }
    }
}
