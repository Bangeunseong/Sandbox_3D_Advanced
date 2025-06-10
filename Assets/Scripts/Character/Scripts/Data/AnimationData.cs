using System;
using UnityEngine;

namespace Character.Scripts.Data
{
    [Serializable] public class AnimationData
    {
        [Header("Animation Parameters on ground")]
        [SerializeField] private string groundParameterName = "@Ground";
        [SerializeField] private string idleParameterName = "Idle";
        [SerializeField] private string crouchParameterName = "Crouch";
        [SerializeField] private string crouchWalkParameterName = "CrouchWalk";
        [SerializeField] private string walkParameterName = "Walk";
        [SerializeField] private string runParameterName = "Run";
        [SerializeField] private string danceParameterName = "Dance";
        
        [Header("Animation Parameters on air")]
        [SerializeField] private string airParameterName = "@Air";
        [SerializeField] private string jumpParameterName = "Jump";
        [SerializeField] private string fallParameterName = "Fall";
        
        [Header("Animation Parameters on attack")] 
        [SerializeField] private string attackParameterName = "@Attack";
        [SerializeField] private string comboAttackParameterName = "ComboAttack";
        [SerializeField] private string comboAttackIndex = "Combo";
        
        [Header("Animation Parameter on death")]
        [SerializeField] private string deathParameterName = "Dead";
        
        // Properties of parameter hash
        public int GroundParameterHash { get; private set; }
        public int IdleParameterHash { get; private set; }
        public int CrouchParameterHash { get; private set; }
        public int CrouchWalkParameterHash { get; private set; }
        public int WalkParameterHash { get; private set; }
        public int RunParameterHash { get; private set; }
        public int DanceParameterHash { get; private set; }
        public int AirParameterHash { get; private set; }
        public int JumpParameterHash { get; private set; }
        public int FallParameterHash { get; private set; }
        public int AttackParameterHash { get; private set; }
        public int ComboAttackParameterHash { get; private set; }
        public int ComboAttackIndexHash { get; private set; }
        public int DeathParameterHash { get; private set; }
        
        public void Initialize()
        {
            GroundParameterHash = Animator.StringToHash(groundParameterName);
            IdleParameterHash = Animator.StringToHash(idleParameterName);
            CrouchParameterHash = Animator.StringToHash(crouchParameterName);
            CrouchWalkParameterHash = Animator.StringToHash(crouchWalkParameterName);
            WalkParameterHash = Animator.StringToHash(walkParameterName);
            RunParameterHash = Animator.StringToHash(runParameterName);
            DanceParameterHash = Animator.StringToHash(danceParameterName);

            AirParameterHash = Animator.StringToHash(airParameterName);
            JumpParameterHash = Animator.StringToHash(jumpParameterName);
            FallParameterHash = Animator.StringToHash(fallParameterName);
            
            AttackParameterHash = Animator.StringToHash(attackParameterName);
            ComboAttackParameterHash = Animator.StringToHash(comboAttackParameterName);
            ComboAttackIndexHash = Animator.StringToHash(comboAttackIndex);
            
            DeathParameterHash = Animator.StringToHash(deathParameterName);
        }
    }
}