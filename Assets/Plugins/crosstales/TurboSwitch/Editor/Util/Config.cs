using UnityEditor;
using UnityEngine;

namespace Crosstales.TPS.Util
{
    /// <summary>Configuration for the asset.</summary>
    [InitializeOnLoad]
    public static class Config
    {

        #region Variables

        /// <summary>Enable or disable custom location for the cache.</summary>
        public static bool CUSTOM_PATH_CACHE = Constants.DEFAULT_CUSTOM_PATH_CACHE;

        /// <summary>TPS-cache path.</summary>
        private static string pathCache = Constants.DEFAULT_PATH_CACHE;
        public static string PATH_CACHE
        {
            get { return CUSTOM_PATH_CACHE && !string.IsNullOrEmpty(pathCache) ? Helper.ValidatePath(pathCache) : Constants.DEFAULT_PATH_CACHE; }
            set
            {
                if (CUSTOM_PATH_CACHE)
                {
                    string path = value.Substring(0, value.Length - Constants.CACHE_DIRNAME.Length - 1);

                    if (path.CTContains("Library"))
                    {
                        Debug.LogError("Cache path can't be inside a path containing 'Library': " + value);
                    }
                    else if (path.CTContains("Assets"))
                    {
                        Debug.LogError("Cache path can't be inside a path containing 'Assets': " + value);
                    }
                    else if (path.CTContains("ProjectSettings"))
                    {
                        Debug.LogError("Cache path can't be inside a path containing 'ProjectSettings': " + value);
                    }
                    else
                    {
                        pathCache = value;
                    }
                }
                else
                {
                    pathCache = value;
                }
            }
        }

        /// <summary>Selected VCS-system (default: 0, 0 = none, 1 = git, 2 = SVN, 3 Mercurial).</summary>
        public static int VCS = Constants.DEFAULT_VCS;

        /// <summary>Enable or disable batch mode for CLI operations.</summary>
        public static bool BATCHMODE = Constants.DEFAULT_BATCHMODE;

        /// <summary>Enable or disable quit Unity Editor for CLI operations.</summary>
        public static bool QUIT = Constants.DEFAULT_QUIT;

        /// <summary>Enable or disable graphics device in Unity Editor for CLI operations.</summary>
        public static bool NO_GRAPHICS = Constants.DEFAULT_NO_GRAPHICS;

        /// <summary>Execute static method <ClassName.MethodName> in Unity before a switch.</summary>
        public static string EXECUTE_METHOD_PRE_SWITCH = string.Empty;

        /// <summary>Execute static method <ClassName.MethodName> in Unity after a switch.</summary>
        public static string EXECUTE_METHOD = string.Empty;

        /// <summary>Enable or disable copying the 'Assets'-folder.</summary>
        public static bool COPY_ASSETS = Constants.DEFAULT_COPY_ASSETS;

        /// <summary>Enable or disable copying the 'Library'-folder.</summary>
        public static bool COPY_LIBRARY = Constants.DEFAULT_COPY_LIBRARY;

        /// <summary>Enable or disable copying the 'ProjectSettings'-folder.</summary>
        public static bool COPY_SETTINGS = Constants.DEFAULT_COPY_SETTINGS;

        /// <summary>Enable or disable deleting the 'UnityLockfile'.</summary>
        public static bool DELETE_LOCKFILE = Constants.DEFAULT_DELETE_LOCKFILE;

        /// <summary>Enable or disable the switch confirmation dialog.</summary>
        public static bool CONFIRM_SWITCH = Constants.DEFAULT_CONFIRM_SWITCH;

        /// <summary>Enable or disable debug logging for the asset.</summary>
        public static bool DEBUG = Constants.DEFAULT_DEBUG;

        /// <summary>Enable or disable update-checks for the asset.</summary>
        public static bool UPDATE_CHECK = Constants.DEFAULT_UPDATE_CHECK;

