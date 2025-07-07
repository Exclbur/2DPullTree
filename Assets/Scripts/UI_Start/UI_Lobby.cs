using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class UI_Lobby : MonoBehaviourPunCallbacks
{
    public GameObject lobbyUI;
    public GameObject maskUI;
    TypedLobby lobby;//大厅对象

    public Transform contectTf;
    public GameObject roomPerfabs;
    public GameObject roomUI;
    void Start()
    {
        lobby = new TypedLobby("fpsLoby", LobbyType.SqlLobby);//大厅名字+可搜索

        PhotonNetwork.JoinLobby(lobby);
    }

    public void ReJoinLobby()
    {
        PhotonNetwork.JoinLobby(lobby);

    }


    //进入大厅回调
    public override void OnJoinedLobby()
    {
        Debug.Log("enter lobby");
    }

    public void CloseButton()
    {
        PhotonNetwork.Disconnect();
        lobbyUI.SetActive(false);

    }

    //刷新列表房间
    public void OnUpdateRoom()
    {
        maskUI.SetActive(true);

        PhotonNetwork.GetCustomRoomList(lobby, "1=1");//执行方法后会获得回调
    }

    //刷新房间回调
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        maskUI.SetActive(false);
        Debug.Log("刷新成功");

        ClearRoomList();

        for(int i = 0;i< roomList.Count;i++)
        {
            GameObject obj = Instantiate(roomPerfabs, contectTf);

            obj.SetActive(true);

            string roomName = roomList[i].Name;

            obj.transform.Find("roomName").GetComponent<Text>().text = roomName;
            obj.transform.Find("joinBtn").GetComponent<Button>().onClick.AddListener(delegate()
            {
                Debug.Log(roomName);

                maskUI.SetActive(true);

                PhotonNetwork.JoinRoom(roomName);//加入房间
            });
        }
    }

    public override void OnJoinedRoom()
    {
        //加入房间回调
        maskUI.SetActive(false); 
        roomUI.SetActive(true);//显示房间UI、
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //加入房间失败
        maskUI.SetActive(false);

    }

    //清除已经存在的房间
    private void ClearRoomList()
    {
        while (contectTf.childCount != 0)
        {
            DestroyImmediate(contectTf.GetChild(0).gameObject);
        }
    }

}
    
