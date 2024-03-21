using UnityEditor;
using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : DecoratorDrawer
    {
        public override VisualElement CreatePropertyGUI()
        {
            var helpBox = attribute as HelpBoxAttribute;
            return new HelpBox(helpBox.text, helpBox.messageType);
        }
    }
}
