using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource buttonClickSource;
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
        buttonClickSource.volume = 0.5f;
        buttonClickSource.pitch = Random.Range(0.95f, 1.3f);
        buttonClickSource.Play();
    }
}
