using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerAim : MonoBehaviour
{
 /*   private Player player;
    
    private void Start()
    {
        player = GetComponent<Player>();
        GenderateDots();
        DotsActive(false);
    }
    [Header("Skill info")]
    [SerializeField] private Vector2 launchDir;//定义着路方向

    private Vector2 finalDir;

    [Header("AimDot info")]
    [SerializeField] private int numberOfDots;//生成点的数量
    [SerializeField] private float spaceBeetwenDots;//两个点之间的间隔
    [SerializeField] private GameObject dotPerfabs;//点的预制体
    [SerializeField] private Transform dotsParent;//点的出发点

    private GameObject[] dots;


    private void Update()
    {
 

            if (Input.GetMouseButton(0) )//要在按住的过程中持续检测
            {
                if (player.inventory.inventory.Count < player.ChangeBall()+1)
                    return;
                finalDir = new Vector2(AimDirected().normalized.x * launchDir.x, AimDirected().normalized.y * launchDir.y);//进行抛物线乘法
                DotsActive(true);

            //在瞄准的时候调换方向
                if (player.facingRight && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 0)
                    player.Flip();
                else if (!player.facingRight && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > 0)
                    player.Flip();

            for (int i = 0; i < dots.Length; i++)
                {
                    dots[i].transform.position = DotsPos(i * spaceBeetwenDots);//根据抛物线函数来设定点的范围               
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (player.inventory.inventory.Count < player.ChangeBall()+1)
                    return;

                Create(player.ChangeBall());
                DotsActive(false);
            }

        if (Input.GetMouseButton(1))//要在按住的过程中持续检测
        {
            if (player.inventory.inventory.Count < player.ChangeBall() + 1)
                return;
            finalDir = new Vector2(AimDirected().normalized.x * launchDir.x, AimDirected().normalized.y * launchDir.y);//进行抛物线乘法
            DotsActive(true);

            //在瞄准的时候调换方向
            if (player.facingRight && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 0)
                player.Flip();
            else if (!player.facingRight && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > 0)
                player.Flip();

            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPos(i * spaceBeetwenDots);//根据抛物线函数来设定点的范围               
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (player.inventory.inventory.Count < player.ChangeBall() + 1)
                return;
            player.SetDir(new Vector2(-finalDir.x,-finalDir.y * .5f));
            Create(player.ChangeBall());
            DotsActive(false);
        }

    }
    private void Create(int a)
    {
        GameObject newBall = Instantiate(player.inventory.inventory[a].data.perfabe, player.throwSword.transform.position, Quaternion.identity);
        BallBounce ball = newBall.GetComponent<BallBounce>();
        ItemObject ob = newBall.GetComponent<ItemObject>();
        ob.nextPickTime = 2;
        ball.SetDir(finalDir, player.inventory.inventory[a].data.gravity);

        player.inventory.RemoveBall(a);
    }

    public Vector2 AimDirected()//返回动态瞄准方向
    {
        Vector2 playerPos = player.throwSword.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//获取鼠标位置

        Vector2 direction = mousePos - playerPos;//返回一个二维向量
        return direction;
    }


    public void DotsActive(bool isActive)//可视化的投射角度
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenderateDots()//在瞄准过程中可视化各个点位，形呈抛物线
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPerfabs, player.throwSword.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(true);
        }
    }

    private Vector2 DotsPos(float t)//绘制抛物线所用到的函数
    {
        Vector2 position = (Vector2)player.throwSword.transform.position + new Vector2
            (AimDirected().normalized.x * launchDir.x,
            AimDirected().normalized.y * launchDir.y) * t + .5f * (Physics2D.gravity * player.inventory.inventory[player.ChangeBall()].data.gravity) * (t * t);

        return position;
    }
*/
}
