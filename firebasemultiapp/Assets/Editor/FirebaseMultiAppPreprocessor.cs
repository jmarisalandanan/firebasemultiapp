using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace MagicSpace.Tools
{
    public class FirebaseMultiAppPreprocessor : IPreprocessBuildWithReport
    {
        private const string PLUGIN_PATH = "Plugins";
        private const string ANDROID_PLUGIN_PATH = "Android";
        private const string IOS_PLUGIN_PATH = "iOS";
        private const string FIREBASE_PATH = "Firebase";
        private const string VALUES_PATH = "res";
        private const string ANDROID_CONFIG_FILE_NAME = "google-services.json";
        private const string IOS_CONFIG_FILE_NAME = "GoogleService-Info.plist";

        private const string CONFIG_DIR = "multiapp";
        private const string DEVELOPMENT_CONFIG_PATH = "development";
        private const string RELEASE_CONFIG_PATH = "release";

        private static readonly string androidProjectConfigPath;
        private static readonly string androidValuesConfigPath;
        private static readonly string androidReleaseConfigPath;
        private static readonly string androidDevelopmentConfigPath;
        private static readonly string iosProjectConfigPath;
        private static readonly string iosReleaseConfigPath;
        private static readonly string iosDevelopmentConfigPath;

        static FirebaseMultiAppPreprocessor()
        {
            androidProjectConfigPath = Path.Combine(Application.dataPath, PLUGIN_PATH, ANDROID_PLUGIN_PATH, FIREBASE_PATH, ANDROID_CONFIG_FILE_NAME);
            androidValuesConfigPath = Path.Combine(Application.dataPath, PLUGIN_PATH, ANDROID_PLUGIN_PATH, FIREBASE_PATH, VALUES_PATH);

            var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName;
            androidReleaseConfigPath = Path.Combine(parentDirectory, CONFIG_DIR, FIREBASE_PATH, RELEASE_CONFIG_PATH, ANDROID_CONFIG_FILE_NAME);
            androidDevelopmentConfigPath = Path.Combine(parentDirectory, CONFIG_DIR, FIREBASE_PATH, DEVELOPMENT_CONFIG_PATH, ANDROID_CONFIG_FILE_NAME);

            iosProjectConfigPath = Path.Combine(Application.dataPath, PLUGIN_PATH, IOS_PLUGIN_PATH, FIREBASE_PATH, IOS_CONFIG_FILE_NAME);
            iosReleaseConfigPath = Path.Combine(parentDirectory, CONFIG_DIR, FIREBASE_PATH, RELEASE_CONFIG_PATH, IOS_CONFIG_FILE_NAME);
            iosDevelopmentConfigPath = Path.Combine(parentDirectory, CONFIG_DIR, FIREBASE_PATH, DEVELOPMENT_CONFIG_PATH, IOS_CONFIG_FILE_NAME);
        }

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            Debug.Log("Doing Firebase Preprocess");
#if FIREBASE_PROD
    #if UNITY_ANDROID
                var configSourcePath = androidReleaseConfigPath;
    #else
                var configSourcePath = iosReleaseConfigPath;
    #endif
#else
    #if UNITY_ANDROID
                var configSourcePath = androidDevelopmentConfigPath;
    #else
                var configSourcePath = iosDevelopmentConfigPath;
    #endif
#endif
            Debug.Log($"Using Firebase config from: {configSourcePath}");
            // Check if file exists on root gitlab project directory (cdh-unity/unity-build-processing)
            // first before replacing firebase configs
            if (File.Exists(configSourcePath))
            {
#if UNITY_ANDROID
                FileUtil.DeleteFileOrDirectory(androidValuesConfigPath);
                FileUtil.ReplaceFile(configSourcePath, androidProjectConfigPath);
                GenerateXmlFromGoogleServicesJson.ForceJsonUpdate();
#else
                FileUtil.ReplaceFile(configSourcePath, iosProjectConfigPath);
#endif
            }
            else
            {
                Debug.LogError($"Target firebase config {configSourcePath} does not exist");
            }
        }
    }
}
