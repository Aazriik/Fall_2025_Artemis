using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu: BaseMenu
{
    public Button[] allButtons;

    public override void Initialize(MenuController menuController)
    {
        base.Initialize(menuController);
        state = MenuStates.MainMenu;

        allButtons = GetComponentsInChildren<Button>(true);

        if (allButtons.Length < 0)
        {
            Debug.LogError("MainMenu: No buttons found in children.");
            return;
        }

        foreach (Button btn in allButtons)
        {
            if (btn.name == "StartButton")
                btn.onClick.AddListener(() => SceneManager.LoadScene(1));
            else if (btn.name == "SettingsButton")
                btn.onClick.AddListener(() => JumpTo(MenuStates.SettingsMenu));
            else if (btn.name == "CreditsButton")
                btn.onClick.AddListener(() => JumpTo(MenuStates.CreditsMenu));
            else if (btn.name == "QuitButton")
                btn.onClick.AddListener(QuitGame);
        }
    }
}
