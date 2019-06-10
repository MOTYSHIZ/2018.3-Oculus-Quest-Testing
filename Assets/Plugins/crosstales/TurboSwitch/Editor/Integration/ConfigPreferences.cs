#if !UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine;
using Crosstales.TPS.Util;

namespace Crosstales.TPS.EditorIntegration
{
    /// <summary>Unity "Preferences" extension.</summary>
    public class ConfigPreferences : ConfigBase
    {

        #region Variables

        private static int tab = 0;
        private static int lastTab = 0;
        private static ConfigPreferences cp;

        #endregion


        #region Static methods

        [PreferenceItem(Constants.ASSET_NAME_SHORT)]
        private static void PreferencesGUI()
        {
            if (cp == null) {
                cp = ScriptableObject.CreateInstance(typeof(ConfigPreferences)) as ConfigPreferences;
            }

            init();

            tab = GUILayout.Toolbar(tab, new string[] { "Switch", "Config", "Help", "About" });

            if (tab != lastTab)
            {
                lastTab = tab;
                GUI.FocusControl(null);
            }
            if (tab == 0)
            {
                cp.showSwitch();
            }
            else if (tab == 1)
            {
                cp.showConfiguration();

                Helper.SeparatorUI();

                if (GUILayout.Button(new GUIContent(" Reset", Helper.Icon_Reset, "Resets the configuration settings for this project.")))
                {
                    if (EditorUtility.DisplayDialog("Reset configuration?", "Reset the configuration of " + Constants.ASSET_NAME + "?", "Yes", "No"))
                    {
                        Config.Reset();
                        save();
                    }
                }

                GUILayout.Space(6);
            }
            else if (tab == 2)
            {
                cp.showHelp();
            }
            else
            {
                cp.showAbout();
            }

            if (GUI.changed)
            {
                save();
            }
        }

        #endregion
    }
}
#endif
// © 2016-2019 crosstales LLC (https://www.crosstales.com)