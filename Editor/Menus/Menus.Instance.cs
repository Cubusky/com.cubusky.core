using UnityEditor;

namespace Cubusky.Editor
{
    public static partial class Menus
    {
        [MenuItem("CONTEXT/" + nameof(Instance) + "/" + nameof(Instance.SetState))]
        public static void SetState(MenuCommand command) => ((Instance)command.context).SetState();
    }
}
