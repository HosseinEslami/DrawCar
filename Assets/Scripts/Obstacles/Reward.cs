using Managers;
using UnityEngine;

namespace Obstacles
{
    public class Reward : Obstacle
    {
        public int rewardAmount;
        public bool isFinalFlag;
        protected override void ObstacleAttitude()
        {
            Debug.Log("reward");
            CurrencyManager.Earn(rewardAmount, "Coin");
            if(isFinalFlag) GameManager.Instance.gameOver.Invoke();
            else gameObject.SetActive(false);
        }
    }
}