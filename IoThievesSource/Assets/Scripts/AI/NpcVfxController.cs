using UnityEngine;

namespace AI
{
    public class NpcVfxController : MonoBehaviour
    {
        [Header("Stun VFX")]
        [SerializeField] private GameObject stunVfxPrefab;

        private GameObject _currentStunVfx;

        public void Initialize()
        {
            if (_currentStunVfx is not null)
            {
                Destroy(_currentStunVfx);
                _currentStunVfx = null;
            }
        }

        public void StunVfx(bool active)
        {
            if (active)
            {
                if (_currentStunVfx is not null) return;

                if (stunVfxPrefab is not null)
                {
                    Vector3 spawnPos = transform.position + Vector3.up * 2.2f;
                    _currentStunVfx = Instantiate(stunVfxPrefab, spawnPos, Quaternion.Euler(90,0,0), transform);
                }
            }
            else
            {
                // Despawn
                if (_currentStunVfx is not null)
                {
                    Destroy(_currentStunVfx);
                    _currentStunVfx = null;
                }
            }
        }
    }
}
