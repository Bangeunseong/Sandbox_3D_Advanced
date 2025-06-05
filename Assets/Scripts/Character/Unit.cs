using System;
using Character.Data;
using UnityEngine;
using Utils;

namespace Character
{
    public class Unit : MonoBehaviour
    {
        [field: Header("UnitStat Data")]
        [field: SerializeField] public UnitStat Stat { get; private set; }
        
        [field: Header("Animations")]
        [field: SerializeField] public AnimationData AnimationData { get; private set; }

        [field: Header("Components")] 
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public UnitCondition UnitCondition { get; private set; }
        [field: SerializeField] public UnitEquipment UnitEquipment { get; private set; }

        private void Awake()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!UnitCondition) UnitCondition = gameObject.GetComponent_Helper<UnitCondition>();
            if (!UnitEquipment) UnitEquipment = gameObject.GetComponent_Helper<UnitEquipment>();
            
            AnimationData.Initialize();
        }
    }
}