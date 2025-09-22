using System;
using UnityEngine;

public class GoalManager : MonoBehaviour
{

    private void Awake()
    {
        LevelManager.OnLevelSpawned += OnLevelSpawnedCallBack;
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelSpawned -= OnLevelSpawnedCallBack;

    }

    private void OnLevelSpawnedCallBack(Level level)
    {

    }
}
