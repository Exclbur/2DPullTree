using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoaclPlayer : MonoBehaviour
{
    public Human human;

    public float soilAmount;//泥土重量
    public float treeWeight;//树的重量

    public float humanStrength;//人的力气
    public float humanPhysical;//人的体力
    public int[] consumePhysical;//消耗的体力值0-摇晃消耗，1-拔树消耗，2-防御消耗，3-打人消耗
    public int[] restorePhysical;//回复的体力值,从前往后是（idle）正常速度，（defense）缓慢速度，巨他妈慢速度
    public float currentPhysical;//当前体力条
    public float chance;

    public bool isShaking = false;//是否在松土
    public bool isPulling = false;//是否在用力拔
    public bool isDefensing = false;//是否在防御
    public bool isInterrupt = false;//是否中断
    public bool isFalling = false;
    public bool isResting = true;//是否休息
    public bool isTiring = false;//是否劳累
    public bool isYeBudy = false;//是否被加持！！！！！！！！！！！！！！！！！！
    public bool isAttacking = false;
    public bool isWin = false;

    public float[] exitTime;//退出时间

    public float minProgress;//最小的拔树限制

    public int checkAmount;

    public Transform root;

    public KeyCode[] keys;

    private float EffectDur;

    public Image UIPhysical;

    public AudioSource audioSource;

    public AudioClip[] clips;//一共三个元素0-摇树音效，1-用力音效，2-喘息音效


    #region 状态类
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
