using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunAndJump.LevelCreator
{
    public class PaletteItem : MonoBehaviour
    {
        #region Entities
        #region Enums
#if UNITY_EDITOR
        public enum Category
        {
            Misc,
            Colectables,
            Enemies,
            Blocks
        }
#endif
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
#if UNITY_EDITOR
        [SerializeField]
        private Category _category;

        [SerializeField]
        private string _itemName;

        [SerializeField]
        private Object _inspectedObject;
#endif
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
#if UNITY_EDITOR
        public string ItemName { get { return _itemName; } }

        public object InspectedObject { get { return _inspectedObject; } }
#endif
        #endregion

        #region Constructors
        #endregion

        #region Methods
#if UNITY_EDITOR
        public Category GetCategory()
        {
            return _category;
        }
#endif
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}