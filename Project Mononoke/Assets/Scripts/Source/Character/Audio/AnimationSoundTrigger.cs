using UnityEngine;

namespace Source.Character.Audio
{
    public class AnimationSoundTrigger : MonoBehaviour
    {
        [SerializeField] private AudioPlayer _audioPlayer = null;

        private void OnValidate()
        {
            _audioPlayer ??= transform.parent.GetComponentInChildren<AudioPlayer>();
        }

        public void PlayFootstepSound()
        {
            _audioPlayer.PlaySound(AudioTypes.Footstep, 0.3f);
        }

        public void PlayShadowBallCastSound()
        {
            _audioPlayer.PlaySound(AudioTypes.ShadowBallCast);
        }
        
        public void PlayShadowBallHitSound()
        {
            _audioPlayer.PlaySound(AudioTypes.ShadowBallHit);
        }
    }
}