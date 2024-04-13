using System.Collections;
using UnityEngine;

namespace GCU.CultureTour
{

    [RequireComponent(typeof(SceneLoader))]
    public class GameStart : MonoBehaviour
    {
        [SerializeField]
        string [] _messages = new string[0];

        StatusMessageDisplay _messageDisplay;

        private void Awake()
        {
            _messageDisplay = FindObjectOfType<StatusMessageDisplay>();
        }

        private IEnumerator Start ( )
        {
            foreach ( var message in _messages )
            {
                _messageDisplay.DisplayMessage( message );
            }

            yield return null;

            while ( _messageDisplay.HasMessages )
            {
                yield return null;
            }

            // Let the UI fade out
            yield return new WaitForSeconds ( _messageDisplay.FadeDuration );

            GetComponent<SceneLoader>().LoadScene();
        }
    }
}
