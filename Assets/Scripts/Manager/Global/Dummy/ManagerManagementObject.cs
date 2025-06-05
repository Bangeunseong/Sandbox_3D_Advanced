using UnityEngine;

namespace Manager.Global.Dummy
{
    public class ManagerManagementObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}