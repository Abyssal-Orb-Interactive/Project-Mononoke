using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.SpriteSheetProcessor.AnimationMetadataGenerator;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Plugins.SpriteSheetProcessor.SpriteRanamer
{
    public class SpriteRenamerWindow : EditorWindow
    {
        private bool _showAnimations = true;
        private bool _showAnimationsNames = true;
        private bool _showAnimationsOrientations = true;
        private bool _spriteShitContainsOneEntity = true;
        private bool _allAnimationsHasSameFrames = true;
        private bool _animationsMetadataGenerated = false;
        
        Texture2D _spriteSheet = null;

        private TypesOfSpriteSheetOrganization _organization = TypesOfSpriteSheetOrganization.Unorganized;
        private TypesOfAnimationsOrientationSet _orientationSet = TypesOfAnimationsOrientationSet.One;

        private Vector2 _inAnimationsScrollPosition = Vector2.zero;
        private Vector2 _inAnimationsNamesScrollPosition = Vector2.zero;
        private Vector2 _inAnimationsOrientationsScrollPosition = Vector2.zero;
        
        private ReorderableList _animationsList = null;
        private ReorderableList _animationsNamesList = null;
        private ReorderableList _amimationsOrientationsList = null;
        
        private List<AnimationMetaData> _animationsMetaData = null;
        private List<string> _animationsNames = null;
        private List<AnimationOrientation> _animationsOrientations = null;
        
        private SpriteRenamer _spriteRenamer = null;
        private AnimationsMetadataGenerator _animationsMetadataGenerator = null;
        
        private int _frameCount = 6;
        private string _prefix = "Unknown";

        [MenuItem("Tools/Spritesheet Sprites Renamer")]
        public static void ShowWindow()
        {
            GetWindow<SpriteRenamerWindow>("Spritesheet Sprites Renamer");
        }

        private void OnEnable()
        {
            InitializeAnimationListGUI();
            InitializeAnimationsNamesListGUI();
            InitializeAnimationsOrientationsListGUI();
            _animationsMetadataGenerated = false;
        }

        private void InitializeAnimationListGUI()
        {
            _spriteRenamer = new SpriteRenamer();
            _animationsMetadataGenerator = new AnimationsMetadataGenerator(_frameCount, _prefix);
            _animationsMetaData ??= new List<AnimationMetaData>();
            
            _animationsList = new ReorderableList(_animationsMetaData, typeof(AnimationMetaData), true, true, true, true)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "Animations");
                },
                drawElementCallback = (rect, index, _, _) =>
                {
                    if (!_showAnimations) return;
                    var element = _animationsMetaData[index];
                    
                    rect.y += 2;
                    rect.height -= 4;

                    var labelWidth = 65f;
                    var fieldWidth = (rect.width - labelWidth) / 6f;
                    
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, rect.height), "Prefix");
                    element.Prefix = EditorGUI.TextField(new Rect(rect.x + labelWidth, rect.y, fieldWidth, rect.height), GUIContent.none, element.Prefix);

                    EditorGUI.LabelField(new Rect(rect.x + labelWidth + fieldWidth, rect.y, labelWidth, rect.height), "Name");
                    element.Name = EditorGUI.TextField(new Rect(rect.x + 2 * labelWidth + fieldWidth, rect.y, fieldWidth, rect.height), GUIContent.none, element.Name);
                    
                    EditorGUI.LabelField(new Rect(rect.x + 2 * (labelWidth + fieldWidth), rect.y, labelWidth, rect.height), "Frames");
                    element.FrameCount = EditorGUI.IntField(new Rect(rect.x + 3 * labelWidth + 2 * fieldWidth, rect.y, fieldWidth, rect.height), GUIContent.none, element.FrameCount);
                    
                    EditorGUI.LabelField(new Rect(rect.x + 3 * (labelWidth + fieldWidth), rect.y, labelWidth, rect.height), "Orientation");
                    element.Orientation = (AnimationOrientation)EditorGUI.EnumPopup(new Rect(rect.x + 4 * labelWidth + 3 * fieldWidth, rect.y, fieldWidth, rect.height), GUIContent.none, element.Orientation);

                    _animationsMetaData[index] = element;
                },
                onChangedCallback = _ =>
                {
                    ApplyAnimationOrder();
                },
                onAddCallback = _ =>
                {
                    
                    var metadata = new AnimationMetaData("", "New Animation", AnimationOrientation.SouthEast, 0);

                    if (_spriteShitContainsOneEntity) metadata.Prefix = _prefix;
                    if (_allAnimationsHasSameFrames) metadata.FrameCount = _frameCount;
                    
                    _animationsMetaData.Add(metadata);
                    _animationsList.list = _animationsMetaData;
                },
                onRemoveCallback = list =>
                {
                    _animationsMetaData.RemoveAt(list.index);
                    _animationsList.list = _animationsMetaData;
                },
                onReorderCallback = _ =>
                {
                    ApplyAnimationOrder();
                }
            };
        }
        
        private void ApplyAnimationOrder()
        {
            var orderedAnimations = _animationsList.list.Cast<AnimationMetaData>().ToList();
            _animationsMetaData = orderedAnimations;
            _animationsList.list = _animationsMetaData;
        }
        
        private void InitializeAnimationsNamesListGUI()
        {
            _animationsNames ??= new List<string>();
            
            _animationsNamesList = new ReorderableList(_animationsNames, typeof(string), true, true, true, true)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "Animations Names");
                },
                drawElementCallback = (rect, index, _, _) =>
                {
                    var element = _animationsNames[index];
                    
                    rect.y += 2;
                    _animationsNames[index] = EditorGUI.TextField(
                        new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        element);
                },
                onChangedCallback = _ =>
                {
                    ApplyAnimationsNamesOrder();
                },
                onAddCallback = _ =>
                {
                    _animationsNames.Add("");
                    _animationsNamesList.list = _animationsNames;
                },
                onRemoveCallback = list =>
                {
                    _animationsNames.RemoveAt(list.index);
                    _animationsNamesList.list = _animationsNames;
                },
                onReorderCallback = _ =>
                {
                    ApplyAnimationsNamesOrder();
                }
            };
        }
        
        private void ApplyAnimationsNamesOrder()
        {
            var orderedAnimationsNames = _animationsNamesList.list.Cast<string>().ToList();
            _animationsNames = orderedAnimationsNames;
            _animationsNamesList.list = _animationsNames;
        }
        
        private void InitializeAnimationsOrientationsListGUI()
        {
            _animationsOrientations ??= new List<AnimationOrientation>();
            
            _amimationsOrientationsList = new ReorderableList(_animationsOrientations, typeof(AnimationOrientation), true, true, true, true)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "Animations Orientations");
                },
                drawElementCallback = (rect, index, _, _) =>
                {
                    var element = _animationsOrientations[index];
                    
                    rect.y += 2;
                    _animationsOrientations[index] = (AnimationOrientation)EditorGUI.EnumPopup(
                        new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        element);
                },
                onChangedCallback = _ =>
                {
                    ApplyAnimationsOrientationsOrder();
                },
                onAddCallback = _ =>
                {
                    switch (_orientationSet)
                    {
                        case TypesOfAnimationsOrientationSet.One:
                            if(_animationsOrientations.Count >= 1) return;
                            break;
                        case TypesOfAnimationsOrientationSet.Four:
                            if(_animationsOrientations.Count >= 4) return;
                            break;
                        case TypesOfAnimationsOrientationSet.Eight:
                            if(_animationsOrientations.Count >= 8) return;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    _animationsOrientations.Add(AnimationOrientation.SouthEast);
                    _amimationsOrientationsList.list = _animationsOrientations;
                },
                onRemoveCallback = list =>
                {
                    _animationsOrientations.RemoveAt(list.index);
                    _amimationsOrientationsList.list = _animationsOrientations;
                },
                onReorderCallback = _ =>
                {
                    ApplyAnimationsOrientationsOrder();
                }
            };
        }
        
        private void ApplyAnimationsOrientationsOrder()
        {
            var orderedAnimationsOrientations = _amimationsOrientationsList.list.Cast<AnimationOrientation>().ToList();
            _animationsOrientations = orderedAnimationsOrientations;
            _amimationsOrientationsList.list = _animationsOrientations;
        }

        private void OnGUI()
        {
            GUILayout.Label("Rename Sprites", EditorStyles.boldLabel);
            
            GUILayout.BeginVertical();
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperCenter,
                fixedWidth = 70
            };
            GUILayout.Label("Sprite Sheet", style);
            
            _spriteSheet = (Texture2D)EditorGUILayout.ObjectField(_spriteSheet, 
                typeof(Texture2D), false, GUILayout.Width(280), GUILayout.Height(70));
 
            GUILayout.EndVertical();
            
            
            EditorGUILayout.Space(20);
            _spriteShitContainsOneEntity = GUILayout.Toggle(_spriteShitContainsOneEntity, "Sprite Shit Contains One Entity");
            if (_spriteShitContainsOneEntity)
            {
                EditorGUILayout.Space(5);
                _prefix = EditorGUILayout.TextField(_prefix);
            }
            
            EditorGUILayout.Space(10);
            _allAnimationsHasSameFrames = GUILayout.Toggle(_allAnimationsHasSameFrames, "All Animations Has Same Frames");;
            if (_allAnimationsHasSameFrames)
            {
                EditorGUILayout.Space(5);
                _frameCount = EditorGUILayout.IntField(_frameCount);
            }
            
            GUILayout.Space(10);
            _organization = (TypesOfSpriteSheetOrganization) EditorGUILayout.EnumPopup("Sprite Sheet Organization", _organization);

            if (_organization == TypesOfSpriteSheetOrganization.Unorganized || _animationsMetadataGenerated)
            {
                EditorGUILayout.Space(10);
                _inAnimationsScrollPosition = EditorGUILayout.BeginScrollView(_inAnimationsScrollPosition);

                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                _showAnimations = EditorGUILayout.Foldout(_showAnimations, "Animations");
                GUILayout.EndHorizontal();

                if (_showAnimations)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();
                    _animationsList.DoLayoutList();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }


                EditorGUILayout.EndScrollView();
            }
            else if(!_animationsMetadataGenerated)
            {
                EditorGUILayout.Space(10);
                _inAnimationsNamesScrollPosition = EditorGUILayout.BeginScrollView(_inAnimationsNamesScrollPosition);
                
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                _showAnimationsNames = EditorGUILayout.Foldout(_showAnimationsNames, "Animations Names");
                GUILayout.EndHorizontal();
                
                if (_showAnimationsNames)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();
                    _animationsNamesList.DoLayoutList();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                
                EditorGUILayout.EndScrollView();
                
                EditorGUILayout.Space(10);
                _inAnimationsOrientationsScrollPosition = EditorGUILayout.BeginScrollView(_inAnimationsOrientationsScrollPosition);
                
                EditorGUILayout.Space(10);
                _orientationSet =
                    (TypesOfAnimationsOrientationSet)EditorGUILayout.EnumPopup("Orientation Set", _orientationSet);
                
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                _showAnimationsOrientations = EditorGUILayout.Foldout(_showAnimationsOrientations, "Animations Orientations");
                GUILayout.EndHorizontal();
                
                if (_showAnimationsOrientations)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();
                    _amimationsOrientationsList.DoLayoutList();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                
                EditorGUILayout.EndScrollView();
                
                EditorGUILayout.Space();
                if (GUILayout.Button("Apply Animations Names And Orientations Order"))
                {
                    ApplyAnimationsNamesOrder();
                    ApplyAnimationsOrientationsOrder();
                    _animationsMetaData = _animationsMetadataGenerator.GenerateAnimationsMetaData(_animationsNames, _animationsOrientations, _organization).ToList();
                    _animationsList.list = _animationsMetaData;
                    ApplyAnimationOrder();
                    _animationsMetadataGenerated = true;
                }
            }

            EditorGUILayout.Space();

            if (_organization == TypesOfSpriteSheetOrganization.Unorganized || _animationsMetadataGenerated)
            {
                if (GUILayout.Button("Rename SpriteSheet Sprites"))
                {
                    _spriteRenamer.RenameSprites(_animationsMetaData, _spriteSheet);
                    _animationsMetadataGenerated = false;
                }
            }
        }
    }
}
