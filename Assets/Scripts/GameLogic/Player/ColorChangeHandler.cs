using UnityEngine;

namespace GameLogic
{
    public class ColorChangeHandler : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _characterMeshRenderer;

        private Color _initialColor;

        public void ChangeColor(Color targetColor)
        {
            _initialColor = _characterMeshRenderer.material.color;
            _characterMeshRenderer.material.color = targetColor;
        }

        public void ReturnColor()
        {
            _characterMeshRenderer.material.color = _initialColor;
        }
    }
}
