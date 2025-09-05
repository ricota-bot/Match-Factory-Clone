using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BumpyButtons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Elements")]
    private Button button;

    [Header("Actions")]
    public static Action OnButtonPressed;

    private void Awake() => button = GetComponent<Button>();


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable)
            return;

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(button.gameObject, new Vector3(1.1f, 0.9f), .5f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);

        Debug.Log("Clickou");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!button.interactable)
            return;

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(button.gameObject, Vector3.one, .5f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);

        LeanTween.color(button.gameObject, Color.cyan, 0.5f).setIgnoreTimeScale(true);


        OnButtonPressed?.Invoke(); // Call the Action

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable)
            return;

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(button.gameObject, Vector3.one, .5f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);

    }
}
