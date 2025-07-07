using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerState : MonoBehaviourPun
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    private string animBoolName;
    protected float timerState;
    protected bool triggerCalled;
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName,true);
        player.photonView.RPC("RPC_PlayAnimation", RpcTarget.Others, animBoolName, true);
        triggerCalled = false; 
    }


    public virtual void Update()
    {
        timerState -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        player.photonView.RPC("RPC_PlayAnimation", RpcTarget.Others, animBoolName, false);
    }

    public virtual void AnimatorFinish()
    {
        triggerCalled = true;
    }

   
}