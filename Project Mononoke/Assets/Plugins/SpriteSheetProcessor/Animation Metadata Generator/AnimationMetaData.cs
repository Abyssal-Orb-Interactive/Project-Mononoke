using Plugins.SpriteSheetProcessor.SpriteRanamer;

namespace Plugins.SpriteSheetProcessor.AnimationMetadataGenerator
{
    public class AnimationMetaData
    {
        public string Prefix;
        public string Name;
        public int FrameCount;
        public AnimationOrientation Orientation;

        public AnimationMetaData(string prefix, string name, AnimationOrientation orientation, int frameCount)
        {
            Prefix = prefix;
            Name = name;
            Orientation = orientation;
            FrameCount = frameCount;
        }

        /*private void InitializeAnimationList()
        {
            _animationsMetaData = new List<AnimationMetaData>
            {
                 new() {Prefix = "Mage", Name = "Idle", FrameCount = 6, Orientation = AnimationOrientation.SouthEast},
                new() {Prefix = "Mage", Name = "Walk", FrameCount = 6,  Orientation =  AnimationOrientation.SouthEast},
                new() {Prefix = "Mage", Name = "Ranged_Attack", FrameCount = 6, Orientation = AnimationOrientation.SouthEast},
                new() {Prefix = "Mage", Name = "Idle", FrameCount = 6, Orientation = AnimationOrientation.South},
                new() {Prefix = "Mage", Name = "Walk", FrameCount = 6,  Orientation =  AnimationOrientation.South},
                new() {Prefix = "Mage", Name = "Ranged_Attack", FrameCount = 6, Orientation = AnimationOrientation.South},
                new() {Prefix = "Mage", Name = "Idle", FrameCount = 6, Orientation = AnimationOrientation.SouthWest},
                new() {Prefix = "Mage", Name = "Walk", FrameCount = 6,  Orientation =  AnimationOrientation.SouthWest},
                new() {Prefix = "Mage", Name = "Ranged_Attack", FrameCount = 6, Orientation = AnimationOrientation.SouthWest},
                new() {Prefix = "Mage", Name = "Idle", FrameCount = 6, Orientation = AnimationOrientation.West},
                new() {Prefix = "Mage", Name = "Walk", FrameCount = 6,  Orientation =  AnimationOrientation.West},
                new() {Prefix = "Mage", Name = "Ranged_Attack", FrameCount = 6, Orientation = AnimationOrientation.West},
                new() {Prefix = "Mage", Name = "Idle", FrameCount = 6, Orientation = AnimationOrientation.NorthWest},
                new() {Prefix = "Mage", Name = "Walk", FrameCount = 6,  Orientation =  AnimationOrientation.NorthWest},
                new() {Prefix = "Mage", Name = "Ranged_Attack", FrameCount = 6, Orientation = AnimationOrientation.NorthWest},
                new() {Prefix = "Mage", Name = "Idle", FrameCount = 6, Orientation = AnimationOrientation.East},
                new() {Prefix = "Mage", Name = "Walk", FrameCount = 6,  Orientation =  AnimationOrientation.East},
                new() {Prefix = "Mage", Name = "Ranged_Attack", FrameCount = 6, Orientation = AnimationOrientation.East},
                new() {Prefix = "Mage", Name = "Idle", FrameCount = 6, Orientation = AnimationOrientation.NorthEast},
                new() {Prefix = "Mage", Name = "Walk", FrameCount = 6,  Orientation =  AnimationOrientation.NorthEast},
                new() {Prefix = "Mage", Name = "Ranged_Attack", FrameCount = 6, Orientation = AnimationOrientation.NorthEast},
                new() {Prefix = "Mage", Name = "Idle", FrameCount = 6, Orientation = AnimationOrientation.North},
                new() {Prefix = "Mage", Name = "Walk", FrameCount = 6,  Orientation =  AnimationOrientation.North},
                new() {Prefix = "Mage", Name = "Ranged_Attack", FrameCount = 6, Orientation = AnimationOrientation.North}
            };
        }*/
    }
}