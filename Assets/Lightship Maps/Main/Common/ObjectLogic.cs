using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GCU.CultureTour.Map
{
    public class ObjectLogic : MonoBehaviour
    {
        /// <summary>
        /// This code manages the logic behind objects. This includes:
        /// Detecting if the player goes in and out of range using Colliders.
        /// What the object does depending on it's name when interacted with.
        /// </summary>

        private bool inRange;
        private string sceneNameToLoad = null;

        public void Initalise(string sceneNameToLoad)
        {
            this.sceneNameToLoad = sceneNameToLoad;
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
                inRange = true;
                Handheld.Vibrate();
                Debug.Log("Player is in range.", gameObject);
                SetColour();
            }
        }

        private void Exit (GameObject other)
        {
            if (other.name == "Player")
            {
                inRange = false;
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

            renderer.material.color = inRange ? Color.green : Color.red;
        }

        private void OnMouseUp()
        {
            if (inRange)
            {
                if (sceneNameToLoad == null)
                {
                    return;
                }

                SceneManager.LoadScene(sceneNameToLoad);
            }
        }
    } 
}

