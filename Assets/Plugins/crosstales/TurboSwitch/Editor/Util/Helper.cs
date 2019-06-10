using UnityEngine;
using UnityEditor;

namespace Crosstales.TPS.Util
{
    /// <summary>Various helper functions.</summary>
    public abstract class Helper : Common.EditorUtil.BaseEditorHelper
    {

        #region Static variables

        private static System.Collections.Generic.Dictionary<string, bool> cachedDict = new System.Collections.Generic.Dictionary<string, bool>(20);
        private static System.Collections.Generic.Dictionary<string, string> scanDict = new System.Collections.Generic.Dictionary<string, string>(20);

        private static Texture2D logo_asset;
        private static Texture2D logo_asset_small;

        private static Texture2D icon_delete_big;

        private static Texture2D logo_windows;
        private static Texture2D logo_mac;
        private static Texture2D logo_linux;
        private static Texture2D logo_ios;
        private static Texture2D logo_android;
        private static Texture2D logo_wsa;
        private static Texture2D logo_webgl;
        private static Texture2D logo_tvos;
        private static Texture2D logo_ps4;
        private static Texture2D logo_xboxone;
        private static Texture2D logo_switch;
#if !UNITY_2017_3_OR_NEWER
        private static Texture2D logo_tizen;
        private static Texture2D logo_samsungtv;
#endif
#if !UNITY_2018_2_OR_NEWER
        private static Texture2D logo_wiiu;
        private static Texture2D logo_3ds;
        private static Texture2D logo_psp;
#endif

        private static Texture2D icon_cachefull;
        private static Texture2D icon_cacheempty;

        public static bool isDeleting = false;

        #endregion


        #region Static properties

        public static Texture2D Logo_Asset
        {
            get
            {
                return loadImage(ref logo_asset, "logo_asset_pro.png");
            }
        }

        public static Texture2D Logo_Asset_Small
        {
            get
            {
                return loadImage(ref logo_asset_small, "logo_asset_small_pro.png");
            }
        }

        public static Texture2D Icon_Delete_Big
        {
            get
            {
                return loadImage(ref icon_delete_big, "icon_delete_big.png");
            }
        }

        public static Texture2D Logo_Windows
        {
            get
            {
                return loadImage(ref logo_windows, "logo_windows.png");
            }
        }

        public static Texture2D Logo_Mac
        {
            get
            {
                return loadImage(ref logo_mac, "logo_mac.png");
            }
        }

        public static Texture2D Logo_Linux
        {
            get
            {
                return loadImage(ref logo_linux, "logo_linux.png");
            }
        }

        public static Texture2D Logo_Ios
        {
            get
            {
                return loadImage(ref logo_ios, "logo_ios.png");
            }
        }

        public static Texture2D Logo_Android
        {
            get
            {
                return loadImage(ref logo_android, "logo_android.png");
            }
        }

        public static Texture2D Logo_Wsa
        {
            get
            {
                return loadImage(ref logo_wsa, "logo_wsa.png");
            }
        }

        public static Texture2D Logo_Webgl
        {
            get
            {
                return loadImage(ref logo_webgl, "logo_webgl.png");
            }
        }

        public static Texture2D Logo_Tvos
        {
            get
            {
                return loadImage(ref logo_tvos, "logo_tvos.png");
            }
        }
        public static Texture2D Logo_Ps4
        {
            get
            {
                return loadImage(ref logo_ps4, "logo_ps4.png");
            }
        }

        public static Texture2D Logo_Xboxone
        {
            get
            {
                return loadImage(ref logo_xboxone, "logo_xboxone.png");
            }
        }

        public static Texture2D Logo_Switch
        {
            get
            {
                return loadImage(ref logo_switch, "logo_switch.png");
            }
        }

#if !UNITY_2017_3_OR_NEWER
        public static Texture2D Logo_Tizen
        {
            get
            {
                return loadImage(ref logo_tizen, "logo_tizen.png");
            }
        }

        public static Texture2D Logo_Samsungtv
        {
            get
            {
                return loadImage(ref logo_samsungtv, "logo_samsungtv.png");
            }
        }
#endif
#if !UNITY_2018_2_OR_NEWER
        public static Texture2D Logo_Wiiu
        {
            get
            {
                return loadImage(ref logo_wiiu, "logo_wiiu.png");
            }
        }

        public static Texture2D Logo_3ds
        {
            get
            {
                return loadImage(ref logo_3ds, "logo_3ds.png");
            }
        }

        public static Texture2D Logo_Psp
        {
            get
            {
                return loadImage(ref logo_psp, "logo_psp.png");
            }
        }
#endif

        public static Texture2D Icon_Cachefull
        {
            get
            {
                return loadImage(ref icon_cachefull, "icon_cachefull.png");
            }
        }

        public static Texture2D Icon_Cacheempty
        {
            get
            {
                return loadImage(ref icon_cacheempty, "icon_cacheempty.png");
            }
        }

        /// <summary>Checks if the user has selected any architecture platforms.</summary>
        /// <returns>True if the user has selected any architecture platforms.</returns>
        public static bool hasActiveArchitecturePlatforms
        {
            get
            {
#if UNITY_2017_3_OR_NEWER
                return Config.PLATFORM_WINDOWS || Config.PLATFORM_LINUX;
#else
                return Config.PLATFORM_WINDOWS || Config.PLATFORM_MAC || Config.PLATFORM_LINUX;
#endif
            }
        }

