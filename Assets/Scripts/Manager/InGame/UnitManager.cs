using System;
using Character;
using Character.Scripts;
using UnityEngine;

namespace Manager.InGame
{
    public class UnitManager : MonoBehaviour
    {
        [field: Header("Unit")]
        [field: SerializeField] public Unit CurrentUnit { get; private set; }
        
        public static UnitManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            } else{ if(Instance != this) Destroy(gameObject); }

            if (!CurrentUnit) CurrentUnit = FindObjectOfType<Unit>();
        }

        public Unit FindUnitWithUuid(string uuid)
        {
            return CurrentUnit.Uuid.Equals(uuid) ? CurrentUnit : null;
        }
    }
}