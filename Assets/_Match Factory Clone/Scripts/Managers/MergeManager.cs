using System;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{

    [Header("Go UP Settings")]
    [SerializeField] private float goUpDistance;
    [SerializeField] private float goUpDuration;
    [SerializeField] private LeanTweenType goUpEasing;

    [Header("Smash Settings")]
    [SerializeField] private float smashDuration;
    [SerializeField] private LeanTweenType smashEasing;

    [Header("Effects")]
    [SerializeField] private GameObject mergeParticles;

    private void Awake()
    {
        ItemSpotsManager.OnMergeStarted += OnMergeStartedCallBack;
    }

    private void OnDestroy()
    {
        ItemSpotsManager.OnMergeStarted -= OnMergeStartedCallBack;

    }

    private void OnMergeStartedCallBack(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Vector3 targetPosition = items[i].transform.position + items[i].transform.up * goUpDistance;

            Action callBack = null;
            if (i == 0) // Fazemos i = o para chamar a ação apenas no primeiro item
                callBack = () => SmashItems(items);

            LeanTween.move(items[i].gameObject, targetPosition, goUpDuration)
                .setEase(goUpEasing)
                .setOnComplete(callBack); // Isso chama o metodo Smash
        }
    }

    private void SmashItems(List<Item> items)
    {
        items.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        float targetX = items[1].transform.position.x; // Pegamos o Index do meio, item do meio

        LeanTween.moveX(items[0].gameObject, targetX, smashDuration) // Primeira Posição
            .setEase(smashEasing)
            .setOnComplete(() => FinalizeMerge(items));

        LeanTween.moveX(items[2].gameObject, targetX, smashDuration) // Ultima Posição
            .setEase(smashEasing);
    }

    private void FinalizeMerge(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
            Destroy(items[i].gameObject);

        GameObject particles = Instantiate(mergeParticles, items[1].transform.position, Quaternion.identity, transform);
    }
}
