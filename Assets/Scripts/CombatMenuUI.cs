using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenuUI : MonoBehaviour
{
    public static CombatMenuUI Instance { get; private set; }

    [SerializeField] private GameObject panel;

    [SerializeField] private List<Button> buttons;
    private List<TMPro.TextMeshProUGUI> texts;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        texts = new List<TMPro.TextMeshProUGUI>();

        for(int i = 0; i < buttons.Count; i++)
        {
            texts.Add(buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>());
        }
    }

    public void Setup(List<AbilityBase> abilities, System.Action<AbilityBase> onClick)
    {
        panel.SetActive(true);

        for(int i = 0; i < abilities.Count; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
            if (abilities[i].GetType() != typeof(Ability_Passive))
                SetOnClick(buttons[i], abilities[i], onClick);
            else
                buttons[i].interactable = false;
            texts[i].text = abilities[i].Name;
        }
    }

    public void SetOnClick(Button button, AbilityBase ability, System.Action<AbilityBase> onClick)
    {
        button.onClick.AddListener(() => onClick(ability));
    }
}
