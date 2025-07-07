using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaclPlayerMachine : MonoBehaviour
{
    public LoaclPlayerState currentState { get; private set; }

    // Start is called before the first frame update

    public void Initialize(LoaclPlayerState _playerState)
    {
        currentState = _playerState;
        currentState.Enter();
    }

    public void ChangeState(LoaclPlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
