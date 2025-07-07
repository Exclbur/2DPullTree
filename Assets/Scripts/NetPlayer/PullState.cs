using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullState : PlayerState
{
    private float progress;

    public PullState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isPulling = true;

        player.audioSource.clip = player.clips[0];//²¥·ÅÒôÐ§
        player.audioSource.Play();

    }

    public override void Exit()
    {
        base.Exit();
        player.isPulling = false;
        progress = 0;
        timerState = player.exitTime[1];

        player.audioSource.Pause();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(player.keys[2]))
        {
            player.currentPhysical -= player.consumePhysical[1] * Time.deltaTime;

            progress += (player.humanStrength-player.soilAmount) * Time.deltaTime;
        }

        if (progress > player.minProgress)
        {
            player.root.position = new Vector2(player.root.position.x, player.root.position.y + (player.humanStrength - player.soilAmount) * Time.deltaTime /50);
            
            if(player.soilAmount>0)
                player.soilAmount -= player.soilAmount /(-player.root.position.y- (float)5.5) *Time.deltaTime;
        }

        if(player.currentPhysical <= 0)
        {
            stateMachine.ChangeState(player.tiredState);
        }

        if(player.isInterrupt)
        {
            player.currentPhysical -= player.consumePhysical[3];
            stateMachine.ChangeState(player.tiredState);
        }


        if(player.root.position.y >= -5.5)
        {
            player.isWin = true;
            UI_Win.Instance.CheckGameOver(PhotonNetwork.LocalPlayer);
        }

        if (Input.GetKeyUp(player.keys[2]))
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
