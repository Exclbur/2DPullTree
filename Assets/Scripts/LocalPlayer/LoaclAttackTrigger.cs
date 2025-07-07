using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaclAttackTrigger : MonoBehaviour
{
    private LoaclPlayer player => GetComponentInParent<LoaclPlayer>();
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] clip;
    private RootCon rootCon;

    private void Start()
    {
        audioSource = player.GetComponent<AudioSource>();//获取打击者的Source组件
        rootCon = transform.root.GetComponentInChildren<RootCon>();
    }


    private void AnimatorTrigger()
    {
        player.AnimationTrigger();
    }

    public void AttackTriggle()//攻击识别
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 100);

        audioSource.clip = clip[0];

        foreach (var item in colliders)
        {
            LoaclPlayer player = item.GetComponent<LoaclPlayer>();

            if (player.human == this.player.human)//识别自身不造成影响
            {
                continue;
            }
            else//对识别到的敌方根据不同状态造成不同影响
            {
                audioSource.Play();
                if (player.isResting)//对休息状态造成消耗体力及概率激励效果
                {
                    player.ConsumePhy(player);
                    if (Random.Range(0, 100) <= player.chance)
                    {
                        player.isYeBudy = true;
                    }
                }
                if (player.isPulling)//对拔树状态造成中断效果
                {
                    player.isInterrupt = true;

                }

                if (player.isShaking)//对松土状态造成中断++效果
                {
                    player.isFalling = true;
                }

                if (player.isAttacking)//对攻击状态造成中断效果
                {
                    player.isInterrupt = true;
                }

            }
        }
    }

    private void ShakeTrigger()
    {
        audioSource.clip = clip[1];
        audioSource.Play();
        StartCoroutine(rootCon.Shake());
    }
}


