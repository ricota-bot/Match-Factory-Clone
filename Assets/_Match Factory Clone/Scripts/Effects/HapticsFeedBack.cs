using System;
using UnityEngine;

public class HapticsFeedBack : MonoBehaviour
{
    private void Awake()
    {
        BumpyButtons.OnButtonPressed += OnButtonPressedCallBack;
    }

    private void OnDestroy()
    {
        BumpyButtons.OnButtonPressed -= OnButtonPressedCallBack;

    }

    private void OnButtonPressedCallBack()
    {
        Debug.Log("Vibrando !!");
        CandyCoded.HapticFeedback.HapticFeedback.MediumFeedback();
    }
}
