using UnityEngine;

namespace GCU.CultureTour
{
    public class DebugResetPlayer : MonoBehaviour
    {
        public void ResetPlayer()
        {
            GameManager.Instance.UndiscoverAllObjects();

            var message = GameObject.FindObjectOfType<StatusMessageDisplay>();

            if ( message != null )
            {
                message.DisplayMessage("Player has been reset.");
            }
        }
    }
}
