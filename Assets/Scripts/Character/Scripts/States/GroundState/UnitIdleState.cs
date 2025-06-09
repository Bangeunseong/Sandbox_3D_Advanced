using UnityEngine;

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

        public override void Update()
        {
            base.Update();

            if (StateMachine.MovementDirection == Vector2.zero) return;
            StateMachine.ChangeState(StateMachine.WalkState);
        }
    }
}