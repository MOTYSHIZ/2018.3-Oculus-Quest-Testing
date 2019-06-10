using UnityEditor;

namespace Crosstales.TPS.EditorTask
{
    /// <summary>Adds the given define symbols to PlayerSettings define symbols.</summary>
    [InitializeOnLoad]
    public class CompileDefines : Common.EditorTask.BaseCompileDefines
    {
        private const string symbol = "CT_TPS";

        static CompileDefines()
        {
            addSymbolsToAllTargets(symbol);
        }
    }
}
// © 2017-2019 crosstales LLC (https://www.crosstales.com)