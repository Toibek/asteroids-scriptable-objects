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
    [SerializeField]
    private VisualTreeAsset m_ComponentRowTemplate;
    [Space]
    [SerializeField] private Sprite m_DefaultItemIcon;

    private ListView m_ShipList;
    private ListView m_EngineList;
    private ListView m_WeaponList;
    private ListView m_BatteryList;

    private float m_ItemHeight = 40;
    private float m_ComponentHeight = 20;

    private static List<ShipSO> ships = new();
    private static List<EngineSO> engines = new();
    private static List<WeaponSO> weapons = new();
    private static List<BatterySO> batteries = new();

    private VisualElement m_ShipDetails;
    private VisualElement m_EngineDetails;
    private VisualElement m_WeaponDetails;
    private VisualElement m_BatteryDetails;

    private VisualElement m_ships;
    private VisualElement m_Engines;
    private VisualElement m_Weapons;
    private VisualElement m_Batteries;


    private VisualElement m_LargeDisplayIcon;

    private ShipSO m_activeShip;

    [MenuItem("Tools/Item Editor")]
    public static void ShowTool()
    {
        ItemDatabase wnd = GetWindow<ItemDatabase>();
        wnd.titleContent = new GUIContent("Item Editor");

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

        m_ships = rootVisualElement.Q<VisualElement>("ItemsTab");
        m_Engines = rootVisualElement.Q<VisualElement>("EngineList");
        m_Weapons = rootVisualElement.Q<VisualElement>("WeaponList");
        m_Batteries = rootVisualElement.Q<VisualElement>("BatteryList");

        GenerateListView();

        m_ShipDetails = rootVisualElement.Q<VisualElement>("Details");
        m_EngineDetails = rootVisualElement.Q<VisualElement>("EngineDetails");
        m_WeaponDetails = rootVisualElement.Q<VisualElement>("WeaponDetails");
        m_BatteryDetails = rootVisualElement.Q<VisualElement>("BatteryDetails");

        m_ShipDetails.style.display = DisplayStyle.None;
        m_EngineDetails.style.display = DisplayStyle.None;
        m_WeaponDetails.style.display = DisplayStyle.None;
        m_BatteryDetails.style.display = DisplayStyle.None;
        m_LargeDisplayIcon = m_ShipDetails.Q<VisualElement>("Sprite");

        rootVisualElement.Q<Button>("NewShip").clicked += AddShip;
        rootVisualElement.Q<Button>("NewEngine").clicked += AddEngine;
        rootVisualElement.Q<Button>("NewWeapon").clicked += AddWeapon;
        rootVisualElement.Q<Button>("NewBattery").clicked += AddBattery;

        m_ShipDetails.Q<TextField>("ShipName").RegisterValueChangedCallback(evt =>
        {
            m_ShipList.Rebuild();
        });
        m_ShipDetails.Q<ObjectField>("ShipSprite").RegisterValueChangedCallback(evt =>
        {
            Sprite newSprite = evt.newValue as Sprite;
            m_LargeDisplayIcon.style.backgroundImage = newSprite == null ? m_DefaultItemIcon.texture : newSprite.texture;
        });
        m_EngineDetails.Q<TextField>("EngineName").RegisterValueChangedCallback(evt =>
        {
            m_EngineList.Rebuild();
        });
        m_WeaponDetails.Q<TextField>("WeaponName").RegisterValueChangedCallback(evt =>
        {
            m_WeaponList.Rebuild();
        });
        m_BatteryDetails.Q<TextField>("BatteryName").RegisterValueChangedCallback(evt =>
        {
            m_BatteryList.Rebuild();
        });

    }
    #region EntryLists
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
        m_ShipList = new ListView(ships, m_ItemHeight, makeItem, bindItem);
        m_ShipList.selectionType = SelectionType.Single;
        m_ShipList.style.height = ships.Count * m_ItemHeight;
        m_ships.Add(m_ShipList);
        m_ShipList.onSelectionChange += OnShipChange;


        Func<VisualElement> makeEngine = () => m_ComponentRowTemplate.CloneTree();
        Action<VisualElement, int> bindEngine = (e, i) =>
        {
            e.Q<Label>("Name").text = engines[i].Name;
        };
        m_EngineList = new ListView(engines, m_ComponentHeight, makeEngine, bindEngine);
        m_EngineList.selectionType = SelectionType.Single;
        m_EngineList.style.height = engines.Count * m_ComponentHeight;
        m_Engines.Add(m_EngineList);
        m_EngineList.onSelectionChange += OnEngineChange;

        Func<VisualElement> makeWeapon = () => m_ComponentRowTemplate.CloneTree();
        Action<VisualElement, int> bindWeapon = (e, i) =>
        {
            e.Q<Label>("Name").text = weapons[i].Name;
        };
        m_WeaponList = new ListView(weapons, m_ComponentHeight, makeWeapon, bindWeapon);
        m_WeaponList.selectionType = SelectionType.Single;
        m_WeaponList.style.height = weapons.Count * m_ComponentHeight;
        m_Weapons.Add(m_WeaponList);
        m_WeaponList.onSelectionChange += OnWeaponChange;

        Func<VisualElement> makeBattery = () => m_ComponentRowTemplate.CloneTree();
        Action<VisualElement, int> bindBattery = (e, i) =>
        {
            e.Q<Label>("Name").text = batteries[i].Name;
        };
        m_BatteryList = new ListView(batteries, m_ComponentHeight, makeBattery, bindBattery);
        m_BatteryList.selectionType = SelectionType.Single;
        m_BatteryList.style.height = batteries.Count * m_ComponentHeight;
        m_Batteries.Add(m_BatteryList);
        m_BatteryList.onSelectionChange += OnBatteryChange;
    }
    private void OnShipChange(IEnumerable<object> selected)
    {
        m_activeShip = (ShipSO)selected.First();
        SerializedObject ss = new SerializedObject(m_activeShip);

        m_ShipDetails.Bind(ss);
        if (m_activeShip.ShipSprite != null)
        {
            m_LargeDisplayIcon.style.backgroundImage = m_activeShip.ShipSprite.texture;
        }
        m_ShipDetails.style.display = DisplayStyle.Flex;

        rootVisualElement.Q<Slider>("Mass").RegisterValueChangedCallback(v =>
        {
            m_activeShip.OnMassChanged?.Invoke();
        });
        UpdateComponents();
    }
    private void OnEngineChange(IEnumerable<object> selected)
    {
        m_activeShip.Engine = (EngineSO)selected.First();
        UpdateComponents();
    }
    private void OnWeaponChange(IEnumerable<object> selected)
    {
        m_activeShip.Weapon = (WeaponSO)selected.First();
        UpdateComponents();
    }
    private void OnBatteryChange(IEnumerable<object> selected)
    {
        m_activeShip.Battery = (BatterySO)selected.First();
        UpdateComponents();
    }
    private void UpdateComponents()
    {
        if (m_activeShip.Engine != null)
        {
            SerializedObject se = new SerializedObject(m_activeShip.Engine);
            m_EngineDetails.Bind(se);
            m_EngineDetails.style.display = DisplayStyle.Flex;
        }
        else
        {
            m_EngineDetails.style.display = DisplayStyle.None;
        }

        if (m_activeShip.Weapon != null)
        {
            SerializedObject sw = new SerializedObject(m_activeShip.Weapon);
            m_WeaponDetails.Bind(sw);
            m_WeaponDetails.style.display = DisplayStyle.Flex;
        }
        else
        {
            m_WeaponDetails.style.display = DisplayStyle.None;
        }

        if (m_activeShip.Battery != null)
        {
            SerializedObject sb = new SerializedObject(m_activeShip.Battery);
            m_BatteryDetails.Bind(sb);
            m_BatteryDetails.style.display = DisplayStyle.Flex;
        }
        else
        {
            m_BatteryDetails.style.display = DisplayStyle.None;
        }
    }
    #endregion
    private void AddShip()
    {
        ShipSO newItem = CreateInstance<ShipSO>();
        newItem.Name = $"New Item";
        newItem.ShipSprite = m_DefaultItemIcon;

        AssetDatabase.CreateAsset(newItem, $"Assets/Data/Ships/{newItem.ID}.asset");
        ships.Add(newItem);
        m_ShipList.Rebuild();
        m_ShipList.style.height = ships.Count * m_ItemHeight;
    }
    private void AddEngine()
    {
        EngineSO newItem = CreateInstance<EngineSO>();
        newItem.Name = $"New Engine";

        AssetDatabase.CreateAsset(newItem, $"Assets/Data/Engines/{newItem.ID}.asset");
        engines.Add(newItem);
        m_EngineList.Rebuild();
        m_EngineList.style.height = engines.Count * m_ComponentHeight;
    }
    private void AddWeapon()
    {
        WeaponSO newItem = CreateInstance<WeaponSO>();
        newItem.Name = $"New Weapon";

        AssetDatabase.CreateAsset(newItem, $"Assets/Data/Weapons/{newItem.ID}.asset");
        weapons.Add(newItem);
        m_WeaponList.Rebuild();
        m_WeaponList.style.height = weapons.Count * m_ComponentHeight;
    }
    private void AddBattery()
    {
        BatterySO newItem = CreateInstance<BatterySO>();
        newItem.Name = $"New Battery";

        AssetDatabase.CreateAsset(newItem, $"Assets/Data/Batteries/{newItem.ID}.asset");
        batteries.Add(newItem);
        m_BatteryList.Rebuild();
        m_BatteryList.style.height = engines.Count * m_ComponentHeight;
    }

    public void LoadAllItems()
    {
        ships.Clear();
        engines.Clear();
        weapons.Clear();
        batteries.Clear();

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
