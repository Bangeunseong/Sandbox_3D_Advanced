using Character.Scripts.Data;
using UnityEngine;

namespace Character.Scripts.States.AttackState
{
    public class UnitComboAttackState : UnitAttackState
    {
        private bool alreadyAppliedCombo;
        private bool alreadyAppliedForce;
        private AttackInfoData attackInfoData;
        
        public UnitComboAttackState(UnitStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Unit.AnimationData.ComboAttackParameterHash);

            alreadyAppliedCombo = false;
            alreadyAppliedForce = false;

            // 다음 Combo공격 Index 정보
            var comboIndex = StateMachine.ComboIndex;
            attackInfoData = StateMachine.Unit.UnitCondition.UnitData.AttackData.GetAttackInfoData(comboIndex);
            StateMachine.Unit.Animator.SetInteger(StateMachine.Unit.AnimationData.ComboAttackIndexHash, comboIndex);
        }
        
        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Unit.AnimationData.ComboAttackParameterHash);

            // 콤보가 가능하지 않다면 ComboIndex를 0으로 reset
            if (!alreadyAppliedCombo) StateMachine.ComboIndex = 0;
        }
        
        public override void Update()
        {
            base.Update();

            ForceMove();

            var normalizedTime = GetNormalizedTime(StateMachine.Unit.Animator, "Attack");
            if (normalizedTime < 1f)
            {
                // ForceTransitionTime (0.1 : 애니메이션 초반)
                if (normalizedTime >= attackInfoData.ForceTransitionTime)
                    TryApplyForce();

                // ComboTransitionTime (0.8 : 애니메이션 끝자락)
                if (normalizedTime >= attackInfoData.ComboTransitionTime)
                    TryComboAttack();
            }
            else // Animation이 끝났을 때 (normalizedTime이 1.0일 때)
            {
                if (alreadyAppliedCombo)
                {
                    // 다음 ComboAttack 정보를 전달
                    StateMachine.ComboIndex = attackInfoData.ComboStateIndex;
                    StateMachine.ChangeState(StateMachine.ComboAttackState);
                }
                else
                {
                    // 콤보 실패시 Idle로 (공격종료)
                    StateMachine.ChangeState(StateMachine.IdleState);
                }
            }
        }

        private void TryComboAttack()
        {
            if (alreadyAppliedCombo) return;

            if (attackInfoData.ComboStateIndex == -1) return;

            if (!StateMachine.IsAttacking) return;

            alreadyAppliedCombo = true;
        }

        private void TryApplyForce()
        {
            if (alreadyAppliedForce) return;
            alreadyAppliedForce = true;
            
            StateMachine.Unit.ForceReceiver.Reset();

            // 앞으로 나가면서 공격 모션을 위한 힘 가중
            StateMachine.Unit.ForceReceiver.AddForce(StateMachine.Unit.transform.forward * (StateMachine.ComboIndex > 1 ? attackInfoData.Force : attackInfoData.Force / 2f));
        }
    }
}