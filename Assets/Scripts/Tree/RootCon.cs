using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RootCon : MonoBehaviour
{
    private Player player;

    public float shakeAmount = 0.1f; // 抖动幅度
    public float shakeDuration = 0.5f; // 抖动持续时间

    private Vector3 originalPosition; // 物体的初始位置

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

        // 在抖动持续时间内进行抖动
        while (elapsedTime < shakeDuration)
        {
            // 每次抖动时，设置物体位置为原始位置 + 一个随机的偏移量
            float xOffset = Random.Range(-shakeAmount, shakeAmount);
            float yOffset = Random.Range(-shakeAmount, shakeAmount);
            transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);

            // 增加已用时间
            elapsedTime += Time.deltaTime;

            // 等待下一帧
            yield return null;
            
        }
        
        // 抖动结束后，确保物体回到初始位置
        transform.position = originalPosition;
        
    }

}
