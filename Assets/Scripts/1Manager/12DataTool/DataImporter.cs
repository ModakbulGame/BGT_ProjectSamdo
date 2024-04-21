using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class DataImporter
{
    private static string CSVPath { get { return Application.dataPath + "/Data/CSVs/"; } }
    private const string ItemCSVName = "ItemSheet.csv";
    private const string MonsterCSVName = "MonsterSheet.csv";
    private const string DropCSVName = "DropSheet.csv";
    private const string SkillCSVName = "SkillSheet.csv";

    // 스크립터블 경로
    private const string ScriptablePath = "Assets/Scriptables/";

    private const string MonsterScriptablePath = ScriptablePath + "MonsterScriptable/";

    private const string ItemScriptablePath = ScriptablePath + "ItemScriptable/";
    private readonly static string[] ItemScriptablePaths = new string[(int)EItemType.LAST] {
        ItemScriptablePath + "Weapon/",
        ItemScriptablePath + "Pattern/",
        ItemScriptablePath + "ThrowItem/",
        ItemScriptablePath + "Others/"          };

    private const string DropInfoScriptablePath = ScriptablePath + "DropInfoScriptable/";

    private const string SkillScriptablePath = ScriptablePath + "SkillScriptable/";

    // 프리펍 경로
    private const string PrefabPath = "Assets/Prefabs/";
    private const string ManagerPrefabPath = PrefabPath + "System/";
    private const string MonsterPrefabPath = PrefabPath + "Monster/";
    private const string ItemPrefabPath = PrefabPath + "Item/";
    private readonly static string[] ItemPrefabPaths = new string[(int)EItemType.LAST] {
        ItemPrefabPath + "Weapon/",
        ItemPrefabPath + "Pattern/",
        ItemPrefabPath + "ThrowItem/",
        ItemPrefabPath + "Others/"
    };
    private const string SkillPrefabPath = PrefabPath + "PlayerSkill/";


    [MenuItem("Utilities/GenerateMonsters")]
    private static void GenerateMonsterData()
    {
        // 드랍 정보
        string[] allDropLines = File.ReadAllLines(CSVPath + DropCSVName);

        Dictionary<string, List<SDropItem>> dropInfos = new();

        for (uint i = 1; i < allDropLines.Length; i++)
        {
            string si = allDropLines[i];
            string[] splitDropData = si.Split(',');

            string id = splitDropData[0];
            List<SDropItem> dropInfo = new()            // 임시 설정
            {

                new("P101", 0.1f),
                new("P201", 0.1f),
                new("P301", 0.1f)

                //new(splitDropData[1], float.TryParse(splitDropData[6])),
                //new(splitDropData[2], float.TryParse(splitDropData[7])),
                //new(splitDropData[3], float.TryParse(splitDropData[8])),
                //new(splitDropData[4], float.TryParse(splitDropData[9])),
                //new(splitDropData[5], float.TryParse(splitDropData[10]))      // 이게 맞나...

            };
            dropInfos[id] = dropInfo;
        }


        // 몬스터 정보
        string[] allMonsterLines = File.ReadAllLines(CSVPath + MonsterCSVName);

        List<MonsterScriptable> monsters = new();

        for (uint i = 1; i < allMonsterLines.Length; i++)
        {
            uint idx = i - 1;
            string si = allMonsterLines[i];
            string[] splitMonsterData = si.Split(',');

            if (splitMonsterData.Length != (int)EMonsterAttribue.LAST)
            {
                Debug.Log(si + $"does not have {(int)EMonsterAttribue.LAST} values.");
                return;
            }

            string id = splitMonsterData[(int)EMonsterAttribue.ID];

            MonsterScriptable scriptable = AssetDatabase.LoadMainAssetAtPath($"{MonsterScriptablePath + id}.asset")as MonsterScriptable
                ??ScriptableObject.CreateInstance<MonsterScriptable>();
            scriptable.SetMonsterScriptable(idx, splitMonsterData, dropInfos[id]);

            monsters.Add(scriptable);

            GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{MonsterPrefabPath + id}.prefab") as GameObject;
            if (prefab == null) { continue; }
            if (!prefab.TryGetComponent<MonsterScript>(out var script)) { Debug.Log(id + " 스크립트 없음"); continue; }

            script.SetScriptable(scriptable);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
            EditorUtility.SetDirty(prefab);
        }
    }

    [MenuItem("Utilities/GenerateItems")]
    private static void GenerateItemData()
    {
        // 아이템 정보
        string[] allItemLines = File.ReadAllLines(CSVPath + ItemCSVName);

        uint[] itemCnt = new uint[(int)EItemType.LAST];

        List<ItemScriptable>[] items = new List<ItemScriptable>[(int)EItemType.LAST] { new(), new(), new(), new() };

        for (uint i = 1; i<allItemLines.Length; i++)
        {
            string si = allItemLines[i];
            string[] splitItemData = si.Split(',');

            if (splitItemData.Length != (int)EItemAttribute.LAST)
            {
                Debug.Log(si + $"does not have {(int)EItemAttribute.LAST} attributes");
                return;
            }

            string id = splitItemData[(int)EItemAttribute.ID];

            ItemScriptable scriptable;
            uint type;
            switch (id[0])
            {
                case ValueDefine.WEAPON_CODE:
                    type = (uint)EItemType.WEAPON;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as WeaponScriptable
                        ??ScriptableObject.CreateInstance<WeaponScriptable>();
                    break;
                case ValueDefine.PATTERN_CODE:
                    type = (uint)EItemType.PATTERN;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as PatternScriptable
                        ??ScriptableObject.CreateInstance<PatternScriptable>();
                    break;
                case ValueDefine.THROW_ITEM_CODE:
                    type = (uint)EItemType.THROW;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as ThrowItemScriptable
                        ??ScriptableObject.CreateInstance<ThrowItemScriptable>();
                    break;
                case ValueDefine.OTHER_ITEM_CODE:
                    type = (uint)EItemType.OTHERS;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as OtherItemScriptable
                        ??ScriptableObject.CreateInstance<OtherItemScriptable>();
                    break;
                default: Debug.LogError("맞는 ID 없음"); return;
            }
            uint idx = itemCnt[type]++;

            scriptable.SetItemScriptable(idx, splitItemData);
            items[type].Add(scriptable);

            string prefabPath = ItemPrefabPaths[type];

            GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{prefabPath + id}.prefab") as GameObject;
            if (prefab == null) { continue; }
            switch (id[0])
            {
                case ValueDefine.WEAPON_CODE:
                    WeaponScript weapon = prefab.GetComponent<WeaponScript>();
                    if (weapon != null && !weapon.IsScriptableSet) { weapon.SetScriptable((WeaponScriptable)scriptable); }
                    break;
                case ValueDefine.PATTERN_CODE:

                    break;
                case ValueDefine.THROW_ITEM_CODE:
                    ThrowItemScript throwItem = prefab.GetComponent<ThrowItemScript>();
                    if (throwItem != null && !throwItem.IsScriptableSet) { throwItem.SetScriptable((ThrowItemScriptable)scriptable); }
                    break;
                case ValueDefine.OTHER_ITEM_CODE:

                    break;
                default: Debug.LogError("맞는 ID 없음"); return;
            }

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
            EditorUtility.SetDirty(prefab);
        }
    }

    [MenuItem("Utilities/GenerateSkills")]
    private static void GenerateSkillData()
    {
        // 스킬 정보
        string[] allSkillLines = File.ReadAllLines(CSVPath + SkillCSVName);

        List<SkillScriptable> skills = new();

        for (uint i = 1; i < allSkillLines.Length; i++)
        {
            uint idx = i - 1;
            string si = allSkillLines[i];
            string[] splitSkillData = si.Split(',');

            if (splitSkillData.Length != (int)ESkillAttribute.LAST)
            {
                Debug.Log(si + $"does not have {(int)ESkillAttribute.LAST} values.");
                return;
            }

            string id = splitSkillData[(int)ESkillAttribute.ID];

            SkillScriptable scriptable = AssetDatabase.LoadMainAssetAtPath($"{SkillScriptablePath + id}.asset")as SkillScriptable
                ??ScriptableObject.CreateInstance<SkillScriptable>();

            scriptable.SetSkillScriptable(idx, splitSkillData);

            skills.Add(scriptable);

            GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{SkillPrefabPath + id}.prefab") as GameObject;
            if (prefab == null) { continue; }
            if (!prefab.TryGetComponent<PlayerSkillScript>(out var script)) { Debug.Log(id + " 스크립트 없음"); continue; }

            script.SetScriptable(scriptable);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
            EditorUtility.SetDirty(prefab);
        }
    }
}
