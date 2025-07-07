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
        string uniqueUserID = System.Guid.NewGuid().ToString(); // 生成一个唯一的 ID
        PhotonNetwork.AuthValues= new AuthenticationValues(uniqueUserID);

        loadUI.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();//成功会执行
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);//注册pun2事件
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);//注销事件
    }

    public void OnConnected()
    {
       
    }


    //连接成功执行的函数
    public void OnConnectedToMaster()
    {
        //关闭ui
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
    //断开服务器执行的函数
    public void OnDisconnected(DisconnectCause cause)
    {
        loadUI.SetActive(false);
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        
    }
}
