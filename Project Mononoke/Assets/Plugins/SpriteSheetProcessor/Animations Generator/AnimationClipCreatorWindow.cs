using UnityEditor;
using UnityEngine;

namespace Plugins.SpriteSheetProcessor.Animations_Generator
{
    public class AnimationClipCreatorWindow : EditorWindow
    {
        private Texture2D _spriteSheet = null;
        private readonly AnimationsClipCreator _clipCreator = new();

        [MenuItem("Tools/Clip Creator")]
        static void ShowWindow()
        {
            GetWindow<AnimationClipCreatorWindow>("Clip Creator");
        }

        private void OnGUI()
        {
            TextureField("Spritesheet", _spriteSheet);
            
            if (_spriteSheet == null)
            {
                EditorGUILayout.HelpBox("Please assign a spritesheet.", MessageType.Warning);
                return;
            }
            
            if (GUILayout.Button("Create Clips from Spritesheet"))
            {
                _clipCreator.AnimationsClipCreate(_spriteSheet);
            }
        }
        
        private void TextureField(string name, Texture2D texture)
        {
            GUILayout.BeginVertical();
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperCenter,
                fixedWidth = 70
            };
            GUILayout.Label(name, style);
 
            _spriteSheet = (Texture2D)EditorGUILayout.ObjectField(texture, 
                typeof(Texture2D), false, GUILayout.Width(280), GUILayout.Height(70));
 
            GUILayout.EndVertical();
        }
    }
}