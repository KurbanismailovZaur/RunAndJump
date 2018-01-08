using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RunAndJump.LevelCreator
{
    [CustomEditor(typeof(Level))]
    public class LevelInspector : Editor
    {
        #region Entities
        #region Enums
        public enum Mode
        {
            View,
            Paint,
            Edit,
            Erase
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private Level _level;

        private int _newTotalColumns;
        private int _newTotalRows;

        private SerializedObject _serializedLevel;
        private SerializedProperty _serializedTotalTime;

        private PaletteItem _itemSelected;
        private Texture2D _itemPreview;
        private LevelPiece _pieceSelected;

        private Mode _selectedMode;
        private Mode _currentMode;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void OnEnable()
        {
            _level = (Level)target;

            InitLevel();
            ResetResizeValues();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PaletteWindow.ItemSelectedEvent += PaletteWindow_ItemSelectedEvent;
        }

        private void UnsubscribeEvent()
        {
            PaletteWindow.ItemSelectedEvent -= PaletteWindow_ItemSelectedEvent;
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }

        public override void OnInspectorGUI()
        {
            DrawLevelDataGUI();
            DrawLevelSizeGUI();
            DrawPieceSelectedGUI();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_level);
            }
        }

        private void DrawLevelDataGUI()
        {
            EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);

            GUILayout.BeginVertical("Box");

            EditorGUILayout.PropertyField(_serializedTotalTime);

            _level.Gravity = EditorGUILayout.FloatField("Gravity", _level.Gravity);
            _level.Bgm = (AudioClip)EditorGUILayout.ObjectField("Bgm", _level.Bgm, typeof(AudioClip), false);
            _level.Background = (Sprite)EditorGUILayout.ObjectField("Background", _level.Background, typeof(Sprite), false);
            GUILayout.EndVertical();

            _serializedLevel.ApplyModifiedProperties();
        }

        private void DrawLevelSizeGUI()
        {
            EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal("Box");

            GUILayout.BeginVertical();
            _newTotalColumns = EditorGUILayout.IntField("Columns", Mathf.Max(1, _newTotalColumns));
            _newTotalRows = EditorGUILayout.IntField("Rows", Mathf.Max(1, _newTotalRows));

            bool oldEnabled = GUI.enabled;
            GUI.enabled = (_newTotalColumns != _level.TotalColumns || _newTotalRows != _level.TotalRows);
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            Color oldColor = GUI.color;
            GUI.color = Color.green;
            bool buttonResize = GUILayout.Button("Resize", GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f));

            if (buttonResize)
            {
                if (EditorUtility.DisplayDialog("Level Creator", "Are you sure you want to resize the level?\nThis action cannot be undone.", "Yes", "No"))
                {
                    ResizeLevel();
                }
            }

            GUI.color = oldColor;
            bool buttonReset = GUILayout.Button("Reset", GUILayout.Height(EditorGUIUtility.singleLineHeight));

            if (buttonReset)
            {
                ResetResizeValues();
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUI.enabled = oldEnabled;
        }

        private void DrawPieceSelectedGUI()
        {
            EditorGUILayout.LabelField("Piece Selected", EditorStyles.boldLabel);

            if (!_pieceSelected)
            {
                EditorGUILayout.HelpBox("No piece selected!", MessageType.Info);
            }
            else
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField(new GUIContent(_itemPreview), GUILayout.Height(40));
                EditorGUILayout.LabelField(_itemSelected.ItemName);
                EditorGUILayout.EndVertical();
            }
        }

        private void InitLevel()
        {
            _serializedLevel = new SerializedObject(_level);
            _serializedTotalTime = _serializedLevel.FindProperty("_totalTime");

            if (_level.Pieces == null || _level.Pieces.Length == 0)
            {
                Debug.Log("Initializing the Pieces array...");
                _level.Pieces = new LevelPiece[_level.TotalColumns * _level.TotalRows];
            }
        }

        private void ResetResizeValues()
        {
            _newTotalColumns = _level.TotalColumns;
            _newTotalRows = _level.TotalRows;
        }

        private void ResizeLevel()
        {
            LevelPiece[] newPieces = new LevelPiece[_newTotalColumns * _newTotalRows]; for (int col = 0; col < _level.TotalColumns; ++col)
            {
                for (int row = 0; row < _level.TotalRows; ++row)
                {
                    if (col < _newTotalColumns && row < _newTotalRows)
                    {
                        newPieces[col + row * _newTotalColumns] = _level.Pieces[col + row * _level.TotalColumns];
                    }
                    else
                    {
                        LevelPiece piece = _level.Pieces[col + row * _level.TotalColumns];

                        if (piece != null)
                        {
                            DestroyImmediate(piece.gameObject);
                        }
                    }
                }
            }

            _level.Pieces = newPieces;
            _level.TotalColumns = _newTotalColumns;
            _level.TotalRows = _newTotalRows;
        }

        private void UpdateCurrentPieceInstance(PaletteItem item, Texture2D preview)
        {
            _itemSelected = item;
            _itemPreview = preview;
            _pieceSelected = (LevelPiece)item.GetComponent<LevelPiece>();

            Repaint();
        }

        private void DrawModeGUI()
        {
            List<Mode> modes = EditorUtils.GetListFromEnum<Mode>();
            List<string> modeLabels = new List<string>();

            foreach (Mode mode in modes)
            {
                modeLabels.Add(mode.ToString());
            }

            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(10f, 10f, 360f, 40f));
            _selectedMode = (Mode)GUILayout.Toolbar((int)_currentMode, modeLabels.ToArray(), GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
            Handles.EndGUI();
        }

        private void ModeHandler()
        {
            switch (_selectedMode)
            {
                case Mode.View:
                    Tools.current = Tool.View;
                    break;
                case Mode.Paint:
                case Mode.Edit:
                case Mode.Erase:
                    Tools.current = Tool.None;
                    break;
            }

            if (_selectedMode != _currentMode)
            {
                _currentMode = _selectedMode;
            }

            SceneView.currentDrawingSceneView.in2DMode = true;
        }

        private void OnSceneGUI()
        {
            DrawModeGUI();
            ModeHandler();
        }
        #endregion

        #region Events handlers
        private void PaletteWindow_ItemSelectedEvent(PaletteItem item, Texture2D preview)
        {
            UpdateCurrentPieceInstance(item, preview);
        }
        #endregion
        #endregion
    }
}