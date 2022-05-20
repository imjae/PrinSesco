using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태를 표시하는 클래스의 이름이 대부분 비슷하기 때문에, 네임스페이스 사용하여 구분
// State를 상속받는 상태일때의 동작을 나타낼 때, 해당 몬스터 내부에 선언 된 동작들로 구성되어
// 해당 클래스와 상태의 대상이 되는 클래스의 참조 관계가 흐트러지지 않도록 관리한다.

namespace ExampleMonsterActionByStates
{
    public class Idle : State<ExampleMonster>
    {
        public override void Enter(ExampleMonster entity)
        {
        }
        public override void Execute(ExampleMonster entity)
        {
        }

        public override void Exit(ExampleMonster entity)
        {
        }
    }
    public class Attack : State<ExampleMonster>
    {
        public override void Enter(ExampleMonster entity)
        {
        }

        public override void Execute(ExampleMonster entity)
        {
        }

        public override void Exit(ExampleMonster entity)
        {
        }
    }
    public class Hit : State<ExampleMonster>
    {
        public override void Enter(ExampleMonster entity)
        {
        }

        public override void Execute(ExampleMonster entity)
        {
        }

        public override void Exit(ExampleMonster entity)
        {
        }
    }
}

