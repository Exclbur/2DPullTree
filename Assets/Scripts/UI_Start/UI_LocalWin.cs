using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class UI_LocalWin : MonoBehaviour
{
    public Image[] winn;
    public LoaclPlayer player1;
    public LoaclPlayer player2;
  
    private bool Wining;

    public static UI_LocalWin Instance;

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

    void Update()
    {

        if (player1.isWin || player2.isWin)
        {
            if (player2.isWin)
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

        if(player1.isWin)
            Invoke("LoadScene", 6);
        if (player2.isWin)
            Invoke("LoadScene1", 6);
    }


    private void WinStart()
    {
        winn[0].transform.DOMove(new Vector2(960, 540), 3f);
    }

    private void Disapire()
    {
        winn[0].transform.DOScale(0, 1f);
    }


    private void LoadScene()
    {
        SceneManager.LoadScene("EndScene");
    }

    private void LoadScene1()
    {
        SceneManager.LoadScene("ZhiShenEndScene");
    }



    public bool GetWin()
    {
        return Wining;
    }
}
