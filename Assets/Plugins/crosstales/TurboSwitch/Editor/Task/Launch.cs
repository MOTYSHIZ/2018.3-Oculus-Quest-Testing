using UnityEditor;
using Crosstales.TPS.Util;

namespace Crosstales.TPS.Task
{
    /// <summary>Show the configuration window on the first launch.</summary>
    [InitializeOnLoad]
    public static class Launch
    {

        #region Constructor

        static Launch()
        {
            bool launched = EditorPrefs.GetBool(Constants.KEY_LAUNCH);

            if (!launched)
            {
                EditorIntegration.ConfigWindow.ShowWindow(3);
                EditorPrefs.SetBool(Constants.KEY_LAUNCH, true);
            }
        }

        #endregion
    }
}
// © 2017-2019 crosstales LLC (https://www.crosstales.com)