using System;
using Character.Scripts.Data;
using UnityEngine;
using Utils;

namespace Character.Scripts
{
    public class Unit : MonoBehaviour
    {
        [field: Header("Animations")]
        [field: SerializeField] public AnimationData AnimationData { get; private set; }

        [field: Header("Components")] 
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public UnitCondition UnitCondition { get; private set; }

        private void Awake()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!UnitCondition) UnitCondition = gameObject.GetComponent_Helper<UnitCondition>();
            
            AnimationData.Initialize();
        }

        private void Reset()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!UnitCondition) UnitCondition = gameObject.GetComponent_Helper<UnitCondition>();
            
            AnimationData.Initialize();
        }
    }
}