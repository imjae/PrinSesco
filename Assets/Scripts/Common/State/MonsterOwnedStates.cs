using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterOwnedStates
{
    public class Idle : State<Monster>
    {
        public override void Enter(Monster entity)
        {
        }
        public override void Execute(Monster entity)
        {
        }

        public override void Exit(Monster entity)
        {
        }
    }
    public class Attack : State<Monster>
    {
        public override void Enter(Monster entity)
        {
        }

        public override void Execute(Monster entity)
        {
        }

        public override void Exit(Monster entity)
        {
        }
    }
    public class Hit : State<Monster>
    {
        public override void Enter(Monster entity)
        {
        }

        public override void Execute(Monster entity)
        {
        }

        public override void Exit(Monster entity)
        {
        }
    }
}

