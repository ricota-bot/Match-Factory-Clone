using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalCard : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image checkmarkImage;


    public void Configure(int initialAmount)
    {
        amountText.text = initialAmount.ToString();
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }

    public void Complete()
    {
        amountText.gameObject.SetActive(false);
        checkmarkImage.gameObject.SetActive(true);
    }
}
