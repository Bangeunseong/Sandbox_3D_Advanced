using System;
using Character;
using Character.Scripts;
using UnityEngine;

namespace Manager.InGame
{
    public class UnitManager : MonoBehaviour
    {
        public Unit currentUnit;
        public static UnitManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            } else{ if(Instance != this) Destroy(gameObject); }
        }
    }
}