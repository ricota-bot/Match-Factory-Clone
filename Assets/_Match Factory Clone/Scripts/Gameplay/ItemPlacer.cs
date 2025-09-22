using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

#endif

public class ItemPlacer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private List<ItemLevelData> itemData;

    [Header("Settings")]
    [SerializeField] private BoxCollider spawnZone;
    [SerializeField] private int seed;


    public ItemLevelData[] GetGoals()
    {
        List<ItemLevelData> goals = new List<ItemLevelData>();
        foreach (var item in itemData)
            if (item.isGoal)
            {
                goals.Add(item);
            }
        return goals.ToArray();
    }

#if UNITY_EDITOR
    [Button]
    private void GenerateItems()
    {
        while (transform.childCount > 0) // Vamos remover todos os items desse gameObject
        {
            Transform t = transform.GetChild(0); // Pega o primeiro filho
            t.SetParent(null); // UnParent
            DestroyImmediate(t.gameObject); // Destroy
        }

        Random.InitState(seed);

        for (int i = 0; i < itemData.Count; i++)
        {
            ItemLevelData data = itemData[i]; // Pegamos todos os itemData

            for (int j = 0; j < data.amount; j++) // Do item Data que pegamos vamos pegar o seu amount
            {
                Vector3 spawnPosition = GetSpawnPosition();

                Item itemInstace = PrefabUtility.InstantiatePrefab(data.itemPrefab, transform) as Item;
                itemInstace.transform.position = spawnPosition;
                itemInstace.transform.rotation = Quaternion.Euler(Random.onUnitSphere * 360);
            }
        }

    }

    private Vector3 GetSpawnPosition()
    {
        float x = Random.Range(-spawnZone.size.x / 2, spawnZone.size.x / 2);
        float y = Random.Range(-spawnZone.size.y / 2, spawnZone.size.y / 2);
        float z = Random.Range(-spawnZone.size.z / 2, spawnZone.size.z / 2);

        Vector3 localPosition = spawnZone.center + new Vector3(x, y, z);
        Vector3 spawnPosition = transform.TransformPoint(localPosition); // transforma de local position para world position

        return spawnPosition;
    }
#endif
}
