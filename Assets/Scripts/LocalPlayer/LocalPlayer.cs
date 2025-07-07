using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoaclPlayer : MonoBehaviour
{
    public Human human;

    public float soilAmount;//��������
    public float treeWeight;//��������

    public float humanStrength;//�˵�����
    public float humanPhysical;//�˵�����
    public int[] consumePhysical;//���ĵ�����ֵ0-ҡ�����ģ�1-�������ģ�2-�������ģ�3-��������
    public int[] restorePhysical;//�ظ�������ֵ,��ǰ�����ǣ�idle�������ٶȣ���defense�������ٶȣ����������ٶ�
    public float currentPhysical;//��ǰ������
    public float chance;

    public bool isShaking = false;//�Ƿ�������
    public bool isPulling = false;//�Ƿ���������
    public bool isDefensing = false;//�Ƿ��ڷ���
    public bool isInterrupt = false;//�Ƿ��ж�
    public bool isFalling = false;
    public bool isResting = true;//�Ƿ���Ϣ
    public bool isTiring = false;//�Ƿ�����
    public bool isYeBudy = false;//�Ƿ񱻼ӳ֣�����������������������������������
    public bool isAttacking = false;
    public bool isWin = false;

    public float[] exitTime;//�˳�ʱ��

    public float minProgress;//��С�İ�������

    public int checkAmount;

    public Transform root;

    public KeyCode[] keys;

    private float EffectDur;

    public Image UIPhysical;

    public AudioSource audioSource;

    public AudioClip[] clips;//һ������Ԫ��0-ҡ����Ч��1-������Ч��2-��Ϣ��Ч


    #region ״̬��
    public LoaclPlayerMachine stateMachine { get; private set; }
    public Animator anim { get; private set; }
    public LoaclPlayerIdle idleState { get; private set; }
    public LoaclPlayerShake shakeState { get; private set; }
    public LoaclPlayerPull pullState { get; private set; }
    public LoaclPlayerAttack attackedState { get; private set; }
    public LoaclPlayerDefense defenseState { get; private set; }
    public LocalPlayerTired tiredState { get; private set; }
    #endregion

    private UI_LocalWin game => GameObject.Find("Canvas").GetComponent<UI_LocalWin>();
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        stateMachine = new LoaclPlayerMachine();

        idleState = new LoaclPlayerIdle(this, stateMachine, "Idle");
        shakeState = new LoaclPlayerShake(this, stateMachine, "isShaking");
        pullState = new LoaclPlayerPull(this, stateMachine, "isPulling");
        attackedState = new LoaclPlayerAttack(this, stateMachine, "isAttacking");
        defenseState = new LoaclPlayerDefense(this, stateMachine, "isDefensing");
        tiredState = new LocalPlayerTired(this, stateMachine, "isTired");
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);

        currentPhysical = humanPhysical;
    }


    void Update()
    {
        stateMachine.currentState.Update();

        UIPhysical.fillAmount = currentPhysical / humanPhysical;

        PhysicalUp();

        EffectDur -= Time.deltaTime;

        if (game.GetWin())
        {
            stateMachine.ChangeState(idleState);
        }
    }


    public void AnimationTrigger() => stateMachine.currentState.AnimatorFinish();

    private void PhysicalUp()
    {
        if (isYeBudy)
        {
            EffectDur = 5;
            humanStrength = 120;

            if (EffectDur < 0)
            {
                humanStrength = 100;
                isYeBudy = false;
            }

        }


    }

    public void ConsumePhy(LoaclPlayer player)
    {
        player.currentPhysical -= player.consumePhysical[2];
    }

}
