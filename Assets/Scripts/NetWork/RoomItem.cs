
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public int owerId;//玩家编号

    public bool isReady = false;
    Text text;

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();
    }
    private void Start()
    {
        text.text = owerId.ToString();
        if(owerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            transform.Find("Button").GetComponent<Button>().onClick.AddListener(OnReadyBtn);
        }
        else
        {
            transform.Find("Image").GetComponent<Image>().color= Color.black;
        }

        ChangeReady(isReady);
    }

    public void OnReadyBtn()
    {
        isReady = !isReady;

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        table.Add("IsReady", isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);//设置自定义参数
        ChangeReady(isReady);
    }

    public void ChangeReady(bool isready)
    {
        transform.Find("Button/Text").GetComponent<Text>().text = isReady == true ? "已准备" : "未准备";
    }

}
