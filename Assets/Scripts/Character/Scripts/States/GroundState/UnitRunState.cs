using UnityEngine;

namespace Character.Scripts.States.GroundState
{
    public class UnitRunState : UnitGroundState
    {
        public UnitRunState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.MovementSpeedModifier = UnitCondition.SprintSpeedModifier;
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.RunParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.RunParameterHash);
        }
    }
}