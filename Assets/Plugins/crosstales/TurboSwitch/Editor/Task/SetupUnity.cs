using UnityEditor;
using Crosstales.TPS.Util;

namespace Crosstales.TPS.Task
{
    /// <summary>Setup Unity after a switch.</summary>
    [InitializeOnLoad]
    public static class SetupUnity
    {

        #region Constructor

        static SetupUnity()
        {
            if (Config.SETUP_DATE < Config.SWITCH_DATE)
            {
                Helper.SetAndroidTexture();
                Helper.RefreshAssetDatabase();

                Config.SETUP_DATE = System.DateTime.Now;
                Config.Save();
            }
        }

        #endregion
    }
}
// © 2019 crosstales LLC (https://www.crosstales.com)