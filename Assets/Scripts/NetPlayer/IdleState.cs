using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isResting = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.isResting = false;
    }

    public override void Update()
    {
        base.Update();

        if ((Input.GetKeyDown(player.keys[0]) || Input.GetKeyDown(player.keys[1])) && timerState < 0)
        {
            stateMachine.ChangeState(player.shakeState);
        }

        if (Input.GetKey(player.keys[2]) && timerState <0)
        {
            stateMachine.ChangeState(player.pullState);
        }

        if (Input.GetKey(player.keys[3]) && timerState < 0)
        {
            stateMachine.ChangeState(player.defenseState);
        }

        if (Input.GetKeyDown(player.keys[4]) && timerState < 0)
        {
            stateMachine.ChangeState(player.attackedState);
        }

        if (player.currentPhysical <= 0)
        {
            player.isTiring = true;
            stateMachine.ChangeState(player.tiredState);
        }

        #region ÌåÁ¦»Ö¸´
        if (player.currentPhysical <player.humanPhysical)
        {
            player.currentPhysical += player.restorePhysical[0] *Time.deltaTime;
        }

        if(player.currentPhysical > player.humanPhysical)
        {
            player.currentPhysical = player.humanPhysical;
        }
        #endregion


    }
}
