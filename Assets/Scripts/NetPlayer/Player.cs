
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
    public bool otherWin = false;
    public float[] exitTime;//�˳�ʱ��

    public float minProgress;//��С�İ�������

    public int checkAmount;

    public Transform root;

    public KeyCode[] keys;

    private float EffectDur;

    public Image[] UIPhysical;// ������

    public AudioSource audioSource;

    public AudioClip[] clips;//һ������Ԫ��0-ҡ����Ч��1-������Ч��2-��Ϣ��Ч

    public Vector3 currentPos;

    private Vector3 networkPosition; // ����������ͬ����λ��
    private int phyNetwork;//������������ͬ��
    private float lerpSpeed = 10f; // ��ֵ�ٶ�

    #region ״̬��
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
            // ���÷Ǳ����������
            Destroy(GetComponent<Rigidbody2D>()); // ��ֹ�����ͻ
        }

       

        stateMachine.Initialize(idleState);

        currentPhysical = humanPhysical;
        currentPos = root.position;
    }


    void Update()
    {
        if (!photonView.IsMine) return; // ��������Ҵ�������

        stateMachine.currentState.Update();


        UIPhysical[0].fillAmount = currentPhysical / humanPhysical;

        if (!photonView.IsMine)
        {
            // ��Զ����ҵ�λ�ú���ת���в�ֵ
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
        {//��������
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
        // ��⹥������
        if (photonView.IsMine)
        {
            AttackTrigger attackTrigger = GetComponentInChildren<AttackTrigger>();
            attackTrigger.AttackTriggle();
        }
    }

    [PunRPC]
    public void RPC_ApplyAttackEffects(int targetActorNumber)
    {
        // ����Ŀ����ҵ�״̬Ӧ�ù���Ч��
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
        // ���ö�������
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

    public float soilAmount;//��������
    public float treeWeight;//��������

    public float  humanStrength;//�˵�����
    public float  humanPhysical;//�˵�����
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
