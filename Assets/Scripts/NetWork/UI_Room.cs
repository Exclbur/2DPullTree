using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UI_Room : MonoBehaviour,IInRoomCallbacks
{
    public Transform contentTf;
    public Transform startTf;
    public GameObject roomPerfab;
    public List<RoomItem> roomList;
    public UI_Lobby lobby;
    private void Awake()
    {
        roomList = new List<RoomItem>();

        PhotonNetwork.AutomaticallySyncScene = true;//当执行跳转场景的时候其余玩家也执行跳转
    }
    void Start()
    {
        //生成房间里的玩家项
        for(int i = 0;i< PhotonNetwork.PlayerList.Length;i++)
        {
            Photon.Realtime.Player p = PhotonNetwork.PlayerList[i];
            CreateRoom(p);
        }
    }


    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    //结束游戏
    public void CloseButton()
    {
        //断开连接
        PhotonNetwork.LeaveRoom();//返回大厅
        lobby.ReJoinLobby();
        gameObject.SetActive(false);

    }


    //开始游戏
    public void StartButton()
    {
        PhotonNetwork.LoadLevel("NetPullTreeScene");
    }


    //生成玩家
    public void CreateRoom(Photon.Realtime.Player P)
    {
        GameObject obj = Instantiate(roomPerfab, contentTf);
        obj.SetActive(true);
        RoomItem item =  obj.AddComponent<RoomItem>();


        item.owerId = P.ActorNumber;

        roomList.Add(item);

        object val; 
        if(P.CustomProperties.TryGetValue("IsReady",out val))
        {
            item.isReady = (bool)val;
        }
    }

    //删除离开房间的玩家
    public void DeleteRoomItem(Photon.Realtime.Player p)
    {
        RoomItem item = roomList.Find((RoomItem _item) => 
        { return p.ActorNumber == _item.owerId; });

        if(item!= null)
        {
            Destroy(item.gameObject);
            roomList.Remove(item);
        }
    }

    //新玩家进入房间
    public void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        CreateRoom(newPlayer);
    }

    //房间里的玩家离开房间
    public void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        DeleteRoomItem(otherPlayer);
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        
    }


    //玩家自定义参数变化回调
    public void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        RoomItem item = roomList.Find(_item => { return _item.owerId == targetPlayer.ActorNumber; });

        if (item != null)
        {
            item.isReady = (bool)changedProps["IsReady"];
            item.ChangeReady(item.isReady);
        }

        //如果是主机玩家 判断所有玩家的准备

        if(PhotonNetwork.IsMasterClient)
        {
            bool isAllReady = true;

            for(int i = 0;i<roomList.Count;i++)
            {
                if (roomList[i].isReady == false)
                {
                    isAllReady = false;
                    break;
                }
            }
            startTf.gameObject.SetActive(isAllReady);//开始按钮是否显示
        }
    }

    public void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CheckAllPlayersReady();
        }
    }

    private void CheckAllPlayersReady()
    {
        bool isAllReady = true;
        foreach (RoomItem item in roomList)
        {
            if (!item.isReady)
            {
                isAllReady = false;
                break;
            }
        }
        startTf.gameObject.SetActive(isAllReady);
    }
}
