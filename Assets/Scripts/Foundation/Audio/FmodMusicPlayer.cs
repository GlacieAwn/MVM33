#if FMOD_PRESENT
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RCWorld.Audio
{
    /// <summary>
    /// Plays one Inspector-assigned FMOD music event. Author its loop region in FMOD Studio.
    /// </summary>
    public sealed class FmodMusicPlayer : MonoBehaviour
    {
        [Tooltip("Assign a music event with an authored FMOD loop region.")]
        [SerializeField] private EventReference musicEvent;
        [SerializeField] private ParamRef[] parameters = System.Array.Empty<ParamRef>();
        [SerializeField] private bool playOnStart = true;

        private EventInstance currentInstance;

        public EventReference MusicEvent => musicEvent;
        public ParamRef[] Parameters => parameters;

        private void Start()
        {
            if (playOnStart)
                PlayLooped();
        }

        private void OnDestroy()
        {
            StopCurrent();
        }

        public void PlayLooped()
        {
            if (musicEvent.IsNull)
                return;

            StopCurrent();
            currentInstance = RuntimeManager.CreateInstance(musicEvent);
            FmodParameterUtility.Apply(currentInstance, parameters);
            currentInstance.start();
        }

        public void StopCurrent()
        {
            if (!currentInstance.isValid())
                return;

            currentInstance.stop(STOP_MODE.ALLOWFADEOUT);
            currentInstance.release();
            currentInstance.clearHandle();
        }

    }
}
#endif
