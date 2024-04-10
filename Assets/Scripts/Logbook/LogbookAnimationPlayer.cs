using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
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
#if DEBUG
            var collectibles = GameManager.Instance.GameSettings.Collectibles;
            if (collectibles.Any(c => c.name == "GCU Sword" && c.Collected) && collectibles.Any(c => c.name == "GCU Skull" && c.Collected))
                buttons[0].interactable = true;
            if (collectibles.Any(c => c.name == "GCU Hammer" && c.Collected) && collectibles.Any(c => c.name == "GCU Toy Wooden Horse" && c.Collected))
                buttons[1].interactable = true;
#else
             var collectibles = GameManager.Instance.GameSettings.Collectibles;
            if (collectibles.Any(c => c.name == "Sword" && c.Collected) && collectibles.Any(c => c.name == "Skull" && c.Collected))
                buttons[0].interactable = true;
            if (collectibles.Any(c => c.name == "Hammer" && c.Collected) && collectibles.Any(c => c.name == "Toy Wooden Horse" && c.Collected))
                buttons[1].interactable = true;
#endif
            

            // More added for future objects
        }

        public void PlayAnimation( int animationToPlay )
        {
            AudioManager.Instance.PlaySoundEffect(0);
            GameManager.Instance.PrepareToPlayAnimation(animationToPlay);
        }
    }
}