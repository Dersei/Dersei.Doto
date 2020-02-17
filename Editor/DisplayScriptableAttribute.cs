using UnityEngine;

namespace Dersei.Doto.Editor
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class DisplayScriptableAttribute : PropertyAttribute
    {
        public readonly bool AllowCreation;
        public readonly bool DisplayScript;
        public readonly bool DisableEditing;

        public DisplayScriptableAttribute(bool allowCreation = true, bool displayScript = false,
            bool disableEditing = false)
        {
            AllowCreation = allowCreation;
            DisplayScript = displayScript;
            DisableEditing = disableEditing;
        }
    }
}