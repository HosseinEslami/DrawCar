using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Transform fWheel, bWheel, carStartPos;

    [HideInInspector] public new Rigidbody2D rigidbody;
    [HideInInspector] public WheelJoint2D fWheelJoint,bWheelJoint;
    //[HideInInspector] 
    public Vector3 checkpointPos;

    //[SerializeField] private
        public LineRenderer lRenderer;
    [SerializeField] private EdgeCollider2D edgeCollider;
    [SerializeField] private float colliderWith, motorSpeed, motorMaxForce;

    public  List<Vector2> _points = new List<Vector2>();
    private readonly List<CircleCollider2D> _circleColliders = new List<CircleCollider2D>();
    private JointMotor2D _fJointMotor,_bJointMotor;
    private bool _freezeCar;
    private Vector3 _lineFreezePos;

    private void Start()
    {
        edgeCollider.transform.position -= transform.position;
        edgeCollider.edgeRadius = colliderWith;
        rigidbody = GetComponent<Rigidbody2D>();
        fWheelJoint = fWheel.GetComponent<WheelJoint2D>();
        //_fJointMotor = fWheel.GetComponent<JointMotor2D>();
        
        bWheelJoint = bWheel.GetComponent<WheelJoint2D>();
        //_bJointMotor = bWheel.GetComponent<JointMotor2D>();
        /*lRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponentInChildren<EdgeCollider2D>();*/
    }

    private void Update()
    {
        if (_freezeCar) transform.position = _lineFreezePos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("boundary"))
        {
            Debug.Log("Respawn");
            fWheel.gameObject.GetComponent<Rigidbody2D>().velocity = bWheel.gameObject.GetComponent<Rigidbody2D>().velocity =  rigidbody.velocity = Vector2.zero;
            GameManager.Respawn.Invoke();
            transform.rotation = Quaternion.identity;
            transform.position = checkpointPos != Vector3.zero ? checkpointPos : carStartPos.position;
            //checkpointPos = Vector3.zero;
        }
    }

    public void NewDrawing()
    {
        //Debug.Log("NewDrawing");
        _points.Clear();
        lRenderer.positionCount = 0;
        edgeCollider.points  = new Vector2[0];
        //edgeCollider.enabled = false;

        foreach (var circleCollider in _circleColliders)
        {
            circleCollider.enabled = false;
        }
    }

    public void SetPosition(Vector2 pos, bool justRefresh)
    {
        if (!justRefresh && !CanAppend(pos)) return;

        if(!justRefresh)
        {
            _points.Add(pos);

            var positionCount = lRenderer.positionCount;
            positionCount++;
            lRenderer.positionCount = positionCount;
            lRenderer.SetPosition(positionCount - 1, pos);
            
            var circleCollider = GetFromCircleColliderPool();
            circleCollider.enabled = true;
            circleCollider.offset = pos;
            circleCollider.radius = colliderWith;
        }
        else
        {
            for (var i = 0; i < _points.Count; i++)
            {
                _circleColliders[i].offset = _points[i];
            }
        }

        edgeCollider.points = _points.ToArray();
    }

    private CircleCollider2D GetFromCircleColliderPool()
    {
        if(_circleColliders.Count > 0)
            foreach (var circleCollider in _circleColliders.Where(circleCollider => !circleCollider.isActiveAndEnabled)) return circleCollider;
        
        var tmpCircleCollider = gameObject.AddComponent<CircleCollider2D>();
        _circleColliders.Add(tmpCircleCollider);
        return tmpCircleCollider;
    }

    private bool CanAppend(Vector2 pos)
    {
        if (lRenderer.positionCount == 0) return true;

        return Vector2.Distance(lRenderer.GetPosition(lRenderer.positionCount - 1), pos) > LineDrawManager.Resolution;
    }

    public void WheelsSetup()
    {
        
        var fWheelPos = GetWheelPos(true);
        var bWheelPos = GetWheelPos(false);
        fWheel.position = fWheelPos;
        bWheel.position = bWheelPos;
        var wheelDis = fWheelPos.x - bWheelPos.x;
        fWheelJoint.connectedAnchor = fWheelPos;// new Vector2(fWheelPos.x, fWheelPos.y);
        bWheelJoint.connectedAnchor = bWheelPos;// new Vector2(bWheelPos.x, bWheelPos.y);
        fWheel.gameObject.SetActive(true);
        bWheel.gameObject.SetActive(true);
    }

    private Vector3 GetWheelPos(bool isFrontWheel)
    {
        var firstPos = lRenderer.GetPosition(0);
        var lastPos = lRenderer.GetPosition(lRenderer.positionCount -1);

        if (firstPos.x > lastPos.x) return isFrontWheel ? firstPos : lastPos;
        return isFrontWheel ? lastPos : firstPos;
    }

    public void StopCar()
    {
        _fJointMotor.motorSpeed = _bJointMotor.motorSpeed = 0;
        fWheelJoint.motor = _fJointMotor;
        bWheelJoint.motor = _bJointMotor;
        rigidbody.velocity = fWheel.GetComponent<Rigidbody2D>().velocity = bWheel.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _lineFreezePos = transform.position;
        _freezeCar = true;
    }
}