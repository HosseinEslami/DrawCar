using UnityEngine;

namespace Obstacles
{
    public class CheckPoint : Obstacle
    {
        protected override void ObstacleAttitude()
        {
            Debug.Log("CheckPoint");
            Car.GetComponent<Line>().checkpointPos = Car.transform.position;
        }
    }
}
