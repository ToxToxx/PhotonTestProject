using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class PhotonLauncher : MonoBehaviourPunCallbacks
    {
        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("✔ Connected to Photon as " + PhotonNetwork.NickName);
        }
    }

}
