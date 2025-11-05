using Events;
using UnityEngine;

namespace Hackables
{
    public class HackableDevice : MonoBehaviour
    {
        public HackableDeviceScrObj scrObj;

        public void Hack()
        {
            Debug.Log($"Hacking {scrObj.deviceName}");

            EventsManager.OnHackableInteraction.Invoke(scrObj.minigamePrefab);
        }
    }
}
