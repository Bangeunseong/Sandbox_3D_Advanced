using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Scripts.States.GroundState
{
    public class UnitWalkState : UnitGroundState
    {
        public UnitWalkState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            StateMachine.MovementSpeedModifier = UnitCondition.WalkSpeedModifier;
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.WalkParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.WalkParameterHash);
        }

        protected override void OnRunStarted(InputAction.CallbackContext context)
        {
            base.OnRunStarted(context);
            StateMachine.ChangeState(StateMachine.RunState);
        }
    }
}