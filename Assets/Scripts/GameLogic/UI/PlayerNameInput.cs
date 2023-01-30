using Observer;
using TMPro;
using UnityEngine;

namespace GameLogic
{
    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;

        private void Start()
        {
            _inputField.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnDestroy()
        {
            _inputField.onEndEdit.RemoveListener(OnEndEdit);
        }

        private void OnEndEdit(string result)
        {
            ObserverService.Instance.RaiseEvent(new PlayerSetName { Name = result });
        }
    }
}
