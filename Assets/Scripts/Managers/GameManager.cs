using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)] //Ensures that this script runs before others
public class GameManager: MonoBehaviour
{
    //Signature that defines the delegate type - delegates are like function pointers
    public delegate void PlayerInstanceDelegate(PlayerController playerInstance);
    public event PlayerInstanceDelegate OnPlayerSpawned;

    // Singleton is a design pattern that restricts the instantiation of a class to one "single" instance.
    #region Singleton Pattern
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }
    #endregion

    #region Lives Management
    private int _lives = 5;
    public int maxLives = 10;
    public int lives
    {
        get => _lives;
        set
        {
            if (value < 0)
            {
                GameOver();
                return;
            }

            if (lives > value)
            {
                Respawn();
                _lives = value;
            }

            if (value > maxLives)
            {
                _lives = maxLives;
            }

            Debug.Log($"Life value has changed to {_lives}");
        }
    }
    private void GameOver()
    {
        Debug.Log("GameOver!");
    }
    private void Respawn()
    {
        playerInstance.transform.position = currentCheckpoint;
    }
    #endregion

    #region Player Controller Information
    [SerializeField] private PlayerController playerPrefab;
    private PlayerController _playerInstance;
    public PlayerController playerInstance => _playerInstance;
    private Vector3 currentCheckpoint;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            string sceneToLoad = (currentSceneName == "Game") ? "Title" : "Game";

            SceneManager.LoadScene(sceneToLoad);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            lives--;
        }
    }

    public void InstantiatePlayer(Vector3 spawnPos)
    {
        _playerInstance = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        currentCheckpoint = spawnPos;

        OnPlayerSpawned?.Invoke(_playerInstance);

        //if (OnPlayerSpawned != null)
        //{
        //    OnPlayerSpawned.Invoke(_playerInstance);
        //}
    }

    public void UpdateCheckpoint(Vector3 newCheckpoint) => currentCheckpoint = newCheckpoint;
}

