using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Ashen.GameManagerWindow
{
    public class GameManagerAssetOption : A_GameManagerUnityObjectOption
    {
        protected DrawScriptableObject drawScriptableObject;
        
        [ReadOnly, NonSerialized]
        private string defaultCreatePath;
        public string DefaultCreatePath
        {
            get
            {
                if (sourcePath == null || defaultCreatePath == null)
                {
                    OnSourcePathChanged();
                }
                return defaultCreatePath; 
            }
        }
        
        [BoxGroup("Custom Path"), HideLabel]
        public bool customPath;
        [FolderPath(RequireExistingPath = true), ShowIfGroup("Custom Path/Custom Path", Condition = nameof(customPath))]
        public List<string> searchPaths;
        [ShowIfGroup("Custom Path/Custom Path", Condition = nameof(customPath))]
        public bool includeAllSubPaths;
        
        public override A_GameManagerOption Clone(GameManagerState state)
        {
            GameManagerAssetOption gameManagerAssetOption = new GameManagerAssetOption();
            gameManagerAssetOption.drawScriptableObject = new DrawScriptableObject(gameManagerAssetOption);
            gameManagerAssetOption.state = state;
            gameManagerAssetOption.searchPaths = searchPaths;
            base.Copy(gameManagerAssetOption);
            return gameManagerAssetOption;
        }

        public virtual void SetSelected(object selectedObject)
        {
            drawScriptableObject.SetSelected(selectedObject);
        }

        public override GameManagerOptionType GetGameManagerOptionType()
        {
            return GameManagerOptionType.ScriptableObjectAsset;
        }

        public override void AddTarget(List<object> targets)
        {
            targets.Add(drawScriptableObject);
        }

        public override void OnSourcePathChanged()
        {
            base.OnSourcePathChanged();
            if (sourcePath != null && File.Exists(sourcePath))
            {
                defaultCreatePath = sourcePath.Substring(0, sourcePath.LastIndexOf('/'));
            }
            else
            {
                defaultCreatePath = "Assets";
            }
        }

        public void AddAllAssetsInPath(OdinMenuTree tree)
        {
            Type type = sourceType.Type;
            if (customPath && searchPaths != null && searchPaths.Count > 0)
            {
                foreach (string path in searchPaths)
                {
                    tree.AddAllAssetsAtPath(null, path, type);
                }
            }
            else
            {
                List<UnityEngine.Object> assets = StaticUtilities.FindAssetsByType(type);
                if (assets == null || assets.Count == 0)
                {
                    tree.AddAllAssetsAtPath(null, DefaultCreatePath, type, true);
                }
                else
                {
                    string currentShortestPath = null;
                    foreach (UnityEngine.Object asset in assets)
                    {
                        if (currentShortestPath == null)
                        {
                            currentShortestPath = AssetDatabase.GetAssetPath(asset);
                            currentShortestPath = currentShortestPath.Replace('\\', '/');
                            currentShortestPath = currentShortestPath.Substring(0, currentShortestPath.LastIndexOf('/'));
                        }
                        else
                        {
                            string assetPath = AssetDatabase.GetAssetPath(asset);
                            assetPath = assetPath.Replace('\\', '/');
                            assetPath = assetPath.Substring(0, assetPath.LastIndexOf('/'));
                            currentShortestPath = GetShortestPath(currentShortestPath, assetPath);
                        }
                    }
                    tree.AddAllAssetsAtPath(null, currentShortestPath, type, true);
                }
            }
        }

        private string GetShortestPath(string path1, string path2)
        {
            if (path1 == path2)
            {
                return path1;
            }
            if (path1.Contains(path2))
            {
                return path2;
            }
            if (path2.Contains(path1))
            {
                return path1;
            }
            string finalPath = "";
            string[] pathArray1 = path1.Split('/');
            string[] pathArray2 = path2.Split('/');
            for (int x = 0; x < pathArray1.Length; x++)
            {
                string pathElement1 = pathArray1[x];
                string pathElement2 = pathArray2[x];
                if (pathElement1 != pathElement2)
                {
                    return finalPath;
                }
                finalPath += pathElement1 + "/";
            }
            return finalPath;
        }

        public override void OnDelete(ScriptableObject scriptableObject)
        {
           
        }
    }
}