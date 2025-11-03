using UnityEngine;

namespace Enemy
{
    public class FOVCollider : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;

        private float _alertLevel;
        private float _alertIncrease;

        private bool _isAlerted;
        private Transform _player;

        private void Awake()
        {
            _isAlerted = false;
        }

        private void Update()
        {
            if (!_isAlerted && _alertLevel > 0)
            {
                _alertLevel -= Time.deltaTime;
            }
            else if (_isAlerted)
            {
                CheckAlertStatus();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (Physics.Raycast(gameObject.transform.position,
                        other.gameObject.transform.position - gameObject.transform.position,
                        out _,
                        20f,
                        playerLayer))
                {
                    _player = other.gameObject.transform;
                    
                    _isAlerted = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isAlerted = false;
            }
        }

        private void CheckAlertStatus()
        {
            if (Physics.Raycast(gameObject.transform.position,
                    _player.position - gameObject.transform.position,
                    out _,
                    20f,
                    playerLayer))
            {
                _alertIncrease = Vector3.Dot(gameObject.transform.forward, 
                    (_player.position - gameObject.transform.position).normalized);
                _alertLevel += _alertIncrease;
                    
                Debug.Log($"Alert: {_alertLevel} increased by {_alertIncrease}");
            }
            else
            {
                _isAlerted = false;
            }
        }
    }
}
