using Events;
using UnityEngine;

namespace Hackables
{
    public class HackableDevice : MonoBehaviour
    {
        public HackableDeviceScrObj scrObj;

        public void Hack()
        {
            EventsManager.OnHackableInteraction.Invoke(scrObj.minigamePrefab);
        }
    }
}
