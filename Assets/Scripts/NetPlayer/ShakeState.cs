using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShakeState : PlayerState
{

    private string check;
    public ShakeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isShaking = true;
        timerState = 0.5f;
        check = "";

    }

    public override void Exit()
    {
        base.Exit();
        player.isShaking = false;
        int a = 0;//a计数
        int d = 0;//d计数
        int error = 0;//错误计数
        int sum = 0;//当前部分总分
        for (int i = 0; i < check.Length - 1; i++)
        {
            char c;
            c = check[i];
            if (c == check[i + 1])
            {
                error++;
            }

            if (check[i] == 'a')
            {
                a++;

            }
            if (check[i] == 'd')
            {
                d++;
            }
        }

        sum = (a + d - error) / 2 - player.checkAmount;

        if (sum > 0)
        {
            
            player.soilAmount -= sum * 2* player.soilAmount/100;
        }

    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(player.keys[0]))//按键时会向string添加一个字符，并消耗体力值
        {
            check += "a";

            if (player.currentPhysical > 0)
            {
                player.currentPhysical -= player.consumePhysical[0];
                if (player.currentPhysical <= 0)
                {
                    player.isTiring = true;
                    player.currentPhysical = 0;
                    stateMachine.ChangeState(player.tiredState);
                }
            }

            timerState =  player.exitTime[0];
        }

        if (Input.GetKeyDown(player.keys[1]))
        {
         
            check += "d";

            if (player.currentPhysical > 0)
            {
                player.currentPhysical -= player.consumePhysical[0];
                if (player.currentPhysical <= 0)
                {
                    player.isTiring = true;
                    player.currentPhysical = 0;
              
                    stateMachine.ChangeState(player.tiredState);
                }
            }

            timerState = player.exitTime[0];
        }

        if(player.isFalling)
        {
            if (player.currentPhysical <=0)
                player.isTiring = true;
            stateMachine.ChangeState(player.tiredState);
        }

        if(timerState < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

    }

}
