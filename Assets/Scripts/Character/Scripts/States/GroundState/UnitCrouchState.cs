using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Scripts.States.GroundState
{
    public class UnitCrouchState : UnitGroundState
    {
        public UnitCrouchState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            StateMachine.MovementSpeedModifier = UnitCondition.CrouchSpeedModifier;
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.CrouchParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.CrouchParameterHash);
        }

        protected override void OnCrouchStarted(InputAction.CallbackContext context)
        {
            base.OnCrouchStarted(context);
            StateMachine.ChangeState(StateMachine.IdleState);
        }
        
        public override void Update()
        {
            base.Update();

            if (StateMachine.MovementDirection == Vector2.zero) return;
            StateMachine.ChangeState(StateMachine.CrouchWalkState);
        }
    }
}