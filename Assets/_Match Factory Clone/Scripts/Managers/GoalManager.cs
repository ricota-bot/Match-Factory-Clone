using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [Header("Elemets")]
    [SerializeField] private Transform goalsCardParent;
    [SerializeField] private GoalCard goalCardPrefab;

    [Header("Data")]
    private ItemLevelData[] goals;
    private List<GoalCard> goalsCardList = new List<GoalCard>();

    private void Awake()
    {
        LevelManager.OnLevelSpawned += OnLevelSpawnedCallBack; // Conhecendo o Level que estamos
        ItemSpotsManager.OnItemPickedUp += OnItemPickedUpCallBack;
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelSpawned -= OnLevelSpawnedCallBack; // Conhecendo o Level que estamos
        ItemSpotsManager.OnItemPickedUp -= OnItemPickedUpCallBack;


    }

    private void OnLevelSpawnedCallBack(Level level)
    {
        goals = level.GetGoals();

        GenerateGoalsCards();
    }

    private void GenerateGoalsCards()
    {
        for (int i = 0; i < goals.Length; i++)
        {
            GenerateGoalCard(goals[i]);
        }
    }

    private void GenerateGoalCard(ItemLevelData goal)
    {
        GoalCard cardInstance = Instantiate(goalCardPrefab, goalsCardParent);

        cardInstance.Configure(goal.amount);
        goalsCardList.Add(cardInstance);
    }

    private void OnItemPickedUpCallBack(Item item)
    {
        // Vamos comparar o item com o goalItem se eles tiverem o nome igual "É O ENUM" então decrementamos a quatidade de goals
        for (int i = 0; i < goals.Length; i++)
        {
            if (!goals[i].itemPrefab.ItemName.Equals(item.ItemName)) // Caso não seja o mesmo nome do enum
                continue;

            goals[i].amount--; // Decrementamos a quantidade de goals do objeto

            if (goals[i].amount <= 0)
                CompletedGoal(i);
            else
                goalsCardList[i].UpdateAmount(goals[i].amount); 

                break;

        }
    }

    private void CompletedGoal(int goalIndex)
    {
        Debug.LogWarning($"Goal Completed:  {goals[goalIndex].itemPrefab.ItemName}");
        goalsCardList[goalIndex].Complete();

        CheckForLevelCompleted();
    }

    private void CheckForLevelCompleted()
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].amount > 0)
                return;
        }

        Debug.LogWarning("Level Completed !!");
    }
}
