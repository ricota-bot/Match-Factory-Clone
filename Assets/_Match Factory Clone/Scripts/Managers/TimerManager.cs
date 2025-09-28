using System;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI timerText;
    private int currentTimer;

    private void Awake()
    {
        LevelManager.OnLevelSpawned += OnLevelSpawnedCallback;
    }

    private void OnDestroy()
    {

        LevelManager.OnLevelSpawned -= OnLevelSpawnedCallback;
    }

    private void OnLevelSpawnedCallback(Level level)
    {
        currentTimer = level.DurationInSeconds;
        timerText.text = SecondsToString(currentTimer);

        StartTimer();
    }

    private void StartTimer()
    {
        InvokeRepeating("UpdateTimer", 0, 1);
    }

    private void PauseTimer()
    {
        CancelInvoke("UpdateTimer");
    }

    private void UpdateTimer()
    {
        currentTimer--;
        timerText.text = SecondsToString(currentTimer);

        if (currentTimer <= 0)
            TimerFinished();
    }

    private void TimerFinished()
    {
        GameManager.instance.SetGameState(EGameState.GAMEOVER);
        PauseTimer();
    }

    private string SecondsToString(int seconds)
    {
        return TimeSpan.FromSeconds(seconds).ToString().Substring(3);
    }

    public void GameStateChangedCallBack(EGameState gameState)
    {
        if (gameState == EGameState.LEVELCOMPLETE || gameState == EGameState.GAMEOVER)
            PauseTimer();
    }
}
