using UnityEngine;

namespace Hackables
{
    public abstract class HackableDevice : MonoBehaviour
    {
        public HackableDeviceScrObj scrObj;
        
        public abstract void Hack();
    }
}
