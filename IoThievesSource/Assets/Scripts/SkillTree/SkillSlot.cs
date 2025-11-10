using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot : MonoBehaviour
{
    [SerializeField]
    private Skill skill;
    [SerializeField]
    private Image skillIcon;
    [SerializeField]
    private TMP_Text skillCostText;
    [SerializeField]
    private bool hasBought = false;

    private void OnValidate()
    {
        if (skill != null)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        skillIcon.sprite = skill.skillIcon;
        if (!hasBought)
        {
            skillCostText.text = skill.skillCost.ToString();
            skillIcon.color = Color.white;
        }
        else
        {
            skillCostText.text = "Upgraded";
            skillIcon.color = Color.gray;
        }
    }
}
