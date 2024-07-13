using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Plugins.SpriteSheetProcessor.Animations_Controller_Generator
{
    public class AnimationsControllerCreator
    {
        public void CreateAnimationControllerAt(string path, string name)
        {
            FoldersCreator.CreateNestedFoldersIfNecessary(path);
            var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            controller.name = name;
            controller.AddParameter("Facing", AnimatorControllerParameterType.Float);
            controller.AddParameter("RunDesired", AnimatorControllerParameterType.Bool);
            //controller.CreateBlendTreeInController("stt", out var tree);
            //tree.blendType = BlendTreeType.Simple1D;
            //tree.minThreshold = 0;
            //tree.maxThreshold = 1;
            //tree.blendParameter = "Facing";
            AssetDatabase.SaveAssets();
        }
    }
}