using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Serbull.GameAssets.Pets
{
    public static class ConfigProvider
    {
        private const string AssetName = "PetConfig.asset";
        private const string LocalDevPath = "Assets/Serbull/Game Assets/Modules/Pets/Editor/Scriptables/" + AssetName;
        private const string PackagePath = "Packages/com.serbull.gameassets.pets/Editor/Scriptables/" + AssetName;
        private const string CopyTargetPath = "Assets/Resources/" + AssetName;
        private const string ResourcesPath = "Resources/" + AssetName;

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

            return AssetDatabase.LoadAssetAtPath<PetConfig>(CopyTargetPath);
#else
            return Resources.Load<PetConfig>(ResourcesPath);
#endif
        }
    }
}
