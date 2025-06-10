using System;
using Character.Scripts.Data;
using Character.Scripts.States;
using Manager.Global;
using UnityEngine;
using Utils;

namespace Character.Scripts
{
    public class Unit : MonoBehaviour
    {
        [field: Header("UUID")]
        [field: SerializeField] public string Uuid { get; private set; }
        
        [field: Header("Animations")]
        [field: SerializeField] public AnimationData AnimationData { get; private set; }

        [field: Header("Components")] 
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public UnitCondition UnitCondition { get; private set; }
        [field: SerializeField] public UnitController UnitController { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

        // Fields
        [field: SerializeField] private UnitStateMachine stateMachine;
        
        private void Awake()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!UnitCondition) UnitCondition = gameObject.GetComponent_Helper<UnitCondition>();
            if (!UnitController) UnitController = gameObject.GetComponent_Helper<UnitController>();
            if (!ForceReceiver) ForceReceiver = gameObject.GetComponent_Helper<ForceReceiver>();
            
            AnimationData.Initialize();
            
        }

        private void Reset()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!UnitCondition) UnitCondition = gameObject.GetComponent_Helper<UnitCondition>();
            if (!UnitController) UnitController = gameObject.GetComponent_Helper<UnitController>();
            if (!ForceReceiver) ForceReceiver = gameObject.GetComponent_Helper<ForceReceiver>();

            AnimationData.Initialize();
        }

        private void Start()
        {
            stateMachine = new UnitStateMachine(this);
            
            Uuid = GameManager.Instance.SaveData != null ? GameManager.Instance.SaveData.Uuid : Guid.NewGuid().ToString();
            Cursor.lockState = CursorLockMode.Locked;
            stateMachine.ChangeState(stateMachine.IdleState);
            UnitCondition.OnDeath += OnDeath;
        }
        
        private void FixedUpdate()
        {
            stateMachine.PhysicsUpdate();
        }

        private void Update()
        {
            stateMachine.HandleInput();
            stateMachine.Update();
        }

        private void OnDeath()
        {
            Animator.SetTrigger(AnimationData.DeathParameterHash);
            enabled = false;
        }
    }
}