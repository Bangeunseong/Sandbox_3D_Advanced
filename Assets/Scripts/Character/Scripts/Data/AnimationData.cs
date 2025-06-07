using System;
using UnityEngine;

namespace Character.Scripts.Data
{
    [Serializable] public class AnimationData
    {
        [Header("Animation Parameters on ground")]
        [SerializeField] private string groundParameterName = "@Ground";
        [SerializeField] private string idleParameterName = "Idle";
        [SerializeField] private string runParameterName = "Run";

        [Header("Animation Parameters on attack")] 
        [SerializeField] private string attackParameterName = "@Attack";
        [SerializeField] private string comboAttackParameterName = "ComboAttack";
        [SerializeField] private string comboAttackIndex = "Combo";
        
        [Header("Animation Parameter on death")]
        [SerializeField] private string deathParameterName = "Dead";
        
        // Properties of parameter hash
        public int GroundParameterHash { get; private set; }
        public int IdleParameterHash { get; private set; }
        public int RunParameterHash { get; private set; }
        public int AttackParameterHash { get; private set; }
        public int ComboAttackParameterHash { get; private set; }
        public int ComboAttackIndexHash { get; private set; }
        public int DeathParameterHash { get; private set; }
        
        public void Initialize()
        {
            GroundParameterHash = Animator.StringToHash(groundParameterName);
            IdleParameterHash = Animator.StringToHash(idleParameterName);
            RunParameterHash = Animator.StringToHash(runParameterName);

            AttackParameterHash = Animator.StringToHash(attackParameterName);
            ComboAttackParameterHash = Animator.StringToHash(comboAttackParameterName);
            ComboAttackIndexHash = Animator.StringToHash(comboAttackIndex);
            
            DeathParameterHash = Animator.StringToHash(deathParameterName);
        }
    }
}