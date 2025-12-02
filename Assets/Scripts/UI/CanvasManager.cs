using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CanvasManager: MonoBehaviour
{
    [Header("Button References")]
    [Header("Main Menu Buttons")]
    public Button mainMenuStartButton;
    public Button mainMenuSettingsButton;

    [Header("Settings Menu Buttons")]
    public Button settingsBackButton;

    [Header("Pause Menu Buttons")]
    public Button pauseResumeButton;
    public Button pauseMainMenuButton;

    [Header("Game Over Buttons")]
    public Button gameOverPlayAgainButton;
    public Button gameOverMainMenuButton;
    public Button gameOverQuitButton;

    [Header("Common Menu Buttons")]
    public Button menuQuitButton;

    [Header("HUD References")]
    public TMP_Text livesText;

    [Header("Menu References")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Main Menu Buttons
        if (mainMenuStartButton)
            mainMenuStartButton.onClick.AddListener(() => SceneManager.LoadScene(1));

        if (mainMenuSettingsButton)
            mainMenuSettingsButton.onClick.AddListener(() => SetMenus(settingsMenu, mainMenu));

        // Settings Menu Buttons
        if (settingsBackButton)
            settingsBackButton.onClick.AddListener(() => SetMenus(mainMenu, settingsMenu));

        // Pause Menu Buttons
        if (pauseResumeButton)
            pauseResumeButton.onClick.AddListener(() => SetMenus(null, pauseMenu));

        if (pauseMainMenuButton)
            pauseMainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));

        // Game Over Buttons
        if (gameOverPlayAgainButton)
            gameOverPlayAgainButton.onClick.AddListener(() => SceneManager.LoadScene(1));

        if (gameOverMainMenuButton)
            gameOverMainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));


        // Quit Game Buttons
        if (gameOverQuitButton)
            gameOverQuitButton.onClick.AddListener(QuitGame);

        if (menuQuitButton)
            menuQuitButton.onClick.AddListener(QuitGame);

        // HUD References
        if (livesText)
        {
            livesText.text = $"Lives: {GameManager.Instance.lives}";
            GameManager.Instance.OnLifeValueChanged += (int newLives) => livesText.text = $"Lives: {newLives}";

            // If Lives reach 0, show Game Over menu
            GameManager.Instance.OnLifeValueChanged += (int newLives) =>
            {
                if (newLives <= 0)
                {
                    SetMenus(gameOverMenu, pauseMenu);
                    PauseGame();
                }
            };
        }

        // Pause Menu Buttons
        if (pauseMainMenuButton)
            pauseMainMenuButton.onClick.AddListener(UnPause);

        // Game Over Buttons
        if (gameOverPlayAgainButton)
            gameOverPlayAgainButton.onClick.AddListener(UnPause);

        if (gameOverMainMenuButton)
            gameOverMainMenuButton.onClick.AddListener(UnPause);

    }

    private void SetMenus(GameObject menuToEnable, GameObject menuToDisable)
    {
        if (menuToEnable)
            menuToEnable.SetActive(true);
        if (menuToDisable)
            menuToDisable.SetActive(false);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void UnPause()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenu.activeSelf)
            {
                SetMenus(null, pauseMenu);
                Time.timeScale = 1;
                return;
            }

            SetMenus(pauseMenu, null);
            PauseGame();
        }

    }
}
