using UnityEngine;

public class UI_MessageManager : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Transform messageContainer;
    [SerializeField] private UI_Message messagePrefab;

    private void OnEnable()
    {
        gameEvent.OnSendMessage += OnSendMessage;
    }

    private void OnDisable()
    {
        gameEvent.OnSendMessage -= OnSendMessage;
    }

    public void OnSendMessage(string text)
    {
        UI_Message message = Instantiate(messagePrefab, messageContainer);

        if (message != null)
            message.Show(text);
    }
}
