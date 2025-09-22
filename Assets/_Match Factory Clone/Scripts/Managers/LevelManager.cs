using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Responsavel por Spawnar os Leveis -- Ir� armazenar todos os Leveis aqui
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

    private void Start()
    {
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        // Verificamos se n�o temos nenhum level spawnado Antes... Limpamos o Filho GetChild(0)
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
}