        /// <summary>Enable or disable reminder-checks for the asset.</summary>
        public static bool REMINDER_CHECK = Constants.DEFAULT_REMINDER_CHECK;

        /// <summary>Enable or disable anonymous tracing data.</summary>
        public static bool TRACER = Constants.DEFAULT_TRACER;

        /// <summary>Enable or disable the Windows platform.</summary>
        public static bool PLATFORM_WINDOWS = Constants.DEFAULT_PLATFORM_WINDOWS;

        /// <summary>Enable or disable the macOS platform.</summary>
        public static bool PLATFORM_MAC = Constants.DEFAULT_PLATFORM_MAC;

        /// <summary>Enable or disable the Linux platform.</summary>
        public static bool PLATFORM_LINUX = Constants.DEFAULT_PLATFORM_LINUX;

        /// <summary>Enable or disable the Android platform.</summary>
        public static bool PLATFORM_ANDROID = Constants.DEFAULT_PLATFORM_ANDROID;

        /// <summary>Enable or disable the iOS platform.</summary>
        public static bool PLATFORM_IOS = Constants.DEFAULT_PLATFORM_IOS;

        /// <summary>Enable or disable the WSA platform.</summary>
        public static bool PLATFORM_WSA = Constants.DEFAULT_PLATFORM_WSA;

        /// <summary>Enable or disable the WebGL platform.</summary>
        public static bool PLATFORM_WEBGL = Constants.DEFAULT_PLATFORM_WEBGL;

        /// <summary>Enable or disable the tvOS platform.</summary>
        public static bool PLATFORM_TVOS = Constants.DEFAULT_PLATFORM_TVOS;

        /// <summary>Enable or disable the PS4 platform.</summary>
        public static bool PLATFORM_PS4 = Constants.DEFAULT_PLATFORM_PS4;

        /// <summary>Enable or disable the XBoxOne platform.</summary>
        public static bool PLATFORM_XBOXONE = Constants.DEFAULT_PLATFORM_XBOXONE;
        
        /// <summary>Enable or disable the Nintendo Switch platform.</summary>
        public static bool PLATFORM_SWITCH = Constants.DEFAULT_PLATFORM_SWITCH;
#if !UNITY_2017_3_OR_NEWER
        /// <summary>Enable or disable the Tizen platform.</summary>
        public static bool PLATFORM_TIZEN = Constants.DEFAULT_PLATFORM_TIZEN;

        /// <summary>Enable or disable the SamsungTV platform.</summary>
        public static bool PLATFORM_SAMSUNGTV = Constants.DEFAULT_PLATFORM_SAMSUNGTV;
#endif
#if !UNITY_2018_2_OR_NEWER
        /// <summary>Enable or disable the WiiU platform.</summary>
        public static bool PLATFORM_WIIU = Constants.DEFAULT_PLATFORM_WIIU;

        /// <summary>Enable or disable the 3DS platform.</summary>
        public static bool PLATFORM_3DS = Constants.DEFAULT_PLATFORM_3DS;

        /// <summary>Enable or disable the PSP2 (Vita) platform.</summary>
        public static bool PLATFORM_PSP2 = Constants.DEFAULT_PLATFORM_PSP2;
#endif

        /// <summary>Architecture of the Windows platform.</summary>
        public static int ARCH_WINDOWS = Constants.DEFAULT_ARCH_WINDOWS;

        /// <summary>Architecture of the macOS platform.</summary>
        public static int ARCH_MAC = Constants.DEFAULT_ARCH_MAC;

        /// <summary>Architecture of the Linux platform.</summary>
        public static int ARCH_LINUX = Constants.DEFAULT_ARCH_LINUX;

        /// <summary>Texture format of the Android platform.</summary>
        public static int TEX_ANDROID = Constants.DEFAULT_TEX_ANDROID;

        /// <summary>Shows or hides the delete button for the cache.</summary>
        public static bool SHOW_DELETE = false;

