using Manager.Global;
using UnityEngine;

namespace UI
{
    public enum CurrentScene
    {
        Intro,
        Loading,
        Main,
        Option,
    }
    
    public abstract class BaseUI : MonoBehaviour
    {
        protected UIManager UIManager;

        public virtual void Init(UIManager uiManager)
        {
            UIManager = uiManager;
        }

        protected abstract CurrentScene GetUIState();

        public void SetActive(CurrentScene state)
        {
            gameObject.SetActive(GetUIState() == state);
        }
    }
}