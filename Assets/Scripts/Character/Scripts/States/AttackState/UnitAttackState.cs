using UnityEngine;

namespace Character.Scripts.States.AttackState
{
    public class UnitAttackState : UnitBaseState
    {
        public UnitAttackState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            StateMachine.MovementSpeedModifier = 0f;
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.AttackParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.AttackParameterHash);
        }
    }
}