using UnityEngine;

namespace GCU.CultureTour
{
    public class DebugResetPlayer : MonoBehaviour
    {
        public void ResetPlayer()
        {

#if DEVELOPMENT_BUILD || UNITY_EDITOR

            GameManager.Instance.ClearPlayerPrefs();

            var message = GameObject.FindObjectOfType<StatusMessageDisplay>();

            if ( message != null )
            {
                message.DisplayMessage("Player has been reset.");
            }

#endif

        }
    }
}
