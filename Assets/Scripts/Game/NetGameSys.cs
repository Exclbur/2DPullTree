using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class NetGameSys : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        // ·ÖÅä½ÇÉ«
        if (actorNumber == 1)
        {
            PhotonNetwork.Instantiate("Player1", new Vector3(0, 0, 0),Quaternion.identity);
        }
        else if (actorNumber == 2)
        {
            PhotonNetwork.Instantiate("Player2", new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
   
}
