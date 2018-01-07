using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

namespace RunAndJump.LevelCreator
{
    public static class EditorUtils
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
        public static void NewScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        public static void CleanScene()
        {
            GameObject[] gameObjects = Object.FindObjectsOfType<GameObject>();

            foreach (GameObject gameObject in gameObjects)
            {
                Object.Destroy(gameObject);
            }
        }

        public static void NewLevel()
        {
            NewScene();
            new GameObject("Level").AddComponent<Level>();
        }

        public static List<T> GetAssetsWithScript<T>(string path) where T : MonoBehaviour
        {
            List<T> assetList = new List<T>();

            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { path });
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T component = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath).GetComponent<T>();

                if (component)
                {
                    assetList.Add(component);
                }
            }

            return assetList;
        }

        public static List<T> GetListFromEnum<T>()
        {
            List<T> enumList = new List<T>();
            System.Array enums = System.Enum.GetValues(typeof(T));

            foreach (T e in enums)
            {
                enumList.Add(e);
            }

            return enumList;
        }
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}