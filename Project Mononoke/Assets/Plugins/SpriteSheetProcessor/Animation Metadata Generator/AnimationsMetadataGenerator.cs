using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.SpriteSheetProcessor.SpriteRanamer;

namespace Plugins.SpriteSheetProcessor.AnimationMetadataGenerator
{
    public class AnimationsMetadataGenerator
    {
        private readonly int _frameCount = 1;
        private readonly string _prefix = "Unknown";

        public AnimationsMetadataGenerator(int frameCount, string prefix)
        {
            _frameCount = frameCount;
            _prefix = prefix;
        }
        
        public IEnumerable<AnimationMetaData> GenerateAnimationsMetaData(List<string> animationNames, List<AnimationOrientation> orientations, TypesOfSpriteSheetOrganization organization, string prefix = null, int frameCount = -1)
        {
            prefix ??= _prefix;
            if (frameCount <= 0) frameCount = _frameCount;
            List<AnimationMetaData> metas;
            switch(organization)
            {
                case TypesOfSpriteSheetOrganization.AnimationBased:
                    metas = GenerateAnimationBasedMetas(animationNames, orientations, prefix, frameCount);
                    break;
                case TypesOfSpriteSheetOrganization.OrientationBased:
                    metas = GenerateOrientationBasedMetas(orientations, animationNames, prefix, frameCount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(organization), organization, null);
            }

            return metas;
        }

        private List<AnimationMetaData> GenerateAnimationBasedMetas(List<string> animationNames, List<AnimationOrientation> orientations, string prefix, int frameCount)
        {
            return (from name in animationNames from orientation in orientations select new AnimationMetaData(prefix, name, orientation, frameCount)).ToList();
        }
        
        private List<AnimationMetaData> GenerateOrientationBasedMetas(List<AnimationOrientation> orientations, List<string> animationNames, string prefix, int frameCount)
        {
            return (from orientation in orientations from name in animationNames select new AnimationMetaData(prefix, name, orientation, frameCount)).ToList();
        }
    }
}