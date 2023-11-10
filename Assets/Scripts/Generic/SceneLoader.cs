using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GCU.CultureTour
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private string sceneName;

        public void LoadScene()
        {
            if ( string.IsNullOrWhiteSpace( sceneName ) )
            {
                return;
            }

            SceneManager.LoadScene( sceneName );
        }

    }
}
