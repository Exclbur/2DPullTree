using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class UI_Login : MonoBehaviour,IConnectionCallbacks
{
    public GameObject loadUI;
    public GameObject LobbyUI;


    public void StartButton()
    {
        string uniqueUserID = System.Guid.NewGuid().ToString(); // ����һ��Ψһ�� ID
        PhotonNetwork.AuthValues= new AuthenticationValues(uniqueUserID);

        loadUI.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();//�ɹ���ִ��
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);//ע��pun2�¼�
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);//ע���¼�
    }

    public void OnConnected()
    {
       
    }


    //���ӳɹ�ִ�еĺ���
    public void OnConnectedToMaster()
    {
        //�ر�ui
        loadUI.SetActive(false);
        Debug.Log("successful");
        LobbyUI.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    
    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
       
    }
    //�Ͽ�������ִ�еĺ���
    public void OnDisconnected(DisconnectCause cause)
    {
        loadUI.SetActive(false);
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        
    }
}
