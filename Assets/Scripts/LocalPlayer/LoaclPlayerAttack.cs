

public class LoaclPlayerAttack : LoaclPlayerState
{
    public LoaclPlayerAttack(LoaclPlayer _player, LoaclPlayerMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isAttacking = true;
        player.currentPhysical -= player.consumePhysical[3];
    }

    public override void Exit()
    {
        base.Exit();
        player.isAttacking = false;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.isInterrupt && player.currentPhysical <= 0)
        {
            player.isTiring = true;
            stateMachine.ChangeState(player.tiredState);
        }


        if (player.isInterrupt)
        {
            player.currentPhysical -= player.consumePhysical[3];
            if (player.currentPhysical <= 0)
            {
                player.isTiring = true;
            }

            stateMachine.ChangeState(player.tiredState);
        }

        if (player.currentPhysical <= 0)
        {
            if (player.isInterrupt)
                return;

            player.isTiring = true;
            stateMachine.ChangeState(player.tiredState);
        }
    }
}
