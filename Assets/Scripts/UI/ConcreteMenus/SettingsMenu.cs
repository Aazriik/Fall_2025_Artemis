using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu: BaseMenu
{
    public Button[] allButtons;

    public override void Initialize(MenuController menuController)
    {
        base.Initialize(menuController);
        state = MenuStates.SettingsMenu;


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
            else if (btn.name == "CreditsButton")
                btn.onClick.AddListener(() => JumpTo(MenuStates.CreditsMenu));
        }
    }
}
