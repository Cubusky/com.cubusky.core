using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : DecoratorDrawer
    {
#if UNITY_2022_3_OR_NEWER
        public override VisualElement CreatePropertyGUI()
        {
            var helpBox = attribute as HelpBoxAttribute;
            return new HelpBox(helpBox.text, helpBox.messageType);
        }
#else
        public override void OnGUI(Rect position)
        {
            var helpBox = attribute as HelpBoxAttribute;
            EditorGUI.HelpBox(position, helpBox.text, (MessageType)helpBox.messageType);
        }
#endif
    }
}
