using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Serbull.GameAssets.Pets
{
    public static class ConfigProvider
    {
        private const string AssetName = "PetConfig";
        private const string AssetFullName = AssetName + ".asset";
        private const string LocalDevPath = "Assets/Serbull/Game Assets/Modules/Pets/Editor/Scriptables/" + AssetFullName;
        private const string PackagePath = "Packages/com.serbull.gameassets.pets/Editor/Scriptables/" + AssetFullName;
        private const string CopyTargetPath = "Assets/Resources/" + AssetName;

        public static PetConfig LoadConfig()
        {
#if UNITY_EDITOR
            if (File.Exists(LocalDevPath))
            {
                return AssetDatabase.LoadAssetAtPath<PetConfig>(LocalDevPath);
            }

            if (!File.Exists(CopyTargetPath) && File.Exists(PackagePath))
            {
                File.Copy(PackagePath, CopyTargetPath);
                AssetDatabase.Refresh();
            }
#endif
            return Resources.Load<PetConfig>(AssetName);
        }
    }
}
