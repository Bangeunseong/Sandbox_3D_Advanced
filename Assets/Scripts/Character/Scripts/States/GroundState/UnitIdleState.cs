using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Scripts.States.GroundState
{
    public class UnitIdleState : UnitGroundState
    {
        public UnitIdleState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            StateMachine.MovementSpeedModifier = 0f;
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.IdleParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.IdleParameterHash);
        }

        protected override void OnCrouchStarted(InputAction.CallbackContext context)
        {
            base.OnCrouchStarted(context);
            StateMachine.ChangeState(StateMachine.CrouchState);
        }

        public override void Update()
        {
            base.Update();

            if (StateMachine.MovementDirection == Vector2.zero) return;
            StateMachine.ChangeState(StateMachine.WalkState);
        }

        protected override void OnDanceStarted(InputAction.CallbackContext context)
        {
            base.OnDanceStarted(context); 
            StateMachine.ChangeState(StateMachine.DanceState);
        }
    }
}