using System;
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
        public override float GetHeight()
        {
            var helpBox = attribute as HelpBoxAttribute;
            var style = GUI.skin != null ? GUI.skin.GetStyle("helpbox") : null;
            var minHeight = helpBox.messageType != HelpBoxMessageType.None ? 40f : 0f;
            var height = style == null ? base.GetHeight() : style.CalcHeight(new(helpBox.text), EditorGUIUtility.currentViewWidth) + 4f;
            return MathF.Max(minHeight, height);
        }

        public override void OnGUI(Rect position)
        {
            var helpBox = attribute as HelpBoxAttribute;
            position.height -= 2f;
            EditorGUI.HelpBox(position, helpBox.text, (MessageType)helpBox.messageType);
        }
#endif
    }
}
