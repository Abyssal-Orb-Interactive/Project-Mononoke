using UnityEngine;

namespace Source.Character.Audio
{
    [RequireComponent(typeof(AudioPlayer))]
    public class AudioTest : MonoBehaviour
    {
        private AudioPlayer _player => GetComponent<AudioPlayer>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _player.PlaySound(AudioTypes.Footstep);
            }
        }
    }
}