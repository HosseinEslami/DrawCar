using UnityEngine;

namespace Obstacles
{
    public class SpeedBoost : Obstacle
    {
        public int boostVal;
        //public int maxForce;
        private JointMotor2D _boostedMotor;
        protected override void ObstacleAttitude()
        {
            Debug.Log("SpeedBoost");
            Car.GetComponent<Rigidbody2D>().velocity += Vector2.right * boostVal; 
            /*_boostedMotor.motorSpeed = boostVal;
            _boostedMotor.maxMotorTorque = maxForce;
            var line = Car.GetComponent<Line>();
            line.fWheelJoint.motor = line.bWheelJoint.motor = _boostedMotor;*/
        }
    }
}
