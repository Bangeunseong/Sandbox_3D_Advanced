using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Scripts.Data
{
    public enum StatType
    {
        LifeSpan,
        ComputeForce,
        ComputeSpeed,
        Accuracy,
    }
    
    [Serializable] public class UnitStat
    {
        [field: Header("Default Stat. Settings")]
        [field: SerializeField] public string Name { get; private set; } = "Chad";
        [field: SerializeField] [field: Range(0f, 2000f)] public float BaseLifeSpan { get; private set; } = 100f;
        [field: SerializeField] [field: Range(0f, 1500f)] public float BaseComputeForce { get; private set; } = 25f; 
        [field: SerializeField] [field: Range(0f, 1500f)] public float BaseComputeSpeed { get; private set; } = 15f; 
        [field: SerializeField] [field: Range(0f, 100f)] public float BaseAccuracy { get; private set; } = 3f;

        [field: SerializeField] [field: Range(0.5f, 15f)] public float BaseComputeRate { get; private set; } = 1f;
        [field: SerializeField] [field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
        [field: SerializeField] [field: Range(0f, 25f)] public float BaseRotationDamping { get; private set; } = 3f;
    }
    
    [Serializable] public class AttackInfoData
    {
        [field: Header("Name")]
        [field: SerializeField] public string AttackName { get; private set; }
        
        [field: Header("Combo Settings")]
        [field: SerializeField] public int ComboStateIndex { get; private set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float ComboTransitionTime { get; private set; }
        
        [field: Header("KnockBack Settings")]
        [field: SerializeField] [field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }
        [field: SerializeField] [field: Range(-10f, 10f)] public float Force { get; private set; }
        
        [field: Header("Damage")]
        [field: SerializeField] public int Damage;
        
        [field: Header("Damage Timing Settings")]
        [field: SerializeField] [field: Range(0f, 1f)] public float DealingStartTransitionTime { get; private set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float DealingEndTransitionTime { get; private set; }
    }
    
    [Serializable] public class UnitAttackData
    {
        [field: Header("Attack Data")]
        [field: SerializeField] public List<AttackInfoData> AttackInfoList { get; private set; }

        public int GetAttackInfoCount => AttackInfoList.Count;
        public AttackInfoData GetAttackInfoData(int index) { return AttackInfoList[index]; }
    }
    
    [CreateAssetMenu(fileName = "New UnitData", menuName = "ScriptableObjects/Unit/Create New UnitData", order = 0)]
    public class UnitData : ScriptableObject
    {
        [field: SerializeField] public UnitStat Stat { get; private set; }
        [field: SerializeField] public UnitAttackData AttackData { get; private set; }
    }
}