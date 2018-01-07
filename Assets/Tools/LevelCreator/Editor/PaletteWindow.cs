using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RunAndJump.LevelCreator
{
    public class PaletteWindow : EditorWindow
    {
        #region Entities
        #region Enums
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
        private List<PaletteItem.Category> _categories;

        private List<string> _categoryLabels;

        private PaletteItem.Category _categorySelected;

        private string _path = "Assets/Prefabs/LevelPieces";

        private List<PaletteItem> _items;

        private Dictionary<PaletteItem.Category, List<PaletteItem>> _categorizedItems;

        private Dictionary<PaletteItem, Texture2D> _previews;

        private Vector2 _scrollPosition;

        private const float ButtonWidth = 80;

        private const float ButtonHeight = 90;
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
            if (_categories == null)
            {
                InitCategories();
            }

            if (_categorizedItems == null)
            {
                InitContent();
            }
        }

        private void InitCategories()
        {
            _categories = EditorUtils.GetListFromEnum<PaletteItem.Category>();

            _categoryLabels = new List<string>();
            foreach (PaletteItem.Category category in _categories)
            {
                _categoryLabels.Add(category.ToString());
            }
        }

        private void InitContent()
        {
            _items = EditorUtils.GetAssetsWithScript<PaletteItem>(_path);
            _categorizedItems = new Dictionary<PaletteItem.Category, List<PaletteItem>>();
            _previews = new Dictionary<PaletteItem, Texture2D>();

            foreach (PaletteItem.Category category in _categories)
            {
                _categorizedItems.Add(category, new List<PaletteItem>());
            }

            foreach (PaletteItem item in _items)
            {
                _categorizedItems[item.GetCategory()].Add(item);
            }
        }

        [MenuItem("Tools/Level Creator/Show Palette #_p")]
        private static void ShowPalette()
        {
            GetWindow<PaletteWindow>().titleContent = new GUIContent("Palette");
        }

        private void DrawTabs()
        {
            _categorySelected = (PaletteItem.Category)GUILayout.Toolbar((int)_categorySelected, _categoryLabels.ToArray());
        }

        private void DrawScroll()
        {
            if (_categorizedItems[_categorySelected].Count == 0)
            {
                EditorGUILayout.HelpBox("This category is empty!", MessageType.Info);
                return;
            }

            int rowCapacity = Mathf.FloorToInt(position.width / ButtonWidth);

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true);

            int selectionGridIndex = -1;
            selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, GetGUIContentsFromItems(), rowCapacity, GetGUIStyle());

            GetSelectedItem(selectionGridIndex);

            GUILayout.EndScrollView();
        }

        private GUIContent[] GetGUIContentsFromItems()
        {
            List<GUIContent> guiContents = new List<GUIContent>();
            if (_previews.Count == _items.Count)
            {
                int totalItems = _categorizedItems[_categorySelected].Count;
                for (int i = 0; i < totalItems; i++)
                {
                    GUIContent guiContent = new GUIContent
                    {
                        text = _categorizedItems[_categorySelected][i].ItemName,
                        image = _previews[_categorizedItems[_categorySelected][i]]
                    };

                    guiContents.Add(guiContent);
                }
            }

            return guiContents.ToArray();
        }

        private GUIStyle GetGUIStyle()
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.LowerCenter,
                imagePosition = ImagePosition.ImageAbove,
                fixedWidth = ButtonWidth,
                fixedHeight = ButtonHeight
            };

            return guiStyle;
        }

        private void GetSelectedItem(int index)
        {
            if (index != -1)
            {
                PaletteItem selectedItem = _categorizedItems[_categorySelected][index];
            }
        }

        private void GeneratePreviews()
        {
            foreach (PaletteItem item in _items)
            {
                if (!_previews.ContainsKey(item))
                {
                    Texture2D preview = AssetPreview.GetAssetPreview(item.gameObject);

                    if (preview)
                    {
                        _previews.Add(item, preview);
                    }
                }
            }
        }

        private void Update()
        {
            if (_previews.Count != _items.Count)
            {
                GeneratePreviews();
            }
        }

        private void OnGUI()
        {
            DrawTabs();
            DrawScroll();
        }
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}