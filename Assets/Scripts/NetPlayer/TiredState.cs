using UnityEngine;

public class TiredState : PlayerState
{
    public TiredState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (player.currentPhysical < 0)
            player.currentPhysical = 0;

        if(player.isInterrupt)
        {
            
            player.isInterrupt = true;
            timerState = .5f;
        }

        if(player.isFalling)
            timerState = 1;

        player.audioSource.clip = player.clips[1];

        if(player.isTiring)
            player.audioSource.Play();
    }

    public override void Exit()
    {
        base.Exit();

        player.isInterrupt = false; 
        player.isFalling = false;
        player.isTiring = false;

    }

    public override void Update()
    {
        base.Update();

        

        if(player.isTiring)//休息时刻需恢复一半的体力条才能继续其余动作
        {

            player.currentPhysical += player.restorePhysical[0]*Time.deltaTime;

            if(player.currentPhysical >= player.humanPhysical/2)
            {
                stateMachine.ChangeState(player.idleState);
            }

        }

        if(timerState <= 0 && player.isFalling && !player.isTiring)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if(timerState <=0 && player.isInterrupt && !player.isTiring)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
