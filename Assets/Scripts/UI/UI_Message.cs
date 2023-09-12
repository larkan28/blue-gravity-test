using TMPro;
using UnityEngine;

public class UI_Message : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMessage;
    [SerializeField] private float duration;

    public void Show(string text)
    {
        textMessage.text = text;
        Destroy(gameObject, duration);
    }
}
