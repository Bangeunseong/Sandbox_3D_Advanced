using UnityEngine;

namespace Character.Scripts.States.AirState
{
    public class UnitAirState : UnitBaseState
    {
        public UnitAirState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.AirParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.AirParameterHash);
        }
    }
}