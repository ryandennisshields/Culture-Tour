using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GCU.CultureTour
{
    public class Compass : MonoBehaviour
    {
        public GameObject camera;
        public GameObject compass;
        private Vector3 direction;
        private float northDirection;

        void Start()
        {
            northDirection = 180f;
        }

        void Update()
        {
            direction.z = camera.transform.eulerAngles.y - northDirection;
            compass.transform.localEulerAngles = direction;
        }
    }
}
