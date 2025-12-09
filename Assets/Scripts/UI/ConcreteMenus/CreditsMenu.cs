using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsMenu: BaseMenu
{
    public Button[] allButtons;

    public override void Initialize(MenuController menuController)
    {
        base.Initialize(menuController);
        state = MenuStates.CreditsMenu;

        allButtons = GetComponentsInChildren<Button>(true);

        if (allButtons.Length < 0)
        {
            Debug.LogError("MainMenu: No buttons found in children.");
            return;
        }

        foreach (Button btn in allButtons)
        {
            if (btn.name == "BackButton")
                btn.onClick.AddListener(() => JumpBack());
            else if (btn.name == "SettingsButton")
                btn.onClick.AddListener(() => JumpTo(MenuStates.SettingsMenu));
        }
    }
}
