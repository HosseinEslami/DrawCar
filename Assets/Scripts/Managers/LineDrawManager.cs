using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class LineDrawManager : MonoBehaviour
    {
        public const float Resolution = 0.1f;

        [SerializeField] private Line line1, line2;

        private Camera _cam;
        private CamFollowManager _camFollowManager;
        private Line _currentLine;
        private Plane _plane;
        private int _drawLayer;
        private Vector3 _currentCarPos;
        private Vector2 _startDrawPos;
        private bool _firstDraw;

        private void Start()
        {
            _cam = Camera.main;
            if (_cam is { })
            {
                _camFollowManager = _cam.GetComponent<CamFollowManager>();
                _plane = new Plane(_cam.transform.forward * -1, transform.position);
            }

            _drawLayer = LayerMask.NameToLayer("DrawBoard");
            _currentLine = line1;
        }


        private void Update()
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (!_plane.Raycast(ray, out var dis)) return;

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
            {
                if (!Physics.Raycast(ray, out _, 20))
                {
                    EndDraw();
                    return;
                }
                StartDraw();
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
                Drawing();
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
                EndDraw();
        
        }

        private void StartDraw()
        {
            if (_firstDraw)
            {
                //_currentLine.StopCar();
                ChangeCurrentLine();
            }
            else _firstDraw = true;
            
            _currentLine.gameObject.SetActive(true);
            _startDrawPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _currentLine.NewDrawing();
        }

        private void Drawing()
        {
            var tmpPos = Input.mousePosition;
            tmpPos.z = 10f;
            Transform t;
            (t = transform).position = _cam.ScreenToWorldPoint(tmpPos);
            _currentLine.SetPosition(t.position);// - new Vector3(_startDrawPos.x, _startDrawPos.y, 0));
        }

        private void EndDraw()
        {
            _currentCarPos = _currentLine.carStartPos.transform.position;
            if (_currentLine == line1 && line2.isActiveAndEnabled)
            {
                _currentCarPos = line2.transform.position;
                line2.gameObject.SetActive(false);
            }
            else if(_currentLine == line2 && line1.isActiveAndEnabled)
            {
                _currentCarPos = line1.transform.position;
                line1.gameObject.SetActive(false);
            }


            _currentLine.WheelsSetup();
            _currentLine.transform.position = _currentCarPos;// - new Vector3(_startDrawPos.x, _startDrawPos.y, 0);
            _currentLine.rigidbody.bodyType = RigidbodyType2D.Dynamic;

            //_camFollowManager.target = _currentLine.gameObject;
        }

        private void ChangeCurrentLine()
        {
            //Change Current Line
            _currentLine = _currentLine == line1 ? line2 : line1;
            _currentLine.rigidbody.bodyType = RigidbodyType2D.Kinematic;
            var tOld = _currentLine.transform;
            //tOld.parent = null;
            tOld.position = Vector3.zero;
            tOld.rotation = Quaternion.identity;
            _currentLine.fWheel.gameObject.SetActive(false);
            _currentLine.bWheel.gameObject.SetActive(false);
        }
    }
}