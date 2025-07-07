
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public enum Human {
    Lu,
    Yu
}

public class Player :MonoBehaviourPun,IPunObservable
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
    public bool otherWin = false;
    public float[] exitTime;//退出时间

    public float minProgress;//最小的拔树限制

    public int checkAmount;

    public Transform root;

    public KeyCode[] keys;

    private float EffectDur;

    public Image[] UIPhysical;// 体力条

    public AudioSource audioSource;

    public AudioClip[] clips;//一共三个元素0-摇树音效，1-用力音效，2-喘息音效

    public Vector3 currentPos;

    private Vector3 networkPosition; // 树根在网络同步的位置
    private int phyNetwork;//体力条在网络同步
    private float lerpSpeed = 10f; // 插值速度

    #region 状态类
    public PlayerStateMachine stateMachine { get; private set; }
    public Animator anim { get; private set; }
    public IdleState idleState { get; private set; }
    public ShakeState shakeState { get; private set; }
    public PullState pullState { get; private set; }
    public AttackState attackedState { get; private set; }
    public DefenseState defenseState { get; private set; }
    public TiredState tiredState { get; private set; }
    #endregion


    private UI_Win game => GameObject.Find("Canvas").GetComponent<UI_Win>();
    private void Awake()
    {
        UIPhysical = new Image[2];
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer.ActorNumber ==1)
        {
            UIPhysical[0] = GameObject.Find("Canvas/Panel/Player1Hp").GetComponent<Image>();
            UIPhysical[1] = GameObject.Find("Canvas/Panel/Player2Hp").GetComponent<Image>();
        }
        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            UIPhysical[0] = GameObject.Find("Canvas/Panel/Player2Hp").GetComponent<Image>();
            UIPhysical[1] = GameObject.Find("Canvas/Panel/Player1Hp").GetComponent<Image>();
        }

        stateMachine = new PlayerStateMachine();

        idleState = new IdleState(this, stateMachine, "Idle");
        shakeState = new ShakeState(this, stateMachine, "isShaking");
        pullState = new PullState(this, stateMachine, "isPulling");
        attackedState = new AttackState(this, stateMachine, "isAttacking");
        defenseState = new DefenseState(this, stateMachine, "isDefensing");
        tiredState = new TiredState(this, stateMachine, "isTired");
    }

    private void Start()
    {


        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            // 禁用非本地玩家输入
            Destroy(GetComponent<Rigidbody2D>()); // 防止物理冲突
        }

       

        stateMachine.Initialize(idleState);

        currentPhysical = humanPhysical;
        currentPos = root.position;
    }


    void Update()
    {
        if (!photonView.IsMine) return; // 仅本地玩家处理输入

        stateMachine.currentState.Update();


        UIPhysical[0].fillAmount = currentPhysical / humanPhysical;

        if (!photonView.IsMine)
        {
            // 对远程玩家的位置和旋转进行插值
            root.position = Vector3.Lerp(root.position, networkPosition, Time.deltaTime * lerpSpeed);
            UIPhysical[1].fillAmount = Mathf.Lerp(UIPhysical[0].fillAmount, UIPhysical[1].fillAmount, Time.deltaTime / .3f);
        }
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
   

    public void ConsumePhy(Player player)
    {
        player.currentPhysical -= player.consumePhysical[2];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {//发送数据
            stream.SendNext(root.position);
            stream.SendNext(isWin);
            stream.SendNext(isInterrupt);
            stream.SendNext(isTiring);
            stream.SendNext(isDefensing);
            stream.SendNext(isAttacking);
            stream.SendNext(UIPhysical[0].fillAmount);
            stream.SendNext(soilAmount);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
  
            otherWin = (bool)stream.ReceiveNext();
            isDefensing = (bool)stream.ReceiveNext();
            isAttacking = (bool)stream.ReceiveNext(); 
            isInterrupt = (bool)stream.ReceiveNext();
            isTiring = (bool)stream.ReceiveNext();
            UIPhysical[1].fillAmount = (float)stream.ReceiveNext();
            soilAmount = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void RPC_TriggerAttack()
    {
        // 检测攻击命中
        if (photonView.IsMine)
        {
            AttackTrigger attackTrigger = GetComponentInChildren<AttackTrigger>();
            attackTrigger.AttackTriggle();
        }
    }

    [PunRPC]
    public void RPC_ApplyAttackEffects(int targetActorNumber)
    {
        // 根据目标玩家的状态应用攻击效果
        Player targetPlayer = PhotonView.Find(targetActorNumber)?.GetComponent<Player>();
        if (targetPlayer != null)
        {
            if (targetPlayer.isResting)
            {
                targetPlayer.ConsumePhy(targetPlayer);
                if (Random.Range(0, 100) <= targetPlayer.chance)
                {
                    targetPlayer.isYeBudy = true;
                }
            }
            if (targetPlayer.isPulling)
            {
                targetPlayer.isInterrupt = true;
            }
            if (targetPlayer.isShaking)
            {
                targetPlayer.isFalling = true;
            }
            if (targetPlayer.isAttacking)
            {
                targetPlayer.isInterrupt = true;
            }
        }
    }

    [PunRPC]
    public void RPC_PlayAnimation(string animBoolName, bool value)
    {
        // 设置动画参数
        anim.SetBool(animBoolName, value);
    }
    void OnDestroy()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}

/*public class Player : MonoBehaviour
{
    public Human human;

    public float soilAmount;//泥土重量
    public float treeWeight;//树的重量

    public float  humanStrength;//人的力气
    public float  humanPhysical;//人的体力
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
    public PlayerStateMachine stateMachine { get; private set; }
    public Animator anim { get; private set; }
    public IdleState idleState {  get; private set; }
    public ShakeState shakeState { get; private set; }
    public PullState pullState { get; private set; }
    public AttackState attackedState { get; private set; }
    public DefenseState defenseState { get; private set; }
    public TiredState tiredState { get; private set; }
    #endregion

    private UI_Win game => GameObject.Find("Canvas").GetComponent<UI_Win>();
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        stateMachine = new PlayerStateMachine();

        idleState = new IdleState(this, stateMachine, "Idle");
        shakeState = new ShakeState(this, stateMachine, "isShaking");
        pullState = new PullState(this, stateMachine, "isPulling");
        attackedState = new AttackState(this, stateMachine, "isAttacking");
        defenseState = new DefenseState(this, stateMachine, "isDefensing");        
        tiredState = new TiredState(this, stateMachine, "isTired");
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

        if(game.GetWin())
        {
            stateMachine.ChangeState(idleState);
        }
    }


    public void AnimationTrigger() => stateMachine.currentState.AnimatorFinish();

    private void PhysicalUp()
    {
        if(isYeBudy)
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

    public void ConsumePhy(Player player)
    {
        player.currentPhysical -= player.consumePhysical[2];
    }

}*/
