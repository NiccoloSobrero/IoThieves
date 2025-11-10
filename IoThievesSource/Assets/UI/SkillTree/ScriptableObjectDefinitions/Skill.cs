using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillTree/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public int skillCost;
    public Sprite skillIcon;
}
