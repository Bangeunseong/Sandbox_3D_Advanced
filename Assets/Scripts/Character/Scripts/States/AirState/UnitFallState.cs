using UnityEngine;

namespace Character.Scripts.States.AirState
{
    public class UnitFallState : UnitAirState
    {
        public UnitFallState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.FallParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.FallParameterHash);
        }

        public override void Update()
        {
            base.Update();
            if(StateMachine.Unit.CharacterController.isGrounded) 
                StateMachine.ChangeState(StateMachine.IdleState);
        }
    }
}