using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI fpsText;
    private float deltaTime = 0.0f;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
    }
}
