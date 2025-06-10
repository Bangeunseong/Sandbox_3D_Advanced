using Character.Scripts.Data;
using Manager.Global;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Character.Scripts.States
{
    public class UnitBaseState : MonoBehaviour, IState
    {
        protected readonly UnitStateMachine StateMachine;
        protected readonly UnitCondition UnitCondition;

        public UnitBaseState(UnitStateMachine stateMachine)
        {
            StateMachine = stateMachine;
            UnitCondition = stateMachine.Unit.UnitCondition;
        }
        
        
        public virtual void Enter()
        {
            AddInputActionCallbacks();
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void Update()
        {
            if (!UnitCondition.IsPlayerHasControl) return;
            Move();
        }

        public virtual void PhysicsUpdate()
        {
            
        }

        public virtual void Exit()
        {
            RemoveInputActionCallbacks();
        }

        
        protected void StartAnimation(int animatorHash)
        {
            StateMachine.Unit.Animator.SetBool(animatorHash, true);
        }

        protected void StopAnimation(int animatorHash)
        {
            StateMachine.Unit.Animator.SetBool(animatorHash, false);
        }
        
        private void ReadMovementInput()
        {
            if (!UnitCondition.IsPlayerHasControl) { StateMachine.MovementDirection = Vector2.zero; return; }
            StateMachine.MovementDirection =
                StateMachine.Unit.UnitController.PlayerActions.Move.ReadValue<Vector2>();
        }
        
        private void Move()
        {
            var movementDirection = GetMovementDirection();
            Move(movementDirection);
            Rotate(movementDirection);
        }

        private Vector3 GetMovementDirection()
        {
            var forward = StateMachine.MainCameraTransform.forward;
            var right = StateMachine.MainCameraTransform.right;


            forward.y = 0;
            right.y = 0;
            
            forward.Normalize();
            right.Normalize();
            
            return forward * StateMachine.MovementDirection.y + right * StateMachine.MovementDirection.x;
        }

        private void Move(Vector3 direction)
        {
            var movementSpeed = GetMovementSpeed();
            StateMachine.Unit.CharacterController.Move((direction * movementSpeed + StateMachine.Unit.ForceReceiver.ExtraMovement) * Time.deltaTime);
        }
        
        private float GetMovementSpeed()
        {
            var movementSpeed = StateMachine.MovementSpeed * StateMachine.MovementSpeedModifier;
            return movementSpeed;
        }

        private void Rotate(Vector3 direction)
        {
            if (direction == Vector3.zero) return;
            
            var unitTransform = StateMachine.Unit.transform;
            var targetRotation = Quaternion.LookRotation(direction);
            unitTransform.rotation = Quaternion.Slerp(unitTransform.rotation, targetRotation, StateMachine.RotationDamping * Time.deltaTime);
        }
        
        protected void ForceMove()
        {
            StateMachine.Unit.CharacterController.Move(StateMachine.Unit.ForceReceiver.ExtraMovement *
                                                         Time.deltaTime);
        }

        protected float GetNormalizedTime(Animator animator, string tag)
        {
            var currentInfo = animator.GetCurrentAnimatorStateInfo(0);
            var nextInfo = animator.GetNextAnimatorStateInfo(0);
            
            if (animator.IsInTransition(0) && nextInfo.IsTag(tag)) { return nextInfo.normalizedTime; }
            return !animator.IsInTransition(0) && currentInfo.IsTag(tag) ? currentInfo.normalizedTime : 0f;
        }

        protected virtual void AddInputActionCallbacks()
        {
            var unitController = StateMachine.Unit.UnitController;
            unitController.PlayerActions.Move.canceled += OnMoveCanceled;
            unitController.PlayerActions.Sprint.started += OnRunStarted;
            unitController.PlayerActions.Jump.started += OnJumpStarted;
            unitController.PlayerActions.Crouch.started += OnCrouchStarted;
            unitController.PlayerActions.Menu.started += OnMenuStarted;
            unitController.PlayerActions.Attack.performed += OnAttackPerformed;
            unitController.PlayerActions.Attack.canceled += OnAttackCanceled;
        }
        
        protected virtual void RemoveInputActionCallbacks()
        {
            var unitController = StateMachine.Unit.UnitController;
            unitController.PlayerActions.Move.canceled -= OnMoveCanceled;
            unitController.PlayerActions.Sprint.started -= OnRunStarted;
            unitController.PlayerActions.Jump.started -= OnJumpStarted;
            unitController.PlayerActions.Crouch.started -= OnCrouchStarted;
            unitController.PlayerActions.Menu.started -= OnMenuStarted;
            unitController.PlayerActions.Attack.performed -= OnAttackPerformed;
            unitController.PlayerActions.Attack.canceled -= OnAttackCanceled;
        }

        protected virtual void OnMoveCanceled(InputAction.CallbackContext context)
        {
            
        }

        protected virtual void OnCrouchStarted(InputAction.CallbackContext context)
        {
            if (!UnitCondition.IsPlayerHasControl) return;
        }

        protected virtual void OnRunStarted(InputAction.CallbackContext context)
        {
            if (!UnitCondition.IsPlayerHasControl) return;
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            if (!UnitCondition.IsPlayerHasControl) return;
        }

        protected virtual void OnAttackPerformed(InputAction.CallbackContext context)
        {
            if (!UnitCondition.IsPlayerHasControl) return;
            StateMachine.IsAttacking = true;
        }

        protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
        {
            StateMachine.IsAttacking = false;
        }

        protected virtual void OnMenuStarted(InputAction.CallbackContext context)
        {
            UnitCondition.IsPlayerHasControl = !UnitCondition.IsPlayerHasControl;
            Cursor.lockState = UnitCondition.IsPlayerHasControl ? CursorLockMode.Locked : CursorLockMode.None;
            UIManager.Instance.MainUI.ToggleMainMenuUI();
        }
    }
}