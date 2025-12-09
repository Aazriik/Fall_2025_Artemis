using System.Collections.Generic;
using UnityEngine;

public class MenuController: MonoBehaviour
{
    public BaseMenu[] allMenus;

    public MenuStates initState = MenuStates.MainMenu;

    public BaseMenu currentMenu => _currentMenu;
    private BaseMenu _currentMenu;

    private Dictionary<MenuStates, BaseMenu> menuDictionary = new Dictionary<MenuStates, BaseMenu>();
    private Stack<MenuStates> menuStack = new Stack<MenuStates>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (allMenus.Length == 0)
        {
            allMenus = GetComponentsInChildren<BaseMenu>(true);
        }

        foreach (BaseMenu menu in allMenus)
        {
            if (menu == null)
                continue;

            menu.Initialize(this);

            if (menuDictionary.ContainsKey(menu.state))
            {
                Debug.LogError($"MenuController: Duplicate menu state detected: {menu.state}. Each menu must have a unique state.");
                continue;
            }

            menuDictionary.Add(menu.state, menu);
        }

        JumpTo(initState);
    }

    public void JumpBack()
    {
        //Defensive check - we can probably log something here - but this should actually never happen
        if (menuStack.Count <= 0)
            return;

        menuStack.Pop();
        JumpTo(menuStack.Peek(), true);
    }

    //Jumps to a new menu state - if fromJumpBack is true, we do not modify the menu stack
    public void JumpTo(MenuStates newState, bool fromJumpBack = false)
    {
        //Defensive check
        if (!menuDictionary.ContainsKey(newState))
        {
            Debug.LogError($"MenuController: Attempted to jump to unknown menu state: {newState}");
            return;
        }

        if (_currentMenu == menuDictionary[newState])
        {
            Debug.LogWarning($"MenuController: Already in menu state: {newState}");
            return;
        }

        if (_currentMenu != null)
        {
            _currentMenu.Exit();
            _currentMenu.gameObject.SetActive(false);
        }

        _currentMenu = menuDictionary[newState];
        _currentMenu.gameObject.SetActive(true);
        _currentMenu.Enter();

        if (!fromJumpBack)
        {
            if (menuStack.Count > 0 && menuStack.Contains(newState))
            {
                List<MenuStates> oldStates = new List<MenuStates>();

                while (menuStack.Peek() != newState)
                {
                    oldStates.Add(menuStack.Pop());
                }

                //pop the new state off the stack as well as we will need to read it at the end
                menuStack.Pop();

                //read all the old states back onto the stack in reverse order
                for (int i = oldStates.Count - 1; i >= 0; i--)
                {
                    menuStack.Push(oldStates[i]);
                }

            }
            menuStack.Push(newState);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
