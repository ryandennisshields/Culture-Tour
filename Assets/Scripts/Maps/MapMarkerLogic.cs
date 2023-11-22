using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace GCU.CultureTour.Map
{
    public class MapMarkerLogic : MonoBehaviour
    {
        /// <summary>
        /// This code manages the logic behind objects. This includes:
        /// Detecting if the player goes in and out of range using Colliders.
        /// What the object does depending on it's name when interacted with.
        /// </summary>

        private bool _inRange;
        private MapMarkerSO _mapMarker = null;
        private SphereCollider _sphereCollider;

        private void Awake()
        {
            _sphereCollider = gameObject.AddComponent<SphereCollider>();
            _sphereCollider.enabled = false;
            _sphereCollider.isTrigger = true;
        }

        public void Initalise( MapMarkerSO marker )
        {
            _mapMarker = marker;

            _sphereCollider.radius = marker.InteractionRadius;
            _sphereCollider.enabled = true;

            SetColour();
        }

        #region Collisions and Triggers

        private void OnCollisionEnter(Collision collision)
        {
            Enter (collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            Exit(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            Enter(other.gameObject);    
        }

        private void OnTriggerExit(Collider other)
        {
            Exit(other.gameObject);
        }
        
        #endregion

        private void Enter(GameObject other)
        {
            if (other.name == "Player")
            {
                _inRange = true;
                Handheld.Vibrate();
                Debug.Log("Player is in range.", gameObject);
                SetColour();
            }
        }

        private void Exit (GameObject other)
        {
            if (other.name == "Player")
            {
                _inRange = false;
                Debug.Log("Player is out of range.", gameObject);
                SetColour();
            }
        }

        private void SetColour()
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            
            if ( renderer == null )
            {
                return;
            }

            renderer.material.color = _inRange ? Color.green : Color.red;
        }

        private void OnMouseUp()
        {
            if (_inRange)
            {
                if (_mapMarker == null)
                {
                    return;
                }

                SceneManager.LoadScene(_mapMarker.SceneToLoadOnInteraction);
            }
        }
    } 
}