        /// <summary>Checks if the user has selected any texture platforms.</summary>
        /// <returns>True if the user has selected any texture platforms.</returns>
        public static bool hasActiveTexturePlatforms
        {
            get
            {
                return Config.PLATFORM_ANDROID;
            }
        }

        /// <summary>Checks if a cache for the project exists.</summary>
        /// <returns>True if a cache for the project exists</returns>
        public static bool hasCache
        {
            get
            {
                return System.IO.Directory.Exists(Config.PATH_CACHE);
            }
        }

        /// <summary>Scans the total cache usage of TPS.</summary>
        /// <returns>Total cache usage information.</returns>
        public static string CacheInfo
        {
            get
            {
                string result = "no cache";

                string key = Config.PATH_CACHE;

                if (scanDict.ContainsKey(key))
                {
                    result = scanDict[key];
                }
                else
                {
                    if (System.IO.Directory.Exists(Config.PATH_CACHE))
                    {
                        scanDict.Add(key, "Scanning..."); //To prevent double-load

                        System.Threading.Thread worker;
                        if (isWindowsEditor)
                        {
                            worker = new System.Threading.Thread(() => scanWindows(Config.PATH_CACHE, key));
                        }
                        else
                        {
                            worker = new System.Threading.Thread(() => scanUnix(Config.PATH_CACHE, key));
                        }
                        worker.Start();
                    }
                    else
                    {
                        scanDict.Add(key, result); //To prevent double-load
                    }

                }

                return result;
            }
        }

        /// <summary>Returns if there is a platform enabled.</summary>
        /// <returns>True if there is a platform enabled.</returns>
        public static bool hasActivePlatforms
        {
            get
            {
                return Config.PLATFORM_WINDOWS ||
                Config.PLATFORM_MAC ||
                Config.PLATFORM_LINUX ||
                Config.PLATFORM_ANDROID ||
                Config.PLATFORM_IOS ||
                Config.PLATFORM_WSA ||
                Config.PLATFORM_WEBGL ||
                Config.PLATFORM_TVOS ||
                Config.PLATFORM_PS4 ||
                Config.PLATFORM_XBOXONE ||
                Config.PLATFORM_SWITCH
#if !UNITY_2017_3_OR_NEWER
            || Config.PLATFORM_TIZEN ||
                Config.PLATFORM_SAMSUNGTV
#endif
#if !UNITY_2018_2_OR_NEWER
            || Config.PLATFORM_WIIU ||
                Config.PLATFORM_3DS ||
                Config.PLATFORM_PSP2
#endif
            ;
            }
        }

        #endregion


        #region Public static methods