        /// <summary>Shows or hides the column for the platform.</summary>
        public static bool SHOW_COLUMN_PLATFORM = Constants.DEFAULT_SHOW_COLUMN_PLATFORM;

        /// <summary>Shows or hides the column for the platform.</summary>
        public static bool SHOW_COLUMN_PLATFORM_LOGO = Constants.DEFAULT_SHOW_COLUMN_PLATFORM_LOGO;

        /// <summary>Shows or hides the column for the architecture.</summary>
        public static bool SHOW_COLUMN_ARCHITECTURE = Constants.DEFAULT_SHOW_COLUMN_ARCHITECTURE;

        /// <summary>Shows or hides the column for the texture format.</summary>
        public static bool SHOW_COLUMN_TEXTURE = Constants.DEFAULT_SHOW_COLUMN_TEXTURE;

        /// <summary>Shows or hides the column for the cache.</summary>
        public static bool SHOW_COLUMN_CACHE = Constants.DEFAULT_SHOW_COLUMN_CACHE;

        /// <summary>Last switch date.</summary>
        public static System.DateTime SWITCH_DATE;

        /// <summary>Last setup date.</summary>
        public static System.DateTime SETUP_DATE;

        /// <summary>Is the configuration loaded?</summary>
        public static bool isLoaded = false;

        private static string assetPath = null;
        private const string idPath = "Documentation/id/";
        private readonly static string idName = Constants.ASSET_UID + ".txt";

        #endregion


        #region Constructor

        static Config()
        {
            if (!isLoaded)
            {
                Load();

                if (DEBUG)
                    UnityEngine.Debug.Log("Config data loaded");
            }
        }

        #endregion


        #region Properties

