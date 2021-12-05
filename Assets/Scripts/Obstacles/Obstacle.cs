using System;
using Managers;
using UnityEngine;

namespace Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        private bool _activated;
        protected GameObject Car;

        private void OnEnable()
        {
            GameManager.Respawn.AddListener(CarRespawned);
        }

        private void OnDisable()
        {
            GameManager.Respawn.RemoveListener(CarRespawned);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player") || _activated) return;
            _activated = true;
            Car = other.gameObject;
            ObstacleAttitude();
        }

        protected virtual void ObstacleAttitude()
        {
        
        }

        private void CarRespawned()
        {
            _activated = false;
        }
    }
}
