using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillNode", menuName = "SkillTree/SkillNode")]
public class SkillNode : ScriptableObject
{
    // [HideInInspector]
    public bool isLocked;
    public Skill skill;
    public List<SkillNode> prerequisites;
    public Vector2 positionInTree;
    
}
