using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseState :PlayerState
{
    public DefenseState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isDefensing = true;
        timerState = 4;
    }

    public override void Exit()
    {
        base.Exit();
        player.isDefensing = false;
        timerState = player.exitTime[1];
    }

    public override void Update()
    {
        base.Update();

        #region ÃÂ¡¶ª÷∏¥
        if (player.currentPhysical < player.humanPhysical && timerState > 4)
        {
            player.currentPhysical += player.restorePhysical[1] * Time.deltaTime;
        }

        if (player.currentPhysical > player.humanPhysical)
        {
            player.currentPhysical = player.humanPhysical;
        }

        if(player.currentPhysical > 0 && timerState < 0)
        {
            player.currentPhysical -= player.consumePhysical[3]*Time.deltaTime;
        }

        if(player.currentPhysical <= 0)
        {
            player.isTiring = true;
            stateMachine.ChangeState(player.tiredState);
        }

        if (Input.GetKeyUp(player.keys[3]))
        {
            stateMachine.ChangeState(player.idleState);
        }
        #endregion

    }


}
