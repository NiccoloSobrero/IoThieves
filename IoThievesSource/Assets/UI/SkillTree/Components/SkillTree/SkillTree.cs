using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillTree : MonoBehaviour
{
    [SerializeField]
    private UIDocument skillTreeDocument;
    [SerializeField]
    private VisualTreeAsset skillNodeTemplate;
    [SerializeField]
    private VisualTreeAsset skillTree;
    [SerializeField]
    private SkillTreeDefinition skillTreeDefinition;
    
    public static event Action<SkillNode, VisualTreeAsset> OnSkillPurchased;
    
    private VisualElement skillTreeVisualElement;
    private Dictionary<SkillNode, VisualElement> nodeUImap = new Dictionary<SkillNode, VisualElement>();

    private void OnEnable()
    {
        var root = skillTreeDocument.rootVisualElement;
        
        skillTreeVisualElement = root.Q<VisualElement>("skillTree");

        if (skillTreeVisualElement == null)
        {
            print("skillTreeScrollView is null");
            return;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (skillTreeVisualElement == null || skillNodeTemplate == null)
            return;

        foreach (SkillNode skillNode in skillTreeDefinition.skillNodes)
        {
            var skillNodeInstance = skillNodeTemplate.Instantiate();
            var skillNodeVisualElement = skillNodeInstance.Q<VisualElement>("skillNode");
            var skillNodeIcon = skillNodeInstance.Q<Image>("skillIcon");
            var nameLabel =  skillNodeInstance.Q<Label>("skillName");
            var costLabel =  skillNodeInstance.Q<Label>("skillCost");
            var skillBuyButton = skillNodeInstance.Q<Button>("skillBuyButton");

            if (skillNode.skill != null)
            {
                nameLabel.text = skillNode.skill.skillName;
                costLabel.text = $"{skillNode.skill.skillCost}";
                if (skillNode.skill.skillIcon != null)
                {
                    skillNodeIcon.image = skillNode.skill.skillIcon.texture;
                }
            }
            
            // should work at least in Editor but it isn't
            skillBuyButton.tooltip = skillNode.skill.skillDescription;
            
            if (skillNode == null)
            {
                print("skillNode is null");
            }

            if (skillNodeVisualElement == null)
            {
                print("skillNodeVisualElement is null");
            }
            
            UpdateSkillNodeVisual(skillNode, skillNodeVisualElement);

            skillBuyButton.clicked += () =>
            {
                TryBuySkill(skillNode);
            };
            
            skillTreeVisualElement.Add(skillNodeInstance);
            
            nodeUImap[skillNode] = skillNodeInstance;
        }
    }

    private void UpdateSkillNodeVisual(SkillNode skillNode, VisualElement skillNodeVisualElement)
    {
        var skillBuyButton = skillNodeVisualElement.Q<Button>("skillBuyButton");
        if (!skillNode.isLocked)
        {
            // skillBuyButton.text = "Owned";
            skillBuyButton.SetEnabled(false);
            skillNodeVisualElement.AddToClassList("unlocked");
        }
        else
        {
            // skillBuyButton.text = "Buy";
            skillBuyButton.SetEnabled(true);
            skillNodeVisualElement.RemoveFromClassList("unlocked");
        }
    }

    private void TryBuySkill(SkillNode skillNode)
    {
        if (!skillNode.isLocked) return;
        
    }
}
