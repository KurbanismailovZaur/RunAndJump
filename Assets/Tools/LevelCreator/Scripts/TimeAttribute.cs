using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunAndJump.LevelCreator
{
    public class TimeAttribute : PropertyAttribute
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
        private bool _displayHours;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public bool DisplayHours { get { return _displayHours; } }
        #endregion

        #region Constructors
        public TimeAttribute(bool displayHours)
        {
            _displayHours = displayHours;
        }
        #endregion

        #region Methods
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}