using UnityEngine;

namespace UI
{
    public class IntroUI : BaseUI
    {
        protected override CurrentScene GetUIState()
        {
            return CurrentScene.Intro;
        }
    }
}