        /// <summary>Switches the current platform to the target.</summary>
        /// <param name="target">Target platform for the switch</param>
        /// <param name="subTarget">Texture format (Android, optional)</param>
        public static void SwitchPlatform(BuildTarget target, MobileTextureSubtarget subTarget = MobileTextureSubtarget.Generic)
        {
            string savePathExtension = getExtension(EditorUserBuildSettings.activeBuildTarget, EditorUserBuildSettings.androidBuildSubtarget);
            string loadPathExtension = getExtension(target, subTarget);

            if (!Config.CUSTOM_PATH_CACHE && Config.VCS != 0)
            {
                if (Config.VCS == 1)
                {
                    // git
                    if (System.IO.File.Exists(Constants.APPLICATION_PATH + ".gitignore"))
                    {
                        string content = System.IO.File.ReadAllText(Constants.APPLICATION_PATH + ".gitignore");

                        if (!content.Contains(Constants.CACHE_DIRNAME + "/"))
                        {
                            System.IO.File.WriteAllText(Constants.APPLICATION_PATH + ".gitignore", content.TrimEnd() + System.Environment.NewLine + Constants.CACHE_DIRNAME + "/");
                        }
                    }
                    else
                    {
                        System.IO.File.WriteAllText(Constants.APPLICATION_PATH + ".gitignore", Constants.CACHE_DIRNAME + "/");
                    }
                }
                else if (Config.VCS == 2)
                {
                    // svn
                    using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                    {
                        process.StartInfo.FileName = "svn";
                        process.StartInfo.Arguments = "propset svn: ignore " + Constants.CACHE_DIRNAME + ".";
                        process.StartInfo.WorkingDirectory = Constants.APPLICATION_PATH;
                        process.StartInfo.UseShellExecute = false;

                        try
                        {
                            process.Start();
                            process.WaitForExit(Constants.PROCESS_KILL_TIME);
                        }
                        catch (System.Exception ex)
                        {
                            string errorMessage = "Could execute svn-ignore! Please do it manually in the console: 'svn propset svn:ignore " + Constants.CACHE_DIRNAME + ".'" + System.Environment.NewLine + ex;
                            Debug.LogError(errorMessage);
                        }
                    }
                }
                else if (Config.VCS == 3)
                {
                    // mercurial
                    Debug.LogError("Mercurial currently not supported. Please add the following lines to your .hgignore: " + System.Environment.NewLine + "syntax: glob" + System.Environment.NewLine + Constants.CACHE_DIRNAME + "/**");
                }
                else
                {
                    // collab
                    if (System.IO.File.Exists(Constants.APPLICATION_PATH + ".collabignore"))
                    {
                        string content = System.IO.File.ReadAllText(Constants.APPLICATION_PATH + ".collabignore");

                        if (!content.Contains(Constants.CACHE_DIRNAME + "/"))
                        {
                            System.IO.File.WriteAllText(Constants.APPLICATION_PATH + ".collabignore", content.TrimEnd() + System.Environment.NewLine + Constants.CACHE_DIRNAME + "/");
                        }
                    }
                    else
                    {
                        System.IO.File.WriteAllText(Constants.APPLICATION_PATH + ".collabignore", Constants.CACHE_DIRNAME + "/");
                    }
                }
            }

            UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            bool success = false;
            string scriptfile = string.Empty;

            if (!string.IsNullOrEmpty(Config.EXECUTE_METHOD_PRE_SWITCH))
                InvokeMethod(Config.EXECUTE_METHOD_PRE_SWITCH.Substring(0, Config.EXECUTE_METHOD_PRE_SWITCH.LastIndexOf(".")), Config.EXECUTE_METHOD_PRE_SWITCH.Substring(Config.EXECUTE_METHOD_PRE_SWITCH.LastIndexOf(".") + 1));

            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                try
                {
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;

                    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        scriptfile = System.IO.Path.GetTempPath() + "TPS-" + System.Guid.NewGuid() + ".cmd";

                        System.IO.File.WriteAllText(scriptfile, generateWindowsScript(target, subTarget, savePathExtension, loadPathExtension));

                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = "/c start  \"\" " + '"' + scriptfile + '"';
                    }
                    else if (Application.platform == RuntimePlatform.OSXEditor)
                    {
                        scriptfile = System.IO.Path.GetTempPath() + "TPS-" + System.Guid.NewGuid() + ".sh";

                        System.IO.File.WriteAllText(scriptfile, generateMacScript(target, subTarget, savePathExtension, loadPathExtension));

                        process.StartInfo.FileName = "/bin/sh";
                        process.StartInfo.Arguments = '"' + scriptfile + "\" &";
                    }
                    else if (Application.platform == RuntimePlatform.LinuxEditor)
                    {
                        scriptfile = System.IO.Path.GetTempPath() + "TPS-" + System.Guid.NewGuid() + ".sh";

                        System.IO.File.WriteAllText(scriptfile, generateLinuxScript(target, subTarget, savePathExtension, loadPathExtension));

                        process.StartInfo.FileName = "/bin/sh";
                        process.StartInfo.Arguments = '"' + scriptfile + "\" &";
                    }
                    else
                    {
                        Debug.LogError("Unsupported Unity Editor: " + Application.platform);
                        return;
                    }

                    Config.SWITCH_DATE = System.DateTime.Now;
                    Config.Save();

                    process.Start();

                    if (isWindowsEditor)
                        process.WaitForExit(Constants.PROCESS_KILL_TIME);

                    success = true;
                }
                catch (System.Exception ex)
                {
                    string errorMessage = "Could execute TPS!" + System.Environment.NewLine + ex;
                    Debug.LogError(errorMessage);
                }
            }

            if (success)
                EditorApplication.Exit(0);
        }

        /// <summary>Scans the cache usage per platform.</summary>
        /// <param name="target">Target platform for the scan</param>
        /// <param name="subTarget">Texture format (Android, optional)</param>
        /// <returns>Cache usage information.</returns>
        public static string ScanCache(BuildTarget target, MobileTextureSubtarget subTarget = MobileTextureSubtarget.Generic)
        {
            string result = "not cached";

            if (isCached(target, subTarget))
            {
                string key = dictKey(target, subTarget);

                if (scanDict.ContainsKey(key))
                {
                    result = scanDict[key];
                }
                else
                {
                    scanDict.Add(key, result); //To prevent double-load

                    if (System.IO.Directory.Exists(cachePath(target, subTarget)))
                    {
                        System.Threading.Thread worker;
                        if (isWindowsEditor)
                        {
                            worker = new System.Threading.Thread(() => scanWindows(cachePath(target, subTarget), key));
                        }
                        else
                        {
                            worker = new System.Threading.Thread(() => scanUnix(cachePath(target, subTarget), key));
                        }
                        worker.Start();
                    }
                }
            }

            return result;
        }

        /// <summary>Checks if a platform is already cached.</summary>
        /// <param name="target">Platform to check</param>
        /// <param name="subTarget">Texture format (Android, optional)</param>
        /// <returns>True if the platform is already cached</returns>
        public static bool isCached(BuildTarget target, MobileTextureSubtarget subTarget = MobileTextureSubtarget.Generic)
        {
            string key = dictKey(target, subTarget);
            bool result = false;

            if (cachedDict.ContainsKey(key))
            {
                result = cachedDict[key];
            }
            else
            {
                result = System.IO.Directory.Exists(cachePath(target, subTarget));
                cachedDict.Add(key, result);
            }

            return result;
        }

        /// <summary>Deletes a cache for a target platform.</summary>
        /// <param name="target">Platform to delete the cache</param>
        /// <param name="subTarget">Texture format (Android, optional)</param>
        public static void DeleteCacheFromTarget(BuildTarget target, MobileTextureSubtarget subTarget = MobileTextureSubtarget.Generic)
        {
            if (!isDeleting && isCached(target, subTarget))
            {
                isDeleting = true;

                System.Threading.Thread worker = new System.Threading.Thread(() => deleteCacheFromTarget(target, subTarget));
                worker.Start();
            }
        }

        /// <summary>Delete the cache for all platforms.</summary>
        public static void DeleteCache()
        {
            if (!isDeleting && System.IO.Directory.Exists(Config.PATH_CACHE))
            {
                isDeleting = true;

                System.Threading.Thread worker = new System.Threading.Thread(() => deleteCache());
                worker.Start();
            }
        }

        /*
        /// <summary>Delete all shell-scripts after a platform switch.</summary>
        public static void DeleteAllScripts()
        {
            //INFO: currently disabled since it could interfer with running scripts!

            DirectoryInfo dir = new DirectoryInfo(Path.GetTempPath());

            try
            {
                foreach (FileInfo file in dir.GetFiles("TPS-" + Constants.ASSET_ID + "*"))
                {
                    if (Constants.DEBUG)
                        Debug.Log("Script file deleted: " + file);

                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Could not delete all script files!" + Environment.NewLine + ex);
            }
        }
        */

        /// <summary>Sets the texture format under Android.</summary>
