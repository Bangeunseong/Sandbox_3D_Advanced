using System;
using Character.Scripts.Data;
using Character.Scripts.Interface;
using JetBrains.Annotations;
using Manager.Global;
using Manager.Global.DTO;
using UnityEngine;

namespace Character.Scripts
{
    public class UnitCondition : MonoBehaviour, IDamagable
    {
        [field: Header("UnitStat Data")]
        [field: SerializeField] public UnitData UnitData { get; private set; }

        [field: Header("Unit Condition Settings")]
        [field: SerializeField] public int Level { get; private set; } = 1;
        [field: SerializeField] public int Experience { get; private set; }
        [field: SerializeField] public float LifeSpan { get; private set; }
        [field: SerializeField] public float MaxLifeSpan { get; private set; }
        [field: SerializeField] public float ComputeForce { get; private set; }
        [field: SerializeField] public float ComputeSpeed { get; private set; }
        [field: SerializeField] public float Accuracy { get; private set; }

        [field: Header("Unit Physics Settings")]
        [field: SerializeField] public float ComputeRate { get; private set; }       // AttackRate
        [field: SerializeField] public float Speed { get; private set; }             // Movement Speed
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float WalkSpeedModifier { get; private set; }
        [field: SerializeField] public float SprintSpeedModifier { get; private set; }
        [field: SerializeField] public float RotationDamping { get; private set; }   // Rotation Speed
        [field: SerializeField] public bool IsDead { get; private set; }
        
        private UIManager uiManager;
        
        [CanBeNull] public event Action OnDamage, OnDeath;
        
        private void Start()
        {
            uiManager = UIManager.Instance;
            UnitData = ResourceManager.Instance.GetResourceByName<UnitData>(ResourceManager.CharacterPrefix +
                                                                            GameManager.Instance.SelectedCharacter);
            InitializeStat(GameManager.Instance.SaveData);
        }

        private void InitializeStat(SaveData data)
        {
            if (data == null)
            {
                MaxLifeSpan = LifeSpan = UnitData.Stat.BaseLifeSpan;
                ComputeForce = UnitData.Stat.BaseComputeForce;
                ComputeSpeed = UnitData.Stat.BaseComputeSpeed;
                Accuracy = UnitData.Stat.BaseAccuracy;
                
                uiManager.MainUI.Initialize_StatusUI(UnitData.Stat);
            }
            else
            {
                MaxLifeSpan = data.MaxLifeSpan;
                LifeSpan = data.LifeSpan;
                ComputeForce = data.ComputeForce;
                ComputeSpeed = data.ComputeSpeed;
                Accuracy = data.Accuracy;
                foreach (StatType type in Enum.GetValues(typeof(StatType)))
                {
                    var value = type switch
                    {
                        StatType.LifeSpan => MaxLifeSpan,
                        StatType.ComputeForce => ComputeForce,
                        StatType.ComputeSpeed => ComputeSpeed,
                        StatType.Accuracy => Accuracy,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    uiManager.MainUI.UpdateStatValueByType(type, value);
                }
            }

            ComputeRate = UnitData.Stat.BaseComputeRate;
            Speed = UnitData.Stat.BaseSpeed;
            JumpForce = UnitData.Stat.BaseJumpForce;
            WalkSpeedModifier = UnitData.Stat.WalkSpeedModifier;
            SprintSpeedModifier = UnitData.Stat.SprintSpeedModifier;
            RotationDamping = UnitData.Stat.BaseRotationDamping;
        }

        public void UpdateValueAndExtraByType(StatType type, float value)
        {
            var originExtraValue = uiManager.MainUI.StatusSlots[type].Extra;
            
            switch (type)
            {
                case StatType.LifeSpan: 
                    MaxLifeSpan += value; LifeSpan = Mathf.Min(LifeSpan, MaxLifeSpan);
                    uiManager.MainUI.StatusSlots[type].UpdateValue(MaxLifeSpan);
                    break;
                case StatType.ComputeForce: ComputeForce += value; 
                    uiManager.MainUI.StatusSlots[type].UpdateValue(ComputeForce);
                    break;
                case StatType.ComputeSpeed: ComputeSpeed += value; 
                    uiManager.MainUI.StatusSlots[type].UpdateValue(ComputeSpeed);
                    break;
                case StatType.Accuracy: Accuracy += value; 
                    uiManager.MainUI.StatusSlots[type].UpdateValue(Accuracy);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            uiManager.MainUI.StatusSlots[type].UpdateExtra(originExtraValue + value);
        }

        public void OnTakeDamage(int damage)
        {
            if (IsDead) return;
            LifeSpan -= damage;
            // Update GameUI LifeSpan
            OnDamage?.Invoke();

            if (LifeSpan <= 0) { OnDead(); }
        }

        private void OnDead()
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
    }
}