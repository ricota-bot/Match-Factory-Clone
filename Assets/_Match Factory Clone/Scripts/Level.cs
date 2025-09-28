using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private ItemPlacer itemPlacer;

    [Header("Settings")]
    [SerializeField] private int durationInSeconds;
    public int DurationInSeconds => durationInSeconds;
    public ItemLevelData[] GetGoals() => itemPlacer.GetGoals();
}
