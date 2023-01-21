using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(BatterySO))]
public class BatteryInspector : Editor
{
    public VisualTreeAsset UXML;
    public override VisualElement CreateInspectorGUI()
    {
        if (UXML != null)
        {
            // Create a new VisualElement to be the root the property UI
            VisualElement container = new VisualElement();
            UXML.CloneTree(container);

            // Return the finished UI
            return container;
        }
        else
            return base.CreateInspectorGUI();
    }
}