#if UNITY_5 || UNITY_2017
        public static void SetAndroidTexture()
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android && Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_TEX_ANDROID))
            {
                MobileTextureSubtarget subTarget = MobileTextureSubtarget.Generic;

                int selectedTexture = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_TEX_ANDROID);

                if (selectedTexture == 1)
                {
                    subTarget = MobileTextureSubtarget.DXT;
                }
                else if (selectedTexture == 2)
                {
                    subTarget = MobileTextureSubtarget.PVRTC;
                }
                else if (selectedTexture == 3)
                {
                    subTarget = MobileTextureSubtarget.ATC;
                }
                else if (selectedTexture == 4)
                {
                    subTarget = MobileTextureSubtarget.ETC;
                }
                else if (selectedTexture == 5)
                {
                    subTarget = MobileTextureSubtarget.ETC2;
                }
                else if (selectedTexture == 6)
                {
                    subTarget = MobileTextureSubtarget.ASTC;
                }

                EditorUserBuildSettings.androidBuildSubtarget = subTarget;
            }
        }
#else
        public static void SetAndroidTexture()
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android && Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_TEX_ANDROID))
            {
                MobileTextureSubtarget subTarget = MobileTextureSubtarget.Generic;

                int selectedTexture = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_TEX_ANDROID);

                if (selectedTexture == 1)
                {
                    subTarget = MobileTextureSubtarget.DXT;
                }
                else if (selectedTexture == 2)
                {
                    subTarget = MobileTextureSubtarget.PVRTC;
                }
                else if (selectedTexture == 3)
                {
                    subTarget = MobileTextureSubtarget.ETC;
                }
                else if (selectedTexture == 4)
                {
                    subTarget = MobileTextureSubtarget.ETC2;
                }
                else if (selectedTexture == 5)
                {
                    subTarget = MobileTextureSubtarget.ASTC;
                }

                EditorUserBuildSettings.androidBuildSubtarget = subTarget;
            }
        }     
