#if FMOD_PRESENT
using FMOD.Studio;
using FMODUnity;

namespace RCWorld.Audio
{
    internal static class FmodParameterUtility
    {
        public static void Apply(EventInstance instance, ParamRef[] parameters)
        {
            if (parameters == null)
                return;

            foreach (ParamRef parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter.Name))
                    instance.setParameterByName(parameter.Name, parameter.Value);
            }
        }
    }
}
#endif
