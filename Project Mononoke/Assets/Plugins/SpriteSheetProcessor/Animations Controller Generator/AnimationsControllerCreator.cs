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
            AssetDatabase.SaveAssets();
        }
    }
}