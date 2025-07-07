using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaclPlayerState : MonoBehaviour
{
    protected LoaclPlayerMachine stateMachine;
    protected LoaclPlayer player;

    private string animBoolName;
    protected float timerState;
    protected bool triggerCalled;
    public LoaclPlayerState(LoaclPlayer _player, LoaclPlayerMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }


    public virtual void Update()
    {
        timerState -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimatorFinish()
    {
        triggerCalled = true;
    }
}