#endif

        #endregion


        #region Private static methods

        private static void scanWindows(string path, string key)
        {
            using (System.Diagnostics.Process scanProcess = new System.Diagnostics.Process())
            {
                string args = "/c dir * /s /a";

                if (Config.DEBUG)
                    Debug.Log("Process arguments: '" + args + "'");

                System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

                scanProcess.StartInfo.FileName = "cmd.exe";
                scanProcess.StartInfo.WorkingDirectory = path;
                scanProcess.StartInfo.Arguments = args;
                scanProcess.StartInfo.CreateNoWindow = true;
                scanProcess.StartInfo.RedirectStandardOutput = true;
                scanProcess.StartInfo.RedirectStandardError = true;
                scanProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                scanProcess.StartInfo.UseShellExecute = false;
                scanProcess.OutputDataReceived += (sender, eventArgs) =>
                {
                    result.Add(eventArgs.Data);
                };

                bool success = true;

                try
                {
                    scanProcess.Start();
                    scanProcess.BeginOutputReadLine();
                }
                catch (System.Exception ex)
                {
                    success = false;
                    Debug.LogError("Could not start the scan process!" + System.Environment.NewLine + ex);
                }

                if (success)
                {
                    do
                    {
                        System.Threading.Thread.Sleep(50);
                    } while (!scanProcess.HasExited);

                    if (scanProcess.ExitCode == 0)
                    {
                        if (Config.DEBUG)
                            Debug.LogWarning("Scan completed: " + result.Count);

                        scanDict[key] = result[result.Count - 3].Trim();
                    }
                    else
                    {
                        using (System.IO.StreamReader sr = scanProcess.StandardError)
                        {
                            Debug.LogError("Could not scan the path: " + scanProcess.ExitCode + System.Environment.NewLine + sr.ReadToEnd());
                        }

                    }
                }
            }
        }

        private static void scanUnix(string path, string key)
        {
            using (System.Diagnostics.Process scanProcess = new System.Diagnostics.Process())
            {
                string args = "-sch \"" + path + '"';

                if (Config.DEBUG)
                    Debug.Log("Process arguments: '" + args + "'");

                System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

                scanProcess.StartInfo.FileName = "du";
                scanProcess.StartInfo.Arguments = args;
                scanProcess.StartInfo.CreateNoWindow = true;
                scanProcess.StartInfo.RedirectStandardOutput = true;
                scanProcess.StartInfo.RedirectStandardError = true;
                scanProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.Default;
                scanProcess.StartInfo.UseShellExecute = false;
                scanProcess.OutputDataReceived += (sender, eventArgs) =>
                {
                    result.Add(eventArgs.Data);
                };

                bool success = true;

                try
                {
                    scanProcess.Start();
                    scanProcess.BeginOutputReadLine();
                }
                catch (System.Exception ex)
                {
                    success = false;
                    Debug.LogError("Could not start the scan process!" + System.Environment.NewLine + ex);
                }

                if (success)
                {

                    while (!scanProcess.HasExited)
                    {
                        System.Threading.Thread.Sleep(50);
                    }

                    if (scanProcess.ExitCode == 0)
                    {
                        if (Config.DEBUG)
                            Debug.LogWarning("Scan completed: " + result.Count);

                        scanDict[key] = result[result.Count - 2].Trim();
                    }
                    else
                    {
                        using (System.IO.StreamReader sr = scanProcess.StandardError)
                        {
                            Debug.LogError("Could not scan the path: " + scanProcess.ExitCode + System.Environment.NewLine + sr.ReadToEnd());
                        }
                    }
                }
            }
        }

        private static string dictKey(BuildTarget target, MobileTextureSubtarget subTarget)
        {
            return target.ToString() + subTarget.ToString();
        }

        private static string cachePath(BuildTarget target, MobileTextureSubtarget subTarget)
        {
            return ValidatePath(Config.PATH_CACHE + target.ToString() + getExtension(target, subTarget));
        }

        private static void deleteCacheFromTarget(BuildTarget target, MobileTextureSubtarget subTarget)
        {
            try
            {
                System.IO.Directory.Delete(cachePath(target, subTarget), true);

                string key = dictKey(target, subTarget);
                cachedDict.Remove(key);
                scanDict.Remove(key);
                scanDict.Remove(Config.PATH_CACHE);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Could not delete the cache for target: " + target + System.Environment.NewLine + ex);
            }

            isDeleting = false;
        }

        private static void deleteCache()
        {
            try
            {
                System.IO.Directory.Delete(Config.PATH_CACHE, true);

                cachedDict.Clear();
                scanDict.Clear();
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Could not delete the cache!" + System.Environment.NewLine + ex);
            }

            isDeleting = false;
        }


        #region Windows

        private static string generateWindowsScript(BuildTarget target, MobileTextureSubtarget subTarget, string savePathExtension, string loadPathExtension)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // setup
            sb.AppendLine("@echo off");
            sb.AppendLine("cls");

            // title
            sb.Append("title ");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" - Relaunch of ");
            sb.Append(Application.productName);
            sb.Append(" under ");
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.AppendLine(" - DO NOT CLOSE THIS WINDOW!");

            // header
            sb.AppendLine("echo ##############################################################################");
            sb.AppendLine("echo #                                                                            #");
            sb.Append("echo #  ");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" ");
            sb.Append(Constants.ASSET_VERSION);
            sb.AppendLine(" - Windows                                       #");
            sb.AppendLine("echo #  Copyright 2016-2019 by www.crosstales.com                                 #");
            sb.AppendLine("echo #                                                                            #");
            sb.AppendLine("echo #  The files will now be synchronized between the platforms.                 #");
            sb.AppendLine("echo #  This will take some time, so please be patient and DON'T CLOSE THIS       #");
            sb.AppendLine("echo #  WINDOW before the process is finished!                                    #");
            sb.AppendLine("echo #                                                                            #");
            sb.AppendLine("echo #  Unity will restart automatically after the sync.                          #");
            sb.AppendLine("echo #                                                                            #");
            sb.AppendLine("echo ##############################################################################");
            sb.AppendLine("echo " + Application.productName);
            sb.AppendLine("echo.");
            sb.AppendLine("echo.");

            // check if Unity is closed
            sb.AppendLine(":waitloop");
            sb.Append("if not exist \"");
            sb.Append(Constants.APPLICATION_PATH);
            sb.Append("Temp\\UnityLockfile\" goto waitloopend");
            sb.AppendLine();
            sb.AppendLine("echo.");
            sb.AppendLine("echo Waiting for Unity to close...");
            sb.AppendLine("timeout /t 3");

            if (Config.DELETE_LOCKFILE)
            {
                sb.Append("del \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("Temp\\UnityLockfile\" /q");
                sb.AppendLine();
            }

            sb.AppendLine("goto waitloop");
            sb.AppendLine(":waitloopend");

            // Save files
            sb.AppendLine("echo.");
            sb.AppendLine("echo ##############################################################################");
            sb.Append("echo #  Saving files from ");
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(savePathExtension);
            sb.AppendLine();
            sb.AppendLine("echo ##############################################################################");

            // Library
            if (Config.COPY_LIBRARY)
            {
                sb.Append("robocopy \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("Library\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("\\Library");
                sb.AppendLine("\" /MIR /W:3 /MT /NFL /NDL /NJH /NJS /nc /ns /np > NUL");
            }

            // ProjectSettings
            if (Config.COPY_SETTINGS)
            {
                sb.Append("robocopy \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("ProjectSettings\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("\\ProjectSettings");
                sb.AppendLine("\" /MIR /W:3 /MT /NFL /NDL /NJH /NJS /nc /ns /np > NUL");
            }

            // Assets (meta files)
            if (Config.COPY_ASSETS)
            {
                sb.Append("robocopy \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("Assets\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("\\Assets");
                sb.AppendLine("\" /MIR /W:3 /MT /NFL /NDL /NJH /NJS /nc /ns /np *.meta > NUL");
            }

            // Restore files
            sb.AppendLine("echo.");
            sb.AppendLine("echo ##############################################################################");
            sb.Append("echo #  Restoring files from ");
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.AppendLine();
            sb.AppendLine("echo ##############################################################################");

            // Library
            if (Config.COPY_LIBRARY)
            {
                sb.Append("robocopy \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("\\Library\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.AppendLine("Library\" /MIR /W:3 /MT /NFL /NDL /NJH /NJS /nc /ns /np > NUL");
            }

            // ProjectSettings
            if (Config.COPY_SETTINGS)
            {
                sb.Append("robocopy \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("\\ProjectSettings\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.AppendLine("ProjectSettings\" /MIR /W:3 /MT /NFL /NDL /NJH /NJS /nc /ns /np > NUL");
            }

            // Assets (meta files)
            if (Config.COPY_ASSETS)
            {
                sb.Append("robocopy \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("\\Assets\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.AppendLine("Assets\" /E /W:3 /MT /NFL /NDL /NJH /NJS /nc /ns /np *.meta > NUL");
            }

            // Restart Unity
            sb.AppendLine("echo.");
            sb.AppendLine("echo ##############################################################################");
            sb.AppendLine("echo #  Restarting Unity                                                          #");
            sb.AppendLine("echo ##############################################################################");
            sb.Append("start \"\" \"");
            sb.Append(Helper.ValidatePath(EditorApplication.applicationPath, false));
            sb.Append("\" -projectPath \"");
            sb.Append(Constants.APPLICATION_PATH.Substring(0, Constants.APPLICATION_PATH.Length - 1));
            sb.Append("\" -buildTarget ");
            sb.Append(getBuildNameFromBuildTarget(target));

            if (Config.BATCHMODE)
            {
                sb.Append(" -batchmode");

                if (Config.QUIT)
                    sb.Append(" -quit");

                if (Config.NO_GRAPHICS)
                    sb.Append(" -nographics");
            }

            if (!string.IsNullOrEmpty(Config.EXECUTE_METHOD))
            {
                sb.Append(" -executeMethod ");
                sb.Append(Config.EXECUTE_METHOD);
            }

            sb.AppendLine();
            sb.AppendLine("echo.");

            // check if Unity is started
            sb.AppendLine(":waitloop2");
            sb.Append("if exist \"");
            sb.Append(Constants.APPLICATION_PATH);
            sb.Append("Temp\\UnityLockfile\" goto waitloopend2");
            sb.AppendLine();
            sb.AppendLine("echo Waiting for Unity to start...");
            sb.AppendLine("timeout /t 3");
            sb.AppendLine("goto waitloop2");
            sb.AppendLine(":waitloopend2");
            sb.AppendLine("echo.");
            sb.AppendLine("echo Bye!");
            sb.AppendLine("timeout /t 1");
            sb.AppendLine("exit");

            return sb.ToString();
        }

        #endregion


        #region Mac

        private static string generateMacScript(BuildTarget target, MobileTextureSubtarget subTarget, string savePathExtension, string loadPathExtension)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // setup
            sb.AppendLine("#!/bin/bash");
            sb.AppendLine("set +v");
            sb.AppendLine("clear");

            // title
            sb.Append("title='");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" - Relaunch of ");
            sb.Append(Application.productName);
            sb.Append(" under ");
            sb.Append(target.ToString());
            sb.Append(savePathExtension);
            sb.AppendLine(" - DO NOT CLOSE THIS WINDOW!'");
            sb.AppendLine("echo -n -e \"\\033]0;$title\\007\"");

            // header
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine("echo \"¦                                                                            ¦\"");
            sb.Append("echo \"¦  ");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" ");
            sb.Append(Constants.ASSET_VERSION);
            sb.AppendLine(" - macOS                                         ¦\"");
            sb.AppendLine("echo \"¦  Copyright 2016-2019 by www.crosstales.com                                 ¦\"");
            sb.AppendLine("echo \"¦                                                                            ¦\"");
            sb.AppendLine("echo \"¦  The files will now be synchronized between the platforms.                 ¦\"");
            sb.AppendLine("echo \"¦  This will take some time, so please be patient and DON'T CLOSE THIS       ¦\"");
            sb.AppendLine("echo \"¦  WINDOW before the process is finished!                                    ¦\"");
            sb.AppendLine("echo \"¦                                                                            ¦\"");
            sb.AppendLine("echo \"¦  Unity will restart automatically after the sync.                          ¦\"");
            sb.AppendLine("echo \"¦                                                                            ¦\"");
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine("echo \"" + Application.productName + "\"");
            sb.AppendLine("echo");
            sb.AppendLine("echo");

            // check if Unity is closed
            sb.Append("while [ -f \"");
            sb.Append(Constants.APPLICATION_PATH);
            sb.Append("Temp/UnityLockfile\" ]");
            sb.AppendLine();
            sb.AppendLine("do");
            sb.AppendLine("  echo \"Waiting for Unity to close...\"");
            sb.AppendLine("  sleep 3");

            if (Config.DELETE_LOCKFILE)
            {
                sb.Append("  rm \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("Temp/UnityLockfile\"");
                sb.AppendLine();
            }

            sb.AppendLine("done");

            // Save files
            sb.AppendLine("echo");
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.Append("echo \"¦  Saving files from ");
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(loadPathExtension);
            sb.Append('"');
            sb.AppendLine();
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");

            // Library
            if (Config.COPY_LIBRARY)
            {
                sb.Append("mkdir -p \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("/Library");
                sb.Append('"');
                sb.AppendLine();
                sb.Append("rsync -aq --delete \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("Library\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.AppendLine("/\"");
            }

            // ProjectSettings
            if (Config.COPY_SETTINGS)
            {
                sb.Append("mkdir -p \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("/ProjectSettings");
                sb.Append('"');
                sb.AppendLine();
                sb.Append("rsync -aq --delete \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("ProjectSettings\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.AppendLine("/\"");
            }

            // Assets (meta files)
            if (Config.COPY_ASSETS) //TODO test it!
            {
                sb.Append("mkdir -p \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("/Assets");
                sb.Append('"');
                sb.AppendLine();
                sb.Append("rsync -aq --delete --include=\"*/\" --include=\"*.meta\" --exclude=\"*\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("Assets\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.AppendLine("/\"");
            }

            // Restore files
            sb.AppendLine("echo");
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.Append("echo \"¦  Restoring files from ");
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.Append('"');
            sb.AppendLine();
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");

            // Library
            if (Config.COPY_LIBRARY)
            {
                sb.Append("rsync -aq --delete \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("/Library");
                sb.Append("/\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.AppendLine("Library/\"");
            }

            // ProjectSettings
            if (Config.COPY_SETTINGS)
            {
                sb.Append("rsync -aq --delete \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("/ProjectSettings");
                sb.Append("/\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.AppendLine("ProjectSettings/\"");
            }

            // Assets (meta files)
            if (Config.COPY_ASSETS) //TODO test it!
            {
                sb.Append("rsync -aq --include=\"*/\" --include=\"*.meta\" --exclude=\"*\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("/Assets");
                sb.Append("/\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.AppendLine("Assets/\"");
            }

            // Restart Unity
            sb.AppendLine("echo");
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine("echo \"¦  Restarting Unity                                                          ¦\"");
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.Append("open -a \"");
            sb.Append(EditorApplication.applicationPath);
            sb.Append("\" --args -projectPath \"");
            sb.Append(Constants.APPLICATION_PATH);
            sb.Append("\" -buildTarget ");
            sb.Append(getBuildNameFromBuildTarget(target));

            if (Config.BATCHMODE)
            {
                sb.Append(" -batchmode");

                if (Config.QUIT)
                    sb.Append(" -quit");

                if (Config.NO_GRAPHICS)
                    sb.Append(" -nographics");
            }

            if (!string.IsNullOrEmpty(Config.EXECUTE_METHOD))
            {
                sb.Append(" -executeMethod ");
                sb.Append(Config.EXECUTE_METHOD);
            }

            sb.AppendLine();

            //check if Unity is started
            sb.AppendLine("echo");
            sb.Append("while [ ! -f \"");
            sb.Append(Constants.APPLICATION_PATH);
            sb.Append("Temp/UnityLockfile\" ]");
            sb.AppendLine();
            sb.AppendLine("do");
            sb.AppendLine("  echo \"Waiting for Unity to start...\"");
            sb.AppendLine("  sleep 3");
            sb.AppendLine("done");
            sb.AppendLine("echo");
            sb.AppendLine("echo \"Bye!\"");
            sb.AppendLine("sleep 1");
            sb.AppendLine("exit");

            return sb.ToString();
        }

        #endregion


        #region Linux

        private static string generateLinuxScript(BuildTarget target, MobileTextureSubtarget subTarget, string savePathExtension, string loadPathExtension)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // setup
            sb.AppendLine("#!/bin/bash");
            sb.AppendLine("set +v");
            sb.AppendLine("clear");

            // title
            sb.Append("title='");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" - Relaunch of ");
            sb.Append(Application.productName);
            sb.Append(" under ");
            sb.Append(target.ToString());
            sb.Append(savePathExtension);
            sb.AppendLine(" - DO NOT CLOSE THIS WINDOW!'");
            sb.AppendLine("echo -n -e \"\\033]0;$title\\007\"");

            // header
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine("echo \"¦                                                                            ¦\"");
            sb.Append("echo \"¦  ");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" ");
            sb.Append(Constants.ASSET_VERSION);
            sb.AppendLine(" - Linux                                         ¦\"");
            sb.AppendLine("echo \"¦  Copyright 2016-2019 by www.crosstales.com                                 ¦\"");
            sb.AppendLine("echo \"¦                                                                            ¦\"");
            sb.AppendLine("echo \"¦  The files will now be synchronized between the platforms.                 ¦\"");
            sb.AppendLine("echo \"¦  This will take some time, so please be patient and DON'T CLOSE THIS       ¦\"");
            sb.AppendLine("echo \"¦  WINDOW before the process is finished!                                    ¦\"");
            sb.AppendLine("echo \"¦                                                                            ¦\"");
            sb.AppendLine("echo \"¦  Unity will restart automatically after the sync.                          ¦\"");
            sb.AppendLine("echo \"¦                                                                            ¦\"");
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine("echo \"" + Application.productName + "\"");
            sb.AppendLine("echo");
            sb.AppendLine("echo");

            // check if Unity is closed
            sb.Append("while [ -f \"");
            sb.Append(Constants.APPLICATION_PATH);
            sb.Append("Temp/UnityLockfile\" ]");
            sb.AppendLine();
            sb.AppendLine("do");
            sb.AppendLine("  echo \"Waiting for Unity to close...\"");
            sb.AppendLine("  sleep 3");

            if (Config.DELETE_LOCKFILE)
            {
                sb.Append("  rm \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("Temp/UnityLockfile\"");
                sb.AppendLine();
            }

            sb.AppendLine("done");

            // Save files
            sb.AppendLine("echo");
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.Append("echo \"¦  Saving files from ");
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(loadPathExtension);
            sb.Append('"');
            sb.AppendLine();
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");

            // Library
            if (Config.COPY_LIBRARY)
            {
                sb.Append("mkdir -p \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("/Library");
                sb.Append('"');
                sb.AppendLine();
                sb.Append("rsync -aq --delete \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("Library\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.AppendLine("/\"");
            }

            // ProjectSettings
            if (Config.COPY_SETTINGS)
            {
                sb.Append("mkdir -p \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("//ProjectSettings");
                sb.Append('"');
                sb.AppendLine();
                sb.Append("rsync -aq --delete \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("ProjectSettings\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.AppendLine("/\"");
            }

            // Assets (meta files)
            if (Config.COPY_ASSETS) //TODO test it!
            {
                sb.Append("mkdir -p \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("//Assets");
                sb.Append('"');
                sb.AppendLine();
                sb.Append("rsync -aq --delete --include=\"*/\" --include=\"*.meta\" --exclude=\"*\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.Append("Assets\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.AppendLine("/\"");
            }

            // Restore files
            sb.AppendLine("echo");
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.Append("echo \"¦  Restoring files from ");
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.Append('"');
            sb.AppendLine();
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");

            // Library
            if (Config.COPY_LIBRARY)
            {
                sb.Append("rsync -aq --delete \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("/Library");
                sb.Append("/\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.AppendLine("Library/\"");
            }

            // ProjectSettings
            if (Config.COPY_SETTINGS)
            {
                sb.Append("rsync -aq --delete \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("//ProjectSettings");
                sb.Append("/\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.AppendLine("ProjectSettings/\"");
            }

            // Assets (meta files)
            if (Config.COPY_ASSETS) //TODO test it!
            {
                sb.Append("rsync -aq --include=\"*/\" --include=\"*.meta\" --exclude=\"*\" \"");
                sb.Append(Config.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("//Assets");
                sb.Append("/\" \"");
                sb.Append(Constants.APPLICATION_PATH);
                sb.AppendLine("Assets/\"");
            }

            // Restart Unity
            sb.AppendLine("echo");
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine("echo \"¦  Restarting Unity                                                          ¦\"");
            sb.AppendLine("echo \"+----------------------------------------------------------------------------+\"");
            sb.Append('"');
            sb.Append(EditorApplication.applicationPath);
            sb.Append("\" --args -projectPath \"");
            sb.Append(Constants.APPLICATION_PATH);
            sb.Append("\" -buildTarget ");
            sb.Append(getBuildNameFromBuildTarget(target));

            if (Config.BATCHMODE)
            {
                sb.Append(" -batchmode");

                if (Config.QUIT)
                    sb.Append(" -quit");

                if (Config.NO_GRAPHICS)
                    sb.Append(" -nographics");
            }

            if (!string.IsNullOrEmpty(Config.EXECUTE_METHOD))
            {
                sb.Append(" -executeMethod ");
                sb.Append(Config.EXECUTE_METHOD);
            }

            sb.Append(" &");
            sb.AppendLine();

            // check if Unity is started
            sb.AppendLine("echo");
            sb.Append("while [ ! -f \"");
            sb.Append(Constants.APPLICATION_PATH);
            sb.Append("Temp/UnityLockfile\" ]");
            sb.AppendLine();
            sb.AppendLine("do");
            sb.AppendLine("  echo \"Waiting for Unity to start...\"");
            sb.AppendLine("  sleep 3");
            sb.AppendLine("done");
            sb.AppendLine("echo");
            sb.AppendLine("echo \"Bye!\"");
            sb.AppendLine("sleep 1");
            sb.AppendLine("exit");

            return sb.ToString();
        }

        #endregion

        private static string getExtension(BuildTarget target, MobileTextureSubtarget subTarget)
        {
            if (target == BuildTarget.Android && subTarget != MobileTextureSubtarget.Generic)
            {
                return "_" + subTarget;
            }

            return string.Empty;
        }


        /// <summary>Loads an image as Texture2D from 'Editor Default Resources'.</summary>
        /// <param name="logo">Logo to load.</param>
        /// <param name="fileName">Name of the image.</param>
        /// <returns>Image as Texture2D from 'Editor Default Resources'.</returns>
        private static Texture2D loadImage(ref Texture2D logo, string fileName)
        {
            if (logo == null)
            {
#if CT_DEVELOP
                logo = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets" + Config.ASSET_PATH + "Icons/" + fileName, typeof(Texture2D));
#else
                logo = (Texture2D)EditorGUIUtility.Load("crosstales/TurboSwitch/" + fileName);
#endif

                if (logo == null)
                {
                    Debug.LogWarning("Image not found: " + fileName);
                }
            }

            return logo;
        }

        #endregion
    }
}
// © 2016-2019 crosstales LLC (https://www.crosstales.com)