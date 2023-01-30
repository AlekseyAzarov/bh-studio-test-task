using UnityEngine;

namespace GameLogic
{
    public class SpawnPointGizmos : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + new Vector3(0f, 0.5f), new Vector3(1f, 1f, 1f));
        }
    }
}