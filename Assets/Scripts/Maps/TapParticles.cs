using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GCU.CultureTour
{
    /// <summary>
    /// Written by Chat GPT 2024-02-22.
    /// </summary>
    public class TapParticles : MonoBehaviour
    {
        public ParticleSystem tapParticles;

        void Update()
        {
            // Check for touch input
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Vector3 inputPosition;

                if (Input.GetMouseButtonDown(0))
                {
                    // Mouse input
                    inputPosition = Input.mousePosition;
                }
                else
                {
                    // Touch input
                    inputPosition = Input.GetTouch(0).position;
                }

                // Convert input position to world space
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 10f));

                // Instantiate the tap particles as a Particle System
                ParticleSystem tapParticle = Instantiate(tapParticles, touchPosition, Quaternion.identity);
                tapParticle.gameObject.transform.SetParent(Camera.main.transform, true);
            }
        }
    }
}
