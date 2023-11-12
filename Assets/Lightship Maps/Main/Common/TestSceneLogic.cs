using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Niantic.Lightship.Maps.Samples.OrbitCamera
{
    public class TestSceneLogic : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnPress()
        {
            SceneManager.LoadScene("OrbitCamera");
        }
    }
}
