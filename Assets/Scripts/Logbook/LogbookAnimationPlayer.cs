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
            var collectibles = GameManager.Instance.GameSettings.Collectibles;
            if (collectibles.Any(c => c.name == "Sword" && c.Collected) && collectibles.Any(c => c.name == "Skull" && c.Collected))
                buttons[0].interactable = true;

            // More added for future objects
        }

        public void PlayAnimation( int animationToPlay )
        {
            GameManager.Instance.PrepareToPlayAnimation(animationToPlay);
        }
    }
}