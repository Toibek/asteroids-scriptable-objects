using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(ShipSO))]
public class ShipInspector : Editor
{
    public VisualTreeAsset UXML;
    public override VisualElement CreateInspectorGUI()
    {
        if (UXML != null)
        {
            VisualElement myInspector = new VisualElement();
            UXML.CloneTree(myInspector);

            return myInspector;
        }
        else
            return base.CreateInspectorGUI();
    }
}