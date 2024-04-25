using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GCU.CultureTour.Logbook
{
    public class LogbookAnimationPlayer : MonoBehaviour
    {
        [SerializeField]
        Button[] buttons;

        public void Start()
        {
             var collectibles = GameManager.Instance.GameSettings.Collectibles;
            if (collectibles.Any(c => c.name == "Sword" && c.Collected) && collectibles.Any(c => c.name == "Cloak" && c.Collected))
                buttons[0].interactable = true;
            if (collectibles.Any(c => c.name == "Skull" && c.Collected) && collectibles.Any(c => c.name == "Lantern" && c.Collected))
                buttons[1].interactable = true;
            
            // More added for future objects
        }

        public void PlayAnimation( int animationToPlay )
        {
            AudioManager.Instance.PlaySoundEffect(0);
            GameManager.Instance.PrepareToPlayAnimation(animationToPlay);
        }
    }
}