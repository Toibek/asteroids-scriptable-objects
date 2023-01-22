using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using System.Linq;

public class ItemDatabase : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset visualTree = default;
    [SerializeField]
    private StyleSheet styleSheet;
    [SerializeField]
    private VisualTreeAsset m_ItemRowTemplate;
    [Space]
    [SerializeField] private Sprite m_DefaultItemIcon;
    private ListView m_ItemListView;
    private float m_ItemHeight = 40;
    private VisualElement m_ItemsTab;

    private static List<ShipSO> ships = new();
    private static List<EngineSO> engines = new();
    private static List<WeaponSO> weapons = new();
    private static List<BatterySO> batteries = new();

    private VisualElement m_DetailSection;
    private VisualElement m_ShipSection;
    private VisualElement m_EngineSection;
    private VisualElement m_WeaponSection;
    private VisualElement m_BatterySection;
    private VisualElement m_LargeDisplayIcon;

    private ShipSO m_activeShip;

    [MenuItem("Tools/Item Database")]
    public static void ShowExample()
    {
        ItemDatabase wnd = GetWindow<ItemDatabase>();
        wnd.titleContent = new GUIContent("Item Database");

        Vector2 size = new Vector2(800, 475);
        wnd.minSize = size;
        wnd.maxSize = size;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        LoadAllItems();

        m_ItemsTab = rootVisualElement.Q<VisualElement>("ItemsTab");
        GenerateListView();

        m_DetailSection = rootVisualElement.Q<VisualElement>("Details");
        m_ShipSection = rootVisualElement.Q<VisualElement>("ShipDetails");
        m_EngineSection = rootVisualElement.Q<VisualElement>("EngineDetails");
        m_WeaponSection = rootVisualElement.Q<VisualElement>("WeaponDetails");
        m_BatterySection = rootVisualElement.Q<VisualElement>("BatteryDetails");
        m_DetailSection.style.visibility = Visibility.Hidden;
        m_EngineSection.style.visibility = Visibility.Hidden;
        m_WeaponSection.style.visibility = Visibility.Hidden;
        m_BatterySection.style.visibility = Visibility.Hidden;
        m_LargeDisplayIcon = m_ShipSection.Q<VisualElement>("Sprite");

        rootVisualElement.Q<Button>("NewShip").clicked += AddItem_OnClick;

    }
    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => m_ItemRowTemplate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            e.Q<VisualElement>("Icon").style.backgroundImage =
                ships[i] == null ? m_DefaultItemIcon.texture :
                ships[i].ShipSprite.texture;
            e.Q<Label>("Name").text = ships[i].Name;
        };
        m_ItemListView = new ListView(ships, 35, makeItem, bindItem);
        m_ItemListView.selectionType = SelectionType.Single;
        m_ItemListView.style.height = ships.Count * m_ItemHeight;
        m_ItemsTab.Add(m_ItemListView);

        m_ItemListView.onSelectionChange += ListView_onSelectionChange;
    }
    private void AddItem_OnClick()
    {
        //Create an instance of the scriptable object and set the default parameters
        ShipSO newItem = CreateInstance<ShipSO>();
        newItem.Name = $"New Item";
        newItem.ShipSprite = m_DefaultItemIcon;
        //Create the asset, using the unique ID for the name
        AssetDatabase.CreateAsset(newItem, $"Assets/Data/Ships/{newItem.ID}.asset");
        //Add it to the item list
        ships.Add(newItem);
        //Refresh the ListView so everything is redrawn again
        m_ItemListView.Rebuild();
        m_ItemListView.style.height = ships.Count * m_ItemHeight;
    }

    private void ListView_onSelectionChange(IEnumerable<object> selectedItems)
    {
        m_activeShip = (ShipSO)selectedItems.First();
        SerializedObject ss = new SerializedObject(m_activeShip);
        
        m_ShipSection.Bind(ss);
        if (m_activeShip.ShipSprite != null)
        {
            m_LargeDisplayIcon.style.backgroundImage = m_activeShip.ShipSprite.texture;
        }
        m_DetailSection.style.visibility = Visibility.Visible;

        rootVisualElement.Q<Slider>("Mass").RegisterValueChangedCallback(v =>
        {
            m_activeShip.OnMassChanged?.Invoke();
        });

        if (m_activeShip.Engine != null)
        {
            SerializedObject se = new SerializedObject(m_activeShip.Engine);
            m_EngineSection.Bind(se);
            m_EngineSection.style.visibility = Visibility.Visible;
        }
        else
        {
            m_EngineSection.style.visibility = Visibility.Hidden;
        }

        if(m_activeShip.Weapon != null)
        {
            SerializedObject sw = new SerializedObject(m_activeShip.Weapon);
            m_WeaponSection.Bind(sw);
            m_WeaponSection.style.visibility = Visibility.Visible;
        }
        else
        {
            m_WeaponSection.style.visibility = Visibility.Hidden;
        }

        if (m_activeShip.Battery != null)
        {
            SerializedObject sb = new SerializedObject(m_activeShip.Battery);
            m_BatterySection.Bind(sb);
            m_BatterySection.style.visibility = Visibility.Visible;
        }
        else
        {
            m_BatterySection.style.visibility = Visibility.Hidden;
        }
    }
    public void LoadAllItems()
    {
        ships.Clear();
        string[] allPaths = Directory.GetFiles("Assets/Data", "*.asset", SearchOption.AllDirectories);
        foreach (string path in allPaths)
        {
            string cleanPath = path.Replace("\\", "/");
            ScriptableObject baseObj = (ScriptableObject)AssetDatabase.LoadAssetAtPath(cleanPath, typeof(ScriptableObject));
            switch (baseObj.GetType().ToString())
            {
                case "ShipSO":
                        ships.Add((ShipSO)baseObj);
                    break;
                case "EngineSO":
                    engines.Add((EngineSO)baseObj);
                    break;
                case "WeaponSO":
                    weapons.Add((WeaponSO)baseObj);
                    break;
                case "BatterySO":
                    batteries.Add((BatterySO)baseObj);
                    break;
                default:
                    Debug.Log("Not ship or component");
                    break;
            }
        }
    }
}
