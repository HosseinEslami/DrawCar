using UnityEngine;

namespace Managers
{
    public class CamFollowManager : MonoBehaviour
    {
        [HideInInspector] public GameObject target;
        
        [SerializeField] private Vector3 offset;

        private void FixedUpdate()
        {
            if(!target) return;
            var camPos = transform.position;
            var targetPos = target.transform.position;

            var smoothedPos = Vector3.Lerp(camPos, targetPos, 0.125f);
            transform.position = smoothedPos + offset;
        }
    }
}
