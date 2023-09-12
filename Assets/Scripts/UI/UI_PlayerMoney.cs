using TMPro;
using UnityEngine;

public class UI_PlayerMoney : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMoney;
    [SerializeField] private GameEvent gameEvent;

    private void OnEnable()
    {
        gameEvent.OnMoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        gameEvent.OnMoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(float money)
    {
        textMoney.text = money.ToString("$#.##");
    }
}
