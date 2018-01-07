using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RunAndJump.LevelCreator
{
    public static class MenuItems
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
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        [MenuItem("Tools/Level Creator/New Level Scene")]
        private static void NewLevel()
        {
            EditorUtils.NewLevel();
        }
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}