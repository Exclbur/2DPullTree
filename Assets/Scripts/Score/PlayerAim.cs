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
    [SerializeField] private Vector2 launchDir;//������·����

    private Vector2 finalDir;

    [Header("AimDot info")]
    [SerializeField] private int numberOfDots;//���ɵ������
    [SerializeField] private float spaceBeetwenDots;//������֮��ļ��
    [SerializeField] private GameObject dotPerfabs;//���Ԥ����
    [SerializeField] private Transform dotsParent;//��ĳ�����

    private GameObject[] dots;


    private void Update()
    {
 

            if (Input.GetMouseButton(0) )//Ҫ�ڰ�ס�Ĺ����г������
            {
                if (player.inventory.inventory.Count < player.ChangeBall()+1)
                    return;
                finalDir = new Vector2(AimDirected().normalized.x * launchDir.x, AimDirected().normalized.y * launchDir.y);//���������߳˷�
                DotsActive(true);

            //����׼��ʱ���������
                if (player.facingRight && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 0)
                    player.Flip();
                else if (!player.facingRight && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > 0)
                    player.Flip();

            for (int i = 0; i < dots.Length; i++)
                {
                    dots[i].transform.position = DotsPos(i * spaceBeetwenDots);//���������ߺ������趨��ķ�Χ               
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (player.inventory.inventory.Count < player.ChangeBall()+1)
                    return;

                Create(player.ChangeBall());
                DotsActive(false);
            }

        if (Input.GetMouseButton(1))//Ҫ�ڰ�ס�Ĺ����г������
        {
            if (player.inventory.inventory.Count < player.ChangeBall() + 1)
                return;
            finalDir = new Vector2(AimDirected().normalized.x * launchDir.x, AimDirected().normalized.y * launchDir.y);//���������߳˷�
            DotsActive(true);

            //����׼��ʱ���������
            if (player.facingRight && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 0)
                player.Flip();
            else if (!player.facingRight && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > 0)
                player.Flip();

            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPos(i * spaceBeetwenDots);//���������ߺ������趨��ķ�Χ               
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

    public Vector2 AimDirected()//���ض�̬��׼����
    {
        Vector2 playerPos = player.throwSword.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//��ȡ���λ��

        Vector2 direction = mousePos - playerPos;//����һ����ά����
        return direction;
    }


    public void DotsActive(bool isActive)//���ӻ���Ͷ��Ƕ�
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenderateDots()//����׼�����п��ӻ�������λ���γ�������
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPerfabs, player.throwSword.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(true);
        }
    }

    private Vector2 DotsPos(float t)//�������������õ��ĺ���
    {
        Vector2 position = (Vector2)player.throwSword.transform.position + new Vector2
            (AimDirected().normalized.x * launchDir.x,
            AimDirected().normalized.y * launchDir.y) * t + .5f * (Physics2D.gravity * player.inventory.inventory[player.ChangeBall()].data.gravity) * (t * t);

        return position;
    }
*/
}
