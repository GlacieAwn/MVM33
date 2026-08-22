using FMOD.Studio;
using FMODUnity;
using UnityEngine;

    /// <summary>Inspector-configured FMOD one-shot player.</summary>
    public sealed class FmodOneShotPlayer : MonoBehaviour
    {
        [SerializeField] private EventReference eventReference;
        [SerializeField] private ParamRef[] parameters = System.Array.Empty<ParamRef>();

        public EventReference EventReference => eventReference;
        public ParamRef[] Parameters => parameters;

        public void Play()
        {
            if (eventReference.IsNull)
                return;

            EventInstance instance = RuntimeManager.CreateInstance(eventReference);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
            instance.start();
            instance.release();
        }
    }
