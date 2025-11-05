using UnityEngine;

namespace Hackables
{
    [CreateAssetMenu(fileName = "HackableDeviceScrObj",  menuName = "Hackable Device")]
    public class HackableDeviceScrObj : ScriptableObject
    {
        public string deviceName;
        public float suspicionLevelIncrease;
        public float attractionRange;

        public GameObject minigamePrefab;
    }
}
