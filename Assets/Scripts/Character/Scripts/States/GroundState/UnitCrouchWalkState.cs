using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Scripts.States.GroundState
{
    public class UnitCrouchWalkState : UnitGroundState
    {
        public UnitCrouchWalkState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.CrouchWalkParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.CrouchWalkParameterHash);
        }
        
        protected override void OnCrouchStarted(InputAction.CallbackContext context)
        {
            base.OnCrouchStarted(context);
            StateMachine.ChangeState(StateMachine.WalkState);
        }
    }
}