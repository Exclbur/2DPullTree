using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RootCon : MonoBehaviour
{
    private Player player;

    public float shakeAmount = 0.1f; // ��������
    public float shakeDuration = 0.5f; // ��������ʱ��

    private Vector3 originalPosition; // ����ĳ�ʼλ��

    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
   
    }


    private void Update()
    {
        if (player.soilAmount < 110)
        {
            anim.SetBool("middle", true);
            player.photonView.RPC("RPC_PlayAnimation", RpcTarget.Others,"middle", true);
        }
        if (player.soilAmount < 80)
        {
            anim.SetBool("end", true);
            player.photonView.RPC("RPC_PlayAnimation", RpcTarget.Others, "end", true);
        }
    }
   
    public IEnumerator Shake()
    {
      
        originalPosition = transform.position;

        float elapsedTime = 0f;

        // �ڶ�������ʱ���ڽ��ж���
        while (elapsedTime < shakeDuration)
        {
            // ÿ�ζ���ʱ����������λ��Ϊԭʼλ�� + һ�������ƫ����
            float xOffset = Random.Range(-shakeAmount, shakeAmount);
            float yOffset = Random.Range(-shakeAmount, shakeAmount);
            transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);

            // ��������ʱ��
            elapsedTime += Time.deltaTime;

            // �ȴ���һ֡
            yield return null;
            
        }
        
        // ����������ȷ������ص���ʼλ��
        transform.position = originalPosition;
        
    }

}
