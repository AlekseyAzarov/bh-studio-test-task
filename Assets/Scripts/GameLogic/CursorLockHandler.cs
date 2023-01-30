using UnityEngine;

namespace GameLogic
{
    public class CursorLockHandler : MonoBehaviour
    {
        [SerializeField] private CursorLockMode _lockState;

        private void Start()
        {
            Cursor.lockState = _lockState;
        }
    }
}
