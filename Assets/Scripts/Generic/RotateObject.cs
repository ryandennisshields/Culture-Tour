using UnityEngine;

namespace GCU.CultureTour
{
    /// <summary>
    /// Written by Chat GPT 2023-11-10.
    /// </summary>
    public class RotateObject : MonoBehaviour
    {
        public Vector3 rotationAxis = Vector3.forward; // Default rotation axis is around the Y-axis
        public float rotationSpeed = 30f; // Default rotation speed in degrees per second
        public bool rotateEnabled = true; // Enable or disable rotation

        void Update()
        {
            if (rotateEnabled)
            {
                // Rotate the object based on the specified velocity
                transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
            }
        }
    }

}
