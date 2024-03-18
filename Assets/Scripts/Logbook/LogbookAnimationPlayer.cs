using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace GCU.CultureTour.Logbook
{
    public class LogbookAnimationPlayer : MonoBehaviour
    {
        CollectibleSO _collectble;
        [SerializeField]
        Button[] buttons;
        bool skip = false;

        public void Store( CollectibleSO collectible )
        {
            _collectble = collectible;
            UpdateButton();
        }

        public void UpdateButton()
        {
            if (_collectble.name == "Sword" && _collectble.Collected || skip == true)
            {
                skip = true;
                if (_collectble.name == "Skull" && _collectble.Collected)
                {
                    buttons[0].interactable = true;
                }
            }
            // More added for future objects
        }

        public void PlayAnimation( int animationToPlay )
        {
            GameManager.Instance.PrepareToPlayAnimation(animationToPlay);
        }
    }
}