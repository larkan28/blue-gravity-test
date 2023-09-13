using TMPro;
using UnityEngine;

public class UI_Interaction : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private GameObject textContainer;
    [SerializeField] private TextMeshProUGUI textInteraction;

    private void OnEnable()
    {
        gameEvent.OnShowInteraction += OnShowInteraction;
    }

    private void OnDisable()
    {
        gameEvent.OnShowInteraction -= OnShowInteraction;
    }

    private void Awake()
    {
        textContainer.SetActive(false);
    }

    private void OnShowInteraction(Interactable interaction)
    {
        if (interaction == null)
            textContainer.SetActive(false);
        else
        {
            textInteraction.text = interaction.GetText();
            textContainer.SetActive(true);
        }
    }
}
