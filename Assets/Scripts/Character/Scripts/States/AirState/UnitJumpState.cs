using UnityEngine;

namespace Character.Scripts.States.AirState
{
    public class UnitJumpState : UnitAirState
    {
        public UnitJumpState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            StateMachine.JumpForce = StateMachine.Unit.UnitCondition.JumpForce; 
            StateMachine.Unit.ForceReceiver.Jump(StateMachine.JumpForce);
            
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.JumpParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.JumpParameterHash);
        }

        public override void Update()
        {
            base.Update();
            if(StateMachine.Unit.CharacterController.velocity.y <= 0) 
                StateMachine.ChangeState(StateMachine.FallState);
        }
    }
}