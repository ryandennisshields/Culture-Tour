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
                Debug.LogWarning( "Scene name not specified." );
                return;
            }

            SceneManager.LoadScene( sceneName );
        }

    }
}
