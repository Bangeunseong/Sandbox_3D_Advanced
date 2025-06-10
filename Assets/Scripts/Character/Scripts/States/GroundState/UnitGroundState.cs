using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Scripts.States.GroundState
{
    public class UnitGroundState : UnitBaseState
    {
        public UnitGroundState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.GroundParameterHash);
        }
        
        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.GroundParameterHash);
        }
        
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!StateMachine.Unit.CharacterController.isGrounded &&
                StateMachine.Unit.CharacterController.velocity.y < Physics.gravity.y * Time.deltaTime)
            {
                StateMachine.ChangeState(StateMachine.FallState);
            }
        }
        
        public override void Update()
        {
            base.Update();

            if (!StateMachine.IsAttacking) return;
            OnAttack();
        }

        protected override void OnMoveCanceled(InputAction.CallbackContext context)
        {
            if (StateMachine.MovementDirection == Vector2.zero) return;
            
            base.OnMoveCanceled(context);
            if (StateMachine.CurrentState is UnitCrouchWalkState) StateMachine.ChangeState(StateMachine.CrouchState);
            else StateMachine.ChangeState(StateMachine.IdleState);
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            base.OnJumpStarted(context);
            StateMachine.ChangeState(StateMachine.JumpState);
        }

        private void OnAttack()
        {
            StateMachine.ChangeState(StateMachine.ComboAttackState);
        }
    }
}