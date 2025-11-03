using UnityEngine;
using UnityEngine.Serialization;

namespace Hackables
{
    public abstract class HackableDevice : MonoBehaviour
    {
        [SerializeField] protected HackableDeviceScrObj scrObj;
        
        public abstract void Hack();
    }
}
