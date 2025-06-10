using System;
using Character.Scripts.States.AirState;
using Character.Scripts.States.AttackState;
using Character.Scripts.States.GroundState;
using UnityEngine;
using Utils;

namespace Character.Scripts.States
{
    [Serializable] public class UnitStateMachine : StateMachine
    {
        public Unit Unit { get; }
        
        public Vector2 MovementDirection { get; set; }
        public float MovementSpeed { get; private set; }
        public float RotationDamping { get; private set; }
        public float MovementSpeedModifier { get; set; } = 1f;
        
        public float JumpForce { get; set; }
        public Transform MainCameraTransform { get; set; }
        
        [field: SerializeField] public UnitIdleState IdleState { get; private set; }
        [field: SerializeField] public UnitCrouchState CrouchState { get; private set; }
        [field: SerializeField] public UnitCrouchWalkState CrouchWalkState { get; private set; }
        [field: SerializeField] public UnitWalkState WalkState { get; private set; }
        [field: SerializeField] public UnitRunState RunState { get; private set; }
        [field: SerializeField] public UnitJumpState JumpState { get; private set; }
        [field: SerializeField] public UnitFallState FallState { get; private set; }
        [field: SerializeField] public UnitComboAttackState ComboAttackState { get; private set; }
        
        public bool IsAttacking { get; set; }
        public int ComboIndex { get; set; }
        
        public UnitStateMachine(Unit unit)
        {
            Unit = unit;
            MainCameraTransform = Camera.main?.transform;

            // Registration of states
            IdleState = new UnitIdleState(this);
            CrouchState = new UnitCrouchState(this);
            CrouchWalkState = new UnitCrouchWalkState(this);
            WalkState = new UnitWalkState(this);
            RunState = new UnitRunState(this);
            JumpState = new UnitJumpState(this);
            FallState = new UnitFallState(this);
            ComboAttackState = new UnitComboAttackState(this);

            MovementSpeed = unit.UnitCondition.Speed;
            RotationDamping = unit.UnitCondition.RotationDamping;
        }
    }
}