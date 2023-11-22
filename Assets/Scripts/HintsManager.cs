using UnityEngine;

namespace GCU.CultureTour
{
    public class HintsManager : MonoBehaviour
    {
        // This should be in a localization dictionary
        // FC 2023-11-10
        const string NO_MORE_HINTS = "No more hints";

        [SerializeField]
        string [ ] hints = new string [ 0 ];

        public int HowManyHintsHaveBeenUsed
        {
            get
            {
                if (allHintsUsed)
                {
                    return hints.Length;
                }

                return hintIndex;
            }
        }

        int hintIndex = 0;
        bool allHintsUsed = false;

        public void DispayHint()
        {
            if ( hintIndex >= hints.Length )
            {
                hintIndex = 0;

                if ( ! allHintsUsed )
                {
                    showHintInStatusMessage(NO_MORE_HINTS);
                    allHintsUsed = true;
                    return;
                }
            }

            showHintInStatusMessage(hints[hintIndex++]);
        }

        /// <summary>
        /// This is just a temporary solution.
        /// 2023-11-10
        /// </summary>
        void showHintInStatusMessage( string hint )
        {
            FindObjectOfType<StatusMessageDisplay>()?.
                DisplayMessage(hint, clearQueue: true);
        }
    }
}
