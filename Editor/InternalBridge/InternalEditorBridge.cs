using System;
using System.Reflection;
using UnityEditor;

namespace Cubusky.Editor
{
    internal static class InternalEditorBridge
    {
        public static FieldInfo GetFieldInfoAndStaticTypeFromProperty(SerializedProperty property, out Type type) => ScriptAttributeUtility.GetFieldInfoAndStaticTypeFromProperty(property, out type);
        public static FieldInfo GetFieldInfoFromProperty(SerializedProperty property, out Type type) => ScriptAttributeUtility.GetFieldInfoFromProperty(property, out type);
    }
}
