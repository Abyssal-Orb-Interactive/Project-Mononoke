using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Plugins.SpriteSheetProcessor.Animations_Generator
{
    public class AnimationsClipCreator
    {
        const string PATTERN = @"^[a-zA-Z]+_[a-zA-Z-]+_[a-zA-Z-]+_[0-9]+$";
        private const string ROOT_SAVE_PATH = "Assets/Animations/Generated Clips";

        public void AnimationsClipCreate(Texture2D spriteSheet)
        {
           
            var regex = new Regex(PATTERN);
            
            var spritesheetPath = AssetDatabase.GetAssetPath(spriteSheet);
            
            var importer = AssetImporter.GetAtPath(spritesheetPath) as TextureImporter;
            if (importer == null || importer.spriteImportMode != SpriteImportMode.Multiple) return;

            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
            if (dataProvider == null) return;

            dataProvider.InitSpriteEditorDataProvider();
            var spriteRects = dataProvider.GetSpriteRects().ToArray();

            var sprites = AssetDatabase.LoadAllAssetsAtPath(spritesheetPath)
                .OfType<Sprite>()
                .ToArray();

            AnimationClip clip = null;
            var interval = 1 / 8f;
            var binding = EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite");
            List<ObjectReferenceKeyframe> keyFrames = null;

            var currentEntity = "";
            var currentOrientation = "";
            var currentAnimation = "";
            var animationFirstFrameIndex = 0;
            var currentFrame = 0;

            for (var rectIndex = 0; rectIndex < spriteRects.Length; rectIndex++)
            {
                var frameName = spriteRects[rectIndex].name;
                if (regex.IsMatch(frameName))
                {
                    var meta = frameName.Split("_");

                    if (ClipAreEmptyOrSpriteFromAnotherEntityOrAnimationOrOrientation(currentEntity, meta,
                            currentAnimation, currentOrientation, clip))
                    {
                        if (!currentEntity.Equals(meta[0]))
                        {
                            //CreateAnimationController(meta[0]);
                        }
                        
                        if (clip != null && keyFrames.Count > 0)
                        {
                            LoopAnimation(currentFrame, interval, sprites, animationFirstFrameIndex, keyFrames);
                            SaveGeneratedClip(clip, binding, keyFrames, currentEntity, currentAnimation,
                                currentOrientation);
                        }

                        animationFirstFrameIndex = rectIndex;
                        clip = CreateAnimationClip();
                        keyFrames = new List<ObjectReferenceKeyframe>();

                        currentEntity = meta[0];
                        currentAnimation = meta[1];
                        currentOrientation = meta[2];
                        currentFrame = int.Parse(meta[3]);
                        
                        var keyFrame = new ObjectReferenceKeyframe
                        {
                            time = currentFrame * interval,
                            value = sprites[rectIndex]
                        };
                        keyFrames.Add(keyFrame);
                    }
                    else
                    {
                        currentFrame = int.Parse(meta[3]);
                        var keyFrame = new ObjectReferenceKeyframe
                        {
                            time = currentFrame * interval,
                            value = sprites[rectIndex]
                        };
                        keyFrames.Add(keyFrame);

                        if (rectIndex + 1 < spriteRects.Length) continue;
                        if (clip == null || keyFrames.Count <= 0) continue;

                        LoopAnimation(currentFrame, interval, sprites, animationFirstFrameIndex, keyFrames);
                        SaveGeneratedClip(clip, binding, keyFrames, currentEntity, currentAnimation,
                            currentOrientation);
                    }
                }
                else
                {
                    Debug.LogWarning($"{frameName} is not match with pattern Prefix_AnimationName_Orientation_Frame");
                }
            }
        }

        private static void LoopAnimation(int currentFrame, float interval, IReadOnlyList<Sprite> sprites, int animationFirstFrameIndex,
            ICollection<ObjectReferenceKeyframe> keyFrames)
        {
            var keyFrame = new ObjectReferenceKeyframe
            {
                time = (currentFrame + 1) * interval,
                value = sprites[animationFirstFrameIndex]
            };
            keyFrames.Add(keyFrame);
        }

        private static bool ClipAreEmptyOrSpriteFromAnotherEntityOrAnimationOrOrientation(string currentEntity, IReadOnlyList<string> meta, string currentAnimation, string currentOrientation, AnimationClip clip)
        {
            return !currentEntity.Equals(meta[0]) || !currentAnimation.Equals(meta[1]) ||
                   !currentOrientation.Equals(meta[2]) || clip == null;
        }

        private static AnimationClip CreateAnimationClip()
        {
            var clip = new AnimationClip();
            var clipSettings = new AnimationClipSettings
            {
                loopTime = true
            };
            AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
            return clip;
        }

        private static void SaveGeneratedClip(AnimationClip clip, EditorCurveBinding binding,
            List<ObjectReferenceKeyframe> keyFrames, string currentEntity, string currentAnimation, string currentOrientation)
        {
            AnimationUtility.SetObjectReferenceCurve(clip, binding, keyFrames.ToArray());
            var clipPath =
                $"{ROOT_SAVE_PATH}/{currentEntity}/{currentAnimation}/{currentEntity}_{currentAnimation}_{currentOrientation}.anim";
            
            var ioPath = System.IO.Path.GetDirectoryName(clipPath);
            FoldersCreator.CreateNestedFoldersIfNecessary(ioPath);
            AssetDatabase.CreateAsset(clip, clipPath);
            AssetDatabase.SaveAssets();
        }
    }
}