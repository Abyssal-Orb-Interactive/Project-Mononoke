using UnityEditor;

namespace Plugins.SpriteSheetProcessor
{
    public static class FoldersCreator
    {
        public static void CreateNestedFoldersIfNecessary(string assetPath)
        {
            var ioPath = System.IO.Path.GetDirectoryName(assetPath);
            if (ioPath == null) return;
            
            var folders = ioPath.Split('/', '\\');
    
            var currentPath = "Assets";
            for (var i = 1; i < folders.Length; i++)
            {
                var folderName = folders[i];
                var newPath = System.IO.Path.Combine(currentPath, folderName);

                if (!AssetDatabase.IsValidFolder(newPath))
                {
                    AssetDatabase.CreateFolder(currentPath, folderName);
                    AssetDatabase.Refresh();
                }

                currentPath = newPath;
            }
        }
    }
}