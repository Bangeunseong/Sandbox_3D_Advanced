using UnityEngine;

namespace Character.Scripts.States.GroundState
{
    public class UnitDanceState : UnitGroundState
    {
        public UnitDanceState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.MovementSpeedModifier = 0f;
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.DanceParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.DanceParameterHash);
        }
    }
}