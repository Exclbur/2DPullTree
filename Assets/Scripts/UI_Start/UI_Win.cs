using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
public class UI_Win : MonoBehaviourPunCallbacks
{
    public Image[] winn;
    public Player player1;
    public Player player2;
    private bool isSceneLoading = false;
    private bool Wining;
    public string nextScnen;

    public static UI_Win Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Invoke("Find", .5f);
        PhotonNetwork.AutomaticallySyncScene = true;//当执行跳转场景的时候其余玩家也执行跳转
    }

    private void Find()
    {
        player1 = GameObject.Find("Player1(Clone)").GetComponent<Player>();
        player2 = GameObject.Find("Player2(Clone)").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.isWin || player2.isWin || player1.otherWin || player2.otherWin)
        {
            if (player2.isWin || player2.otherWin)
            {
                winn[0] = winn[1];
            }
            WIN();
        }
    }

    private void WIN()
    {
        WinStart();

        Invoke("Disapire", 5);
    }


    private void WinStart()
    {
        winn[0].transform.DOMove(new Vector2(960, 540), 3f);
    }

    private void Disapire()
    {
        winn[0].transform.DOScale(0, 1f);
    }

    IEnumerator EndGames(Photon.Realtime.Player loser)
    {
        yield return new WaitForSeconds(6);

       
       photonView.RPC("EndGame", RpcTarget.All, loser.NickName);
      

        yield return null;
    }


    public void CheckGameOver(Photon.Realtime.Player loser)
    {
        StartCoroutine(EndGames(loser));
    }

    [PunRPC]
    void EndGame(string loserName)
    {
        if (isSceneLoading) return; // 如果正在加载场景，直接返回
        photonView.RPC("DestroyPlayerObject", RpcTarget.All);//删除游戏个体
        photonView.RPC("SyncGameState", RpcTarget.All, true);//取消准备
        LoadRoomUIScene();
    }


    public void DestroyPlayerObject()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.LocalPlayer.TagObject != null)
        {
            PhotonNetwork.Destroy(PhotonNetwork.LocalPlayer.TagObject as GameObject);
        }
    }

    [PunRPC]
    void SyncGameState(bool isGameOver)
    {
        if (isGameOver)
        {
            // 重置准备状态
            ResetPlayerReadyState();
        }
    }

    void ResetPlayerReadyState()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { "IsReady", false }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    [PunRPC]
    void SyncLoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
        isSceneLoading = true;
    }

    void LoadRoomUIScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SyncLoadScene", RpcTarget.All, "RoomScene");
        }
    }


    public bool GetWin()
    {
        return Wining;
    }
}