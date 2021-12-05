using UnityEngine;

namespace Managers
{
    public class TrailDrawManager : MonoBehaviour
    {
        public GameObject drawPref;
        private GameObject _trail;
        private Plane _plane;
        private Camera _cam;
        private Vector3 _startPos;

        private void Start()
        {
            _cam = Camera.main;
            if (_cam is { }) _plane = new Plane(_cam.transform.forward * -1, transform.position);
        }

        private void Update()
        {
            var tmpPos = Input.mousePosition;
            tmpPos.z = 10f;
            transform.position = _cam.ScreenToWorldPoint(tmpPos);
        
            var ray2 = _cam.ScreenPointToRay(Input.mousePosition);
            if (!_plane.Raycast(ray2, out var dis2)) return;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
            {
                _trail = Instantiate(drawPref, transform.position, Quaternion.identity);
                _startPos = ray2.GetPoint(dis2);
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
                _trail.transform.position = ray2.GetPoint(dis2);
        }
    }
}