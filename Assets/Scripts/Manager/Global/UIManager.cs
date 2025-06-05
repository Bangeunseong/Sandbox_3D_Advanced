using System;
using UnityEngine;

namespace Manager.Global
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }


        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            } else{if(Instance != this) Destroy(gameObject);}
        }
    }
}