using UnityEngine;

namespace GCU.CultureTour
{
    public class DebugResetPlayer : MonoBehaviour
    {
        public void ResetPlayer()
        {
#if DEBUG
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
