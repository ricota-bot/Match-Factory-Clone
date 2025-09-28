using System;
using UnityEngine;

public class LevelManager : MonoBehaviour, IGameStateListener
{
    // Responsavel por Spawnar os Leveis -- Irá armazenar todos os Leveis aqui
    // Save the Player Index
    [Header("Data")]
    [SerializeField] private Level[] levels;
    private const string levelKey = "LevelReached";
    private int levelIndex;

    [Header("Settings")]
    private Level currentLevel;

    [Header("Actions")]
    public static Action<Level> OnLevelSpawned;

    private void Awake()
    {
        LoadData();
    }

    private void SpawnLevel()
    {
        // Verificamos se não temos nenhum level spawnado Antes... Limpamos o Filho GetChild(0)
        transform.Clear();

        int validatedLevelIndex = levelIndex % levels.Length;

        currentLevel = Instantiate(levels[validatedLevelIndex], transform);

        // Basicamente conhecemos o metodo que foi Spawnado e passamos para outras Classes poderem ver tambem
        OnLevelSpawned?.Invoke(currentLevel); // Passamos o level que foi spawnado, como isso conseguimos acessar o GetGoal ou coisas do tipo
    }

    private void LoadData()
    {
        levelIndex = PlayerPrefs.GetInt(levelKey, 0); // Default value is 0
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(levelKey, levelIndex);
    }

    public void GameStateChangedCallBack(EGameState gameState)
    {
        if (gameState == EGameState.GAME)
            SpawnLevel();
        else if (gameState == EGameState.LEVELCOMPLETE)
        {
            levelIndex++;
            SaveData();
        }

    }
}
