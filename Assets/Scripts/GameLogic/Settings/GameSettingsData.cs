using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Game Settings Data", menuName = "Setting/Game Settings Data")]
    public class GameSettingsData : ScriptableObject
    {
        [SerializeField, Range(0f, 10f)] private float _timeBeforRestart = 5f;
        [SerializeField] private int _hitForWin = 3;

        public float TimeBeforeRestart => _timeBeforRestart;
        public int HitForWin => _hitForWin;
    }
}
