using System.Collections.Generic;
using System.Linq;
using Plugins.SpriteSheetProcessor.AnimationMetadataGenerator;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Plugins.SpriteSheetProcessor.SpriteRanamer
{
    public class SpriteRenamer
    {
        public void RenameSprites(IEnumerable<AnimationMetaData> animationsMetaData, Texture2D spriteSheet)
        {
            var animationsMetaDataList = animationsMetaData.ToList();

            var path = AssetDatabase.GetAssetPath(spriteSheet);
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null || importer.spriteImportMode != SpriteImportMode.Multiple) return;

            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
            if (dataProvider == null) return;

            dataProvider.InitSpriteEditorDataProvider();
            var sprites = dataProvider.GetSpriteRects().ToArray();
            var spritesInMetadata = animationsMetaDataList.Sum(data => data.FrameCount);
            if (spritesInMetadata != sprites.Length) return;
            var frameIndex = 0;

            foreach (var animationMetaData in animationsMetaDataList)
            {
                var frameCount = animationMetaData.FrameCount;

                for (var frame = 0; frame < frameCount; frame++)
                {
                    if (frameIndex >= sprites.Length) continue;

                    var newName =
                        $"{animationMetaData.Prefix}_{animationMetaData.Name}_{animationMetaData.Orientation}_{frame}";
                    sprites[frameIndex].name = newName;
                    frameIndex++;
                }
            }

            dataProvider.SetSpriteRects(sprites);
            dataProvider.Apply();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}