using UnityEngine;
using UnityEngine.InputSystem;

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

        protected override void OnCrouchStarted(InputAction.CallbackContext context)
        {
            base.OnCrouchStarted(context);
            StateMachine.ChangeState(StateMachine.CrouchState);
        }

        protected override void OnRunStarted(InputAction.CallbackContext context)
        {
            base.OnRunStarted(context);
            StateMachine.ChangeState(StateMachine.WalkState);
        }
    }
}