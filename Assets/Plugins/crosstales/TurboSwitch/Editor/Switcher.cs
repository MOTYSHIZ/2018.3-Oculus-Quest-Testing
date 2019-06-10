using UnityEngine;
using UnityEditor;
using Crosstales.TPS.Util;

namespace Crosstales.TPS
{
    /// <summary>Platform switcher.</summary>
    public static class Switcher
    {
        /// <summary>The current switch target.</summary>
        public static BuildTarget CurrentSwitchTarget = BuildTarget.NoTarget;

        /// <summary>Switches the current platform to the target via CLI.</summary>
        public static void SwitchCLI()
        {
            Switch(Helper.getCLIArgument("-tpsBuild"), Helper.getCLIArgument("-tpsExecuteMethod"), ("true".CTEquals(Helper.getCLIArgument("-tpsBatchmode")) ? true : false), ("false".CTEquals(Helper.getCLIArgument("-tpsQuit")) ? false : true), ("true".CTEquals(Helper.getCLIArgument("-tpsNoGraphics")) ? true : false), ("true".CTEquals(Helper.getCLIArgument("-tpsCopySettings")) ? true : false));
        }

        /// <summary>Switches the current platform to the target.</summary>
        /// <param name="build">Build type name for Unity, like 'win64'</param>
        /// <param name="executeMethod">Execute method after switch (optional)</param>
        /// <param name="batchmode">Start Unity in batch-mode (default: false, optional)</param>
        /// <param name="quit">Quit Unity in batch-mode (default: true, optional)</param>
        /// <param name="noGraphics">Disable graphic devices in batch-mode (default: false, optional)</param>
        /// <param name="copySettings">Copy the project settings (default: false, optional)</param>
        public static void Switch(string build, string executeMethod = "", bool batchmode = false, bool quit = true, bool noGraphics = false, bool copySettings = false)
        {
            Config.EXECUTE_METHOD = executeMethod;
            Config.BATCHMODE = batchmode;
            Config.QUIT = quit;
            Config.NO_GRAPHICS = noGraphics;
            Config.COPY_SETTINGS = copySettings;

            Switch(Helper.getBuildTargetForBuildName(build));
        }

        /// <summary>Switches the current platform to the target.</summary>
        /// <param name="target">Target platform for the switch</param>
        /// <param name="subTarget">Texture format (Android, optional)</param>
        public static void Switch(BuildTarget target, MobileTextureSubtarget subTarget = MobileTextureSubtarget.Generic)
        {
            CurrentSwitchTarget = target;

            if (target == EditorUserBuildSettings.activeBuildTarget) //ignore switch
            {
                Debug.LogWarning("Target platform is equals the current platform - switch ignored.");

                if (!string.IsNullOrEmpty(Config.EXECUTE_METHOD))
                {
                    string[] parts = Config.EXECUTE_METHOD.Split('.');

                    if (parts.Length > 1)
                    {
                        string className = parts[0];

                        for (int ii = 1; ii < parts.Length - 1; ii++)
                        {
                            className += "." + parts[ii];
                        }

                        System.Type type = System.Type.GetType(className);
                        System.Reflection.MethodInfo method = type.GetMethod(parts[parts.Length - 1], System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                        method.Invoke(null, null);
                    }
                    else
                    {
                        Debug.LogWarning("Execute method is invalid!");
                    }
                }
#if UNITY_2018_2_OR_NEWER
                if (Application.isBatchMode)
#else
                if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
#endif
                    throw new System.Exception("Target platform is equals the current platform - switch ignored.");
                    //EditorApplication.Exit(0);
            }
            else
            {
                Helper.SwitchPlatform(target, subTarget);
            }

            CurrentSwitchTarget = BuildTarget.NoTarget;
        }

        /// <summary>Test switching with an execute method.</summary>
        public static void SayHello()
        {
            Debug.LogError("Hello everybody, I was called by  " + Constants.ASSET_NAME);

            if (Config.DEBUG)
                Debug.Log("CurrentSwitchTarget: " + CurrentSwitchTarget);
        }
    }
}
// © 2018-2019 crosstales LLC (https://www.crosstales.com)