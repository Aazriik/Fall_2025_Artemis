using UnityEngine;

public abstract class BaseMenu: MonoBehaviour
{
    [HideInInspector] public MenuStates state;
    protected MenuController context;

    public virtual void Initialize(MenuController menuController)
    {
        context = menuController;
    }

    public virtual void Enter()
    {
    }
    public virtual void Exit()
    {
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void JumpBack() => context.JumpBack();

    public void JumpTo(MenuStates newState) => context.JumpTo(newState);
}

public enum MenuStates
{
    MainMenu,
    SettingsMenu,
    CreditsMenu
}
