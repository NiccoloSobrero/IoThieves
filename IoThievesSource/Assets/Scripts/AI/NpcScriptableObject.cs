using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "NpcScriptableObject", menuName = "AI/NpcScriptableObject")]
    public class NpcScriptableObject : ScriptableObject
    {
      [SerializeField]private float _speed;
      [SerializeField]private float _angularSpeed;

      [SerializeField]private float _alternativeSpeed;
      [SerializeField]private float _alternativeAngularSpeed;
      
      [SerializeField]private float _distanceThreshold;
      [SerializeField]private float _angularThreshold;
        
        public float Speed
        {
            get => _speed;
        }

        public float AngularSpeed
        {
            get => _angularSpeed;
        }

        public float AlternativeSpeed
        {
            get => _alternativeSpeed;
        }

        public float AlternativeAngularSpeed
        {
            get => _alternativeAngularSpeed;
        }

        public float DistanceThreshold
        {
            get => _distanceThreshold;
        }

        public float AngularThreshold
        {
            get => _angularThreshold;
        }
    }
}