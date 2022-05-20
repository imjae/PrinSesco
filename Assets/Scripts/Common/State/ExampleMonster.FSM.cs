using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MonsterStatesStates
{
    Idle,
    Attack,
    Hit,
    Global
}

/**
유한상태머신을 사용하는 예제 클래스이다.
몬스터를 예로 드는게 적절할 것 같아 ExampleMonster 클래스를 작성.
이 클래스는 몬스터 특성을 가진 클래스들이 상속을 받을것이라고 가정하고,
상태별로의 행동을 정의하는 클래스이다.

파일이름에 FSM을 붙여 상태만을 관리하고, 몬스터들의 특성들은
Monster.cs 파일에서 따로 관리해 준다.
*/

public partial class ExampleMonster : MonoBehaviour
{
    private State<ExampleMonster>[] states;
    private StateMachine<ExampleMonster> stateMachine;

    public MonsterStatesStates CurrentState
    {
        private set;
        get;
    }

    public void Setup()
    {
        // SlotMachine이 가질수 있는 상태 개수만큼 배열에 할당.
        states = new State<ExampleMonster>[System.Enum.GetValues(typeof(MonsterStatesStates)).Length];
        states[(int)MonsterStatesStates.Idle] = new ExampleMonsterActionByStates.Idle();
        states[(int)MonsterStatesStates.Attack] = new ExampleMonsterActionByStates.Attack();
        states[(int)MonsterStatesStates.Hit] = new ExampleMonsterActionByStates.Hit();

        stateMachine = new StateMachine<ExampleMonster>();
        stateMachine.Setup(this, states[(int)MonsterStatesStates.Idle]);
    }

    public void ChangeState(MonsterStatesStates newState)
    {
        CurrentState = newState;

        stateMachine.ChangeState(states[(int)newState]);
    }

    public void Updated()
    {
        stateMachine.Execute();
    }

    public void RevertToPreviousState()
    {
        stateMachine.RevertToPreviousState();
    }
}