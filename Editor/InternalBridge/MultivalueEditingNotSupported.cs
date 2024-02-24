using UnityEngine.UIElements;

namespace Cubusky.Editor
{
    internal class MultivalueEditingNotSupported : TextField
    {
        public MultivalueEditingNotSupported() : this(string.Empty) { }
        public MultivalueEditingNotSupported(string label) : base(label)
        {
            value = "Multi-value editing not supported.";
            AddToClassList(alignedFieldUssClassName);
            SetEnabled(false);

            textInputBase.style.marginTop = textInputBase.resolvedStyle.marginTop;
            textInputBase.style.marginRight = textInputBase.resolvedStyle.marginRight;
            textInputBase.style.marginBottom = textInputBase.resolvedStyle.marginBottom;
            textInputBase.style.marginLeft = textInputBase.resolvedStyle.marginLeft;
            textInputBase.AddToClassList(HelpBox.ussClassName);
        }
    }
}
