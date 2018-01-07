using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RunAndJump.LevelCreator
{
    [CustomPropertyDrawer(typeof(TimeAttribute))]
    public class TimeDrawer : PropertyDrawer
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
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) * 2f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2f), label, property.intValue);
                EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2f, position.width, position.height / 2f), " ", TimeFormat(property.intValue));
            }
            else
            {
                EditorGUI.HelpBox(position, string.Format("\"{0}\" must be an integer", label.text), MessageType.Error);
            }
        }

        private string TimeFormat(int totalSeconds)
        {
            TimeAttribute time = attribute as TimeAttribute;

            if (time.DisplayHours)
            {
                int hours = totalSeconds / (60 * 60);
                int minutes = ((totalSeconds % (60 * 60)) / 60);
                int seconds = (totalSeconds % 60);

                return string.Format("{0}:{1}:{2} (h:m:s)", hours, minutes.ToString().PadLeft(2, '0'), seconds.ToString().PadLeft(2, '0'));
            }
            else
            {
                int minutes = (totalSeconds / 60);
                int seconds = (totalSeconds % 60);

                return string.Format("{0}:{1} (m:s)", minutes.ToString(), seconds.ToString().PadLeft(2, '0'));
            }
        }
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}