using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalCard : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject checkmarkImage;
    [SerializeField] private GameObject backFace;
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator.enabled = false;
    }

    private void Update()
    {
        backFace.SetActive(Vector3.Dot(Vector3.forward, transform.forward) < 0);
    }

    public void Configure(int initialAmount, Sprite icon)
    {
        amountText.text = initialAmount.ToString();
        this.icon.sprite = icon;
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();

        BumpCardAnimation();
    }

    private void BumpCardAnimation()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.1f, .1f)
            .setLoopPingPong(1);
    }

    public void Complete()
    {
        amountText.gameObject.SetActive(false);
        checkmarkImage.SetActive(true);

        animator.enabled = true;
        animator.Play("Completed");
    }
}
