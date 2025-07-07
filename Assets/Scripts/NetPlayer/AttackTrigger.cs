using Photon.Pun;
using UnityEngine;

/*public class AttackTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private AudioSource audioSource;
    [SerializeField]private AudioClip[] clip;
    private RootCon rootCon;

    private void Start()
    {
        audioSource = player.GetComponent<AudioSource>();//��ȡ����ߵ�Source���
        rootCon = transform.root.GetComponentInChildren<RootCon>();
    }


    private void AnimatorTrigger()
    {
        player.AnimationTrigger();
    }

    public void AttackTriggle()//����ʶ��
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 100);

        audioSource.clip = clip[0];

        foreach (var item in colliders)
        {
                Player player = item.GetComponent<Player>();
            
                if (player.human == this.player.human)//ʶ���������Ӱ��
                { 
                    continue;
                }
                else//��ʶ�𵽵ĵз����ݲ�ͬ״̬��ɲ�ͬӰ��
                {
                    audioSource.Play();
                    if (player.isResting)//����Ϣ״̬����������������ʼ���Ч��
                    {
                        player.ConsumePhy(player);
                        if (Random.Range(0, 100) <= player.chance)
                        {
                            player.isYeBudy = true;
                        }
                    }
                    if (player.isPulling)//�԰���״̬����ж�Ч��
                    {
                        player.isInterrupt = true;
                        
                    }

                    if (player.isShaking)//������״̬����ж�++Ч��
                    {
                        player.isFalling = true;
                    }

                    if(player.isAttacking)//�Թ���״̬����ж�Ч��
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
}*/


public class AttackTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] clip;
    private RootCon rootCon;

    private void Start()
    {
        audioSource = player.GetComponent<AudioSource>();//��ȡ����ߵ�Source���
        rootCon = transform.root.GetComponentInChildren<RootCon>();
    }


    private void AnimatorTrigger()
    {
        player.AnimationTrigger();
    }

    public void AttackTriggle()//����ʶ��
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 100);
        audioSource.clip = clip[0];

        foreach (var item in colliders)
        {
            Player targetPlayer = item.GetComponent<Player>();
            if (targetPlayer != null && targetPlayer.human != player.human)
            {
                // ������Ч
                audioSource.Play();

                // ͨ������ͬ������Ч��
                player.photonView.RPC("RPC_ApplyAttackEffects", RpcTarget.All, targetPlayer.photonView.ViewID);
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