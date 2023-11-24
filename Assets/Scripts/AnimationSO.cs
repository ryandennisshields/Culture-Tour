using UnityEngine;
using UnityEngine.Video;

namespace GCU.CultureTour
{
    [CreateAssetMenu(fileName = "Animation", menuName = "GCU/Create Animation")]
    public class AnimationSO : ScriptableObject
    {
        public VideoClip Clip;
        [Tooltip("-1 for indefinite; 0 or 1 for play once; >1 is the number of times to loop the video.")]
        public int LoopCount = -1;
        [Tooltip("How long in seconds to loop the video for before automatically continuing. -1 for no limit.")]
        public float PlayLength = -1;
    }
}