using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class CreateRoomUI: MonoBehaviourPunCallbacks
{
    public GameObject maskUI;
    public InputField inputField;
    public GameObject roomUI;
    
    void Start()
    {
        inputField.text = "room_" + System.DateTime.Now.Ticks;
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        maskUI.SetActive(true);

        RoomOptions room = new RoomOptions();
        room.MaxPlayers = 2; 
        room.EmptyRoomTtl = 0;
        PhotonNetwork.CreateRoom(inputField.text, room);//��������

    }

    public void onCloseButton()
    {
        roomUI.SetActive(false);
    }

    //�ɹ���������
    public override void OnCreatedRoom()
    {
        Debug.Log("�����ɹ�");
        maskUI.SetActive(false);
        roomUI.SetActive(true);
    }


    //��������ʧ��
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        maskUI.SetActive(false);
    }
}
