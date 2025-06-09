using UnityEngine;
using Utils;

namespace Character.Scripts
{
    public class ForceReceiver : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterController characterController;

        [Header("Drag Settings")] 
        [SerializeField] private float drag = 0.3f;
        
        private float verticalVelocity;
        private Vector3 dampingVelocity;
        private Vector3 impact;
        
        public Vector3 ExtraMovement => impact + Vector3.up * verticalVelocity;

        private void Awake()
        {
            if (!characterController) characterController = gameObject.GetComponent_Helper<CharacterController>();
        }
        
        public void Reset()
        {
            verticalVelocity = 0f;
            impact = Vector3.zero;
        }

        private void Update()
        {
            if (characterController.isGrounded) verticalVelocity = Physics.gravity.y * Time.deltaTime;
            else verticalVelocity += Physics.gravity.y * Time.deltaTime;
            
            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
        }

        public void Jump(float jumpForce)
        {
            verticalVelocity += jumpForce;
        }

        public void AddForce(Vector3 force)
        {
            impact += force;
        }
    }
}