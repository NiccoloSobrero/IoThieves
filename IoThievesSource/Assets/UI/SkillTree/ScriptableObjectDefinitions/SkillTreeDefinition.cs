using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillTreeDefinition", menuName = "SkillTree/SkillTreeDefinition")]
public class SkillTreeDefinition : ScriptableObject
{
    public List<SkillNode> skillNodes = new List<SkillNode>();
}
