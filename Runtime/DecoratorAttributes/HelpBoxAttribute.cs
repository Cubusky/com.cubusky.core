using UnityEngine;
using UnityEngine.UIElements;

namespace Cubusky
{
    public class HelpBoxAttribute : PropertyAttribute
    {
        public string text;
        public HelpBoxMessageType messageType;

        public HelpBoxAttribute(string text) : this(text, default) { }
        public HelpBoxAttribute(string text, HelpBoxMessageType messageType)
        {
            this.text = text;
            this.messageType = messageType;
        }
    }
}