        /// <summary>Returns the path to the asset inside the Unity project.</summary>
        /// <returns>The path to the asset inside the Unity project.</returns>
        public static string ASSET_PATH
        {
            get
            {
                if (assetPath == null)
                {
                    try
                    {
                        if (System.IO.File.Exists(Application.dataPath + Constants.DEFAULT_ASSET_PATH + idPath + idName))
                        {
                            assetPath = Constants.DEFAULT_ASSET_PATH;
                        }
                        else
                        {
                            string[] files = System.IO.Directory.GetFiles(Application.dataPath, idName, System.IO.SearchOption.AllDirectories);

                            if (files.Length > 0)
                            {
                                string name = files[0].Substring(Application.dataPath.Length);
                                assetPath = name.Substring(0, name.Length - idPath.Length - idName.Length).Replace("\\", "/");
                            }
                            else
                            {
                                Debug.LogWarning("Could not locate the asset! File not found: " + idName);
                                assetPath = Constants.DEFAULT_ASSET_PATH;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogWarning("Could not locate asset: " + ex);
                    }
                }

                return assetPath;
            }
        }

        #endregion


        #region Public static methods

        /// <summary>Resets all changable variables to their default value.</summary>
        public static void Reset()
        {
            assetPath = null;

            CUSTOM_PATH_CACHE = Constants.DEFAULT_CUSTOM_PATH_CACHE;
            pathCache = Constants.DEFAULT_PATH_CACHE;
            VCS = Constants.DEFAULT_VCS;
            BATCHMODE = Constants.DEFAULT_BATCHMODE;
            QUIT = Constants.DEFAULT_QUIT;
            NO_GRAPHICS = Constants.DEFAULT_NO_GRAPHICS;
            EXECUTE_METHOD_PRE_SWITCH = string.Empty;
            EXECUTE_METHOD = string.Empty;
            COPY_ASSETS = Constants.DEFAULT_COPY_ASSETS;
            COPY_LIBRARY = Constants.DEFAULT_COPY_LIBRARY;
            COPY_SETTINGS = Constants.DEFAULT_COPY_SETTINGS;
            DELETE_LOCKFILE = Constants.DEFAULT_DELETE_LOCKFILE;
            CONFIRM_SWITCH = Constants.DEFAULT_CONFIRM_SWITCH;

            if (!Constants.DEV_DEBUG)
                DEBUG = Constants.DEFAULT_DEBUG;

            UPDATE_CHECK = Constants.DEFAULT_UPDATE_CHECK;
            REMINDER_CHECK = Constants.DEFAULT_REMINDER_CHECK;
            TRACER = Constants.DEFAULT_TRACER;

            PLATFORM_WINDOWS = Constants.DEFAULT_PLATFORM_WINDOWS;
            PLATFORM_MAC = Constants.DEFAULT_PLATFORM_MAC;
            PLATFORM_LINUX = Constants.DEFAULT_PLATFORM_LINUX;
            PLATFORM_ANDROID = Constants.DEFAULT_PLATFORM_ANDROID;
            PLATFORM_IOS = Constants.DEFAULT_PLATFORM_IOS;
            PLATFORM_WSA = Constants.DEFAULT_PLATFORM_WSA;
            PLATFORM_WEBGL = Constants.DEFAULT_PLATFORM_WEBGL;
            PLATFORM_TVOS = Constants.DEFAULT_PLATFORM_TVOS;
            PLATFORM_PS4 = Constants.DEFAULT_PLATFORM_PS4;
            PLATFORM_XBOXONE = Constants.DEFAULT_PLATFORM_XBOXONE;
            PLATFORM_SWITCH = Constants.DEFAULT_PLATFORM_SWITCH;
#if !UNITY_2017_3_OR_NEWER
            PLATFORM_TIZEN = Constants.DEFAULT_PLATFORM_TIZEN;
            PLATFORM_SAMSUNGTV = Constants.DEFAULT_PLATFORM_SAMSUNGTV;
#endif
#if !UNITY_2018_2_OR_NEWER
            PLATFORM_WIIU = Constants.DEFAULT_PLATFORM_WIIU;
            PLATFORM_3DS = Constants.DEFAULT_PLATFORM_3DS;
            PLATFORM_PSP2 = Constants.DEFAULT_PLATFORM_PSP2;
#endif
            ARCH_WINDOWS = Constants.DEFAULT_ARCH_WINDOWS;
            ARCH_MAC = Constants.DEFAULT_ARCH_MAC;
            ARCH_LINUX = Constants.DEFAULT_ARCH_LINUX;
            TEX_ANDROID = Constants.DEFAULT_TEX_ANDROID;

            SHOW_COLUMN_PLATFORM = Constants.DEFAULT_SHOW_COLUMN_PLATFORM;
            SHOW_COLUMN_PLATFORM_LOGO = Constants.DEFAULT_SHOW_COLUMN_PLATFORM_LOGO;
            SHOW_COLUMN_ARCHITECTURE = Constants.DEFAULT_SHOW_COLUMN_ARCHITECTURE;
            SHOW_COLUMN_TEXTURE = Constants.DEFAULT_SHOW_COLUMN_TEXTURE;
            SHOW_COLUMN_CACHE = Constants.DEFAULT_SHOW_COLUMN_CACHE;
        }

        /// <summary>Loads the all changable variables.</summary>
        public static void Load()
        {
            assetPath = null;

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_CUSTOM_PATH_CACHE))
            {
                CUSTOM_PATH_CACHE = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_CUSTOM_PATH_CACHE);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PATH_CACHE))
            {
                PATH_CACHE = Common.Util.CTPlayerPrefs.GetString(Constants.KEY_PATH_CACHE);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_VCS))
            {
                VCS = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_VCS);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_BATCHMODE))
            {
                BATCHMODE = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_BATCHMODE);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_QUIT))
            {
                QUIT = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_QUIT);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_NO_GRAPHICS))
            {
                NO_GRAPHICS = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_NO_GRAPHICS);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_EXECUTE_METHOD_PRE_SWITCH))
            {
                EXECUTE_METHOD_PRE_SWITCH = Common.Util.CTPlayerPrefs.GetString(Constants.KEY_EXECUTE_METHOD_PRE_SWITCH);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_EXECUTE_METHOD))
            {
                EXECUTE_METHOD = Common.Util.CTPlayerPrefs.GetString(Constants.KEY_EXECUTE_METHOD);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_COPY_ASSETS))
            {
                COPY_ASSETS = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_COPY_ASSETS);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_COPY_LIBRARY))
            {
                COPY_LIBRARY = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_COPY_LIBRARY);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_COPY_SETTINGS))
            {
                COPY_SETTINGS = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_COPY_SETTINGS);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_DELETE_LOCKFILE))
            {
                DELETE_LOCKFILE = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_DELETE_LOCKFILE);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_CONFIRM_SWITCH))
            {
                CONFIRM_SWITCH = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_CONFIRM_SWITCH);
            }

            if (!Constants.DEV_DEBUG)
            {
                if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_DEBUG))
                {
                    DEBUG = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_DEBUG);
                }
            }
            else
            {
                DEBUG = Constants.DEV_DEBUG;
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_UPDATE_CHECK))
            {
                UPDATE_CHECK = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_UPDATE_CHECK);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_REMINDER_CHECK))
            {
                REMINDER_CHECK = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_REMINDER_CHECK);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_TRACER))
            {
                TRACER = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_TRACER);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_WINDOWS))
            {
                PLATFORM_WINDOWS = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_WINDOWS);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_MAC))
            {
                PLATFORM_MAC = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_MAC);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_LINUX))
            {
                PLATFORM_LINUX = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_LINUX);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_ANDROID))
            {
                PLATFORM_ANDROID = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_ANDROID);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_IOS))
            {
                PLATFORM_IOS = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_IOS);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_WSA))
            {
                PLATFORM_WSA = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_WSA);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_WEBGL))
            {
                PLATFORM_WEBGL = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_WEBGL);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_TVOS))
            {
                PLATFORM_TVOS = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_TVOS);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_PS4))
            {
                PLATFORM_PS4 = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_PS4);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_XBOXONE))
            {
                PLATFORM_XBOXONE = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_XBOXONE);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_SWITCH))
            {
                PLATFORM_SWITCH = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_SWITCH);
            }
