using UnityEngine;

namespace AI
{
    public class Device : MonoBehaviour
    {
        [SerializeField] private int interactTime;
        [SerializeField] private float value = 0;
        
        [SerializeField] public bool isHacked = false;
        [SerializeField] private float hackedValue = 0;
        [SerializeField] private float hackedTime = 4;

        public int InteractTime => interactTime;

        public float Value
        {
            get => value;
        }
        
        public bool Interact()
        {
            value += (1.0f/interactTime)*Time.deltaTime;
            if (value >= 1)
            {
                value = 0;
                return true;                   
            }
            return false;
        }

        public bool ResolveHack()
        {
            if (!isHacked)
                return true;
            
            hackedValue += (1.0f/hackedTime)*Time.deltaTime;
            if (hackedValue >= 1)
            {
                hackedValue = 0;
                isHacked = false;
                return true;                   
            }
            return false;
        }
    }
}