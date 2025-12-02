using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Button Refs")]
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backSettingsButton;
    public Button resumeButton;
    public Button returnToMenuButton;

    [Header("HUD Refs")]
    public TMP_Text livesText;

    [Header("Menu Refs")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(() => SceneManager.LoadScene(1));

        if (settingsButton)
            settingsButton.onClick.AddListener(() => SetMenu(settingsMenu, mainMenu));

        if (backSettingsButton)
            backSettingsButton.onClick.AddListener(() => SetMenu(mainMenu, settingsMenu));


        if (resumeButton)
            resumeButton.onClick.AddListener(() => SetMenu(null, pauseMenu));

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));



        if (quitButton)
            quitButton.onClick.AddListener(QuitGame);

    }

    private void SetMenu(GameObject menuToEnable, GameObject menuToDisable)
    {
        if (menuToEnable) menuToEnable.SetActive(true);
        if (menuToDisable) menuToDisable.SetActive(false);
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        if (livesText)
        {
            int currentLives = GameManager.Instance.lives;
            livesText.text = $"Lives: {currentLives}";
        }

        if (!pauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenu.activeSelf)
            {
                SetMenu(null, pauseMenu);
                Time.timeScale = 1f;
                return;
            }
            else
            {
                SetMenu(pauseMenu, null);
                Time.timeScale = 0f;
            }
        }
    }
}
