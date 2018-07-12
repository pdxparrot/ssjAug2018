using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace pdxpartyparrot.Core.Rendering
{
    public static class PostProcessingExtensions
    {
        public static PostProcessProfile Clone(this PostProcessProfile profile)
        {
            PostProcessProfile profileClone = Object.Instantiate(profile);

            profileClone.settings.Clear();
            foreach(PostProcessEffectSettings settings in profile.settings) {
                PostProcessEffectSettings settingsClone = Object.Instantiate(settings);
                profileClone.settings.Add(settingsClone);
            }

            return profileClone;
        }

        public static void Destroy(this PostProcessProfile profile)
        {
            foreach(PostProcessEffectSettings settings in profile.settings) {
                Object.Destroy(settings);
            }
            Object.Destroy(profile);
        }
    }
}
