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

        PhotonNetwork.AutomaticallySyncScene = true;//��ִ����ת������ʱ���������Ҳִ����ת
    }
    void Start()
    {
        //���ɷ�����������
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


    //������Ϸ
    public void CloseButton()
    {
        //�Ͽ�����
        PhotonNetwork.LeaveRoom();//���ش���
        lobby.ReJoinLobby();
        gameObject.SetActive(false);

    }


    //��ʼ��Ϸ
    public void StartButton()
    {
        PhotonNetwork.LoadLevel("NetPullTreeScene");
    }


    //�������
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

    //ɾ���뿪��������
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

    //����ҽ��뷿��
    public void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        CreateRoom(newPlayer);
    }

    //�����������뿪����
    public void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        DeleteRoomItem(otherPlayer);
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        
    }


    //����Զ�������仯�ص�
    public void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        RoomItem item = roomList.Find(_item => { return _item.owerId == targetPlayer.ActorNumber; });

        if (item != null)
        {
            item.isReady = (bool)changedProps["IsReady"];
            item.ChangeReady(item.isReady);
        }

        //������������ �ж�������ҵ�׼��

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
            startTf.gameObject.SetActive(isAllReady);//��ʼ��ť�Ƿ���ʾ
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