#if !UNITY_2017_3_OR_NEWER
            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_TIZEN))
            {
                PLATFORM_TIZEN = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_TIZEN);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_SAMSUNGTV))
            {
                PLATFORM_SAMSUNGTV = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_SAMSUNGTV);
            }
#endif
#if !UNITY_2018_2_OR_NEWER
            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_WIIU))
            {
                PLATFORM_WIIU = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_WIIU);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_3DS))
            {
                PLATFORM_3DS = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_3DS);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_PLATFORM_PSP2))
            {
                PLATFORM_PSP2 = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_PLATFORM_PSP2);
            }
#endif
            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_ARCH_WINDOWS))
            {
                ARCH_WINDOWS = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_ARCH_WINDOWS);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_ARCH_MAC))
            {
                ARCH_MAC = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_ARCH_MAC);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_ARCH_LINUX))
            {
                ARCH_LINUX = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_ARCH_LINUX);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_TEX_ANDROID))
            {
                TEX_ANDROID = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_TEX_ANDROID);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_SHOW_COLUMN_PLATFORM))
            {
                SHOW_COLUMN_PLATFORM = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_SHOW_COLUMN_PLATFORM);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_SHOW_COLUMN_ARCHITECTURE))
            {
                SHOW_COLUMN_ARCHITECTURE = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_SHOW_COLUMN_ARCHITECTURE);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_SHOW_COLUMN_TEXTURE))
            {
                SHOW_COLUMN_TEXTURE = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_SHOW_COLUMN_TEXTURE);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_SWITCH_DATE))
            {
                SWITCH_DATE = Common.Util.CTPlayerPrefs.GetDate(Constants.KEY_SWITCH_DATE);
            }

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_SETUP_DATE))
            {
                SETUP_DATE = Common.Util.CTPlayerPrefs.GetDate(Constants.KEY_SETUP_DATE);
            }

            isLoaded = true;
        }

        /// <summary>Saves the all changable variables.</summary>
        public static void Save()
        {
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_CUSTOM_PATH_CACHE, CUSTOM_PATH_CACHE);
            Common.Util.CTPlayerPrefs.SetString(Constants.KEY_PATH_CACHE, PATH_CACHE);
            Common.Util.CTPlayerPrefs.SetInt(Constants.KEY_VCS, VCS);

            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_BATCHMODE, BATCHMODE);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_QUIT, QUIT);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_NO_GRAPHICS, NO_GRAPHICS);

            Common.Util.CTPlayerPrefs.SetString(Constants.KEY_EXECUTE_METHOD_PRE_SWITCH, EXECUTE_METHOD_PRE_SWITCH);
            Common.Util.CTPlayerPrefs.SetString(Constants.KEY_EXECUTE_METHOD, EXECUTE_METHOD);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_COPY_ASSETS, COPY_ASSETS);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_COPY_LIBRARY, COPY_LIBRARY);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_COPY_SETTINGS, COPY_SETTINGS);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_DELETE_LOCKFILE, DELETE_LOCKFILE);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_CONFIRM_SWITCH, CONFIRM_SWITCH);

            if (!Constants.DEV_DEBUG)
                Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_DEBUG, DEBUG);

            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_UPDATE_CHECK, UPDATE_CHECK);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_REMINDER_CHECK, REMINDER_CHECK);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_TRACER, TRACER);

            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_WINDOWS, PLATFORM_WINDOWS);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_MAC, PLATFORM_MAC);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_LINUX, PLATFORM_LINUX);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_ANDROID, PLATFORM_ANDROID);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_IOS, PLATFORM_IOS);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_WSA, PLATFORM_WSA);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_WEBGL, PLATFORM_WEBGL);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_TVOS, PLATFORM_TVOS);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_PS4, PLATFORM_PS4);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_XBOXONE, PLATFORM_XBOXONE);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_SWITCH, PLATFORM_SWITCH);
