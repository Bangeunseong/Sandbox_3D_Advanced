using UnityEngine;

namespace Utils
{
    public static class Helper
    {
        public static T GetComponent_Helper<T>(this GameObject go) where T : Component
        {
            if(!go) { Debug.LogError("GameObject is null!"); return null; }

            if (go.TryGetComponent(out T component)) return component;
            Debug.LogError($"{typeof(T)} is not found in {go.name}!"); 
            return null;
        }

        public static T GetComponentInChildren_Helper<T>(this GameObject go, bool includeInActive = false) where T : Component
        {
            if(!go) { Debug.LogError("GameObject is null!"); return null; }
            var component = go.GetComponentInChildren<T>(includeInActive);
            if(component) return component;
            Debug.LogError($"{typeof(T)} is not found in {go.name} and children of {go.name}!"); 
            return null;
        }

        public static T GetComponentInParent_Helper<T>(this GameObject go, bool includeInActive = false) where T : Component
        {
            if(!go) { Debug.LogError("GameObject is null!"); return null; }
            var component = go.GetComponentInParent<T>(includeInActive);
            if(component) return component;
            Debug.LogError($"{typeof(T)} is not found in {go.name} and parents of {go.name}!"); 
            return null;
        }

        public static T FindObjectAndGetComponentInChildren_Helper<T>(this GameObject go, string name, bool includeInActive = false)
            where T : Component
        {
            if(!go) { Debug.LogError("GameObject is null!"); return null; }

            var components = go.GetComponentsInChildren<T>(includeInActive);
            foreach (var component in components)
            {
                if (component.gameObject.name.Equals(name)) return component;
            }
            Debug.LogError($"{name} Object or {typeof(T)} in {name} is not found in {go.name} and children of {go.name}!");
            return null;
        }
    }
}
