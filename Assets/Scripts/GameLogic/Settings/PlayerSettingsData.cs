using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Player Settings Data", menuName = "Setting/Player Settings Data")]
    public class PlayerSettingsData : ScriptableObject
    {
        [SerializeField, Range(0f, 100f)] private float _playerSpeed = 5f;
        [SerializeField, Range(0f, 100f)] private float _dashDistance = 5f;
        [SerializeField, Range(0f, 100f)] private float _dashSpeed = 100f;
        [SerializeField, Range(0f, 5f)] private float _dashStunTime = 0.5f;
        [SerializeField, Range(0f, 5f)] private float _changeColorTime = 3f;
        [SerializeField, Range(0f, 5f)] private float _lookSensivity = 3f;
        [SerializeField] private Color _colorAfterHit;

        public float PlayerSpeed => _playerSpeed;
        public float DashDistance => _dashDistance;
        public float DashSpeed => _dashSpeed;
        public float DashStunTime => _dashStunTime;
        public float ChangeColorTime => _changeColorTime;
        public float LookSensivity => _lookSensivity;
        public Color ColorAfterHit => _colorAfterHit;
    }
}
