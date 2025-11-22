using Events;
using UnityEngine;

namespace Hackables
{
    public abstract class HackableDevice : MonoBehaviour
    {
        public HackableDeviceScrObj scrObj;

        public abstract void Hack();
    }

    public class HackableToMinigame : HackableDevice
    {
        public override void Hack()
        {
            EventsManager.OnHackableInteraction.Invoke(scrObj.minigamePrefab);
        }
    }

    public class HackableToDesktop : HackableDevice
    {
        public override void Hack()
        {
            
        }
    }
}