#if !UNITY_2017_3_OR_NEWER
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_TIZEN, PLATFORM_TIZEN);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_SAMSUNGTV, PLATFORM_SAMSUNGTV);
#endif
#if !UNITY_2018_2_OR_NEWER
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_WIIU, PLATFORM_WIIU);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_3DS, PLATFORM_3DS);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_PLATFORM_PSP2, PLATFORM_PSP2);
#endif
            Common.Util.CTPlayerPrefs.SetInt(Constants.KEY_ARCH_WINDOWS, ARCH_WINDOWS);
            Common.Util.CTPlayerPrefs.SetInt(Constants.KEY_ARCH_MAC, ARCH_MAC);
            Common.Util.CTPlayerPrefs.SetInt(Constants.KEY_ARCH_LINUX, ARCH_LINUX);
            Common.Util.CTPlayerPrefs.SetInt(Constants.KEY_TEX_ANDROID, TEX_ANDROID);

            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_SHOW_COLUMN_PLATFORM, SHOW_COLUMN_PLATFORM);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_SHOW_COLUMN_ARCHITECTURE, SHOW_COLUMN_ARCHITECTURE);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_SHOW_COLUMN_TEXTURE, SHOW_COLUMN_TEXTURE);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_SHOW_COLUMN_CACHE, SHOW_COLUMN_CACHE);

            Common.Util.CTPlayerPrefs.SetDate(Constants.KEY_SWITCH_DATE, SWITCH_DATE);
            Common.Util.CTPlayerPrefs.SetDate(Constants.KEY_SETUP_DATE, SETUP_DATE);

            Common.Util.CTPlayerPrefs.Save();
        }

        #endregion

    }
}
// © 2017-2019 crosstales LLC (https://www.crosstales.com)