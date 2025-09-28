using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private EGameState gameState;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SetGameState(EGameState.MENU);
    }

    public void SetGameState(EGameState gameState)
    {
        this.gameState = gameState;

        // Pegamos todos os Scripts que Implementam essa Interface "IGameStateListener"
        IEnumerable<IGameStateListener> gameStateListeners
            = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IGameStateListener>();

        foreach (var dependecy in gameStateListeners)
        {
            dependecy.GameStateChangedCallBack(gameState);
        }
    }

    public bool IsGameState()
    {
        return gameState == EGameState.GAME;
    }

    public void StartGameButtonCallBack()
    {
        SetGameState(EGameState.GAME);
    }

    public void NextButtonCallBack()
    {
        SceneManager.LoadScene(0);
    }

    public void RetryButtonCallBack()
    {
        SceneManager.LoadScene(0);

    }
}
