using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class DataImporter
{
    private static string GameManagerPath = PrefabPath + "System/GameManager.prefab";

    private static string CSVPath { get { return Application.dataPath + "/Data/CSVs/"; } }
    private const string ItemCSVName = "ItemSheet.csv";
    private const string MonsterCSVName = "MonsterSheet.csv";
    private const string DropCSVName = "DropSheet.csv";
    private const string SkillCSVName = "SkillSheet.csv";
    private const string NPCCSVName = "NPCSheet.csv";
    private const string QuestCSVName = "QuestSheet.csv";

    // 스크립터블 경로
    private const string ScriptablePath = "Assets/Scriptables/";

    private const string MonsterScriptablePath = ScriptablePath + "MonsterScriptable/";

    private const string ItemScriptablePath = ScriptablePath + "ItemScriptable/";
    private readonly static string[] ItemScriptablePaths = new string[(int)EItemType.LAST] {
        ItemScriptablePath + "Weapon/",
        ItemScriptablePath + "Pattern/",
        ItemScriptablePath + "ThrowItem/",
        ItemScriptablePath + "Others/"          };

    private const string SkillScriptablePath = ScriptablePath + "SkillScriptable/";
    private const string NPCScriptablePath = ScriptablePath + "NPCScriptable/";
    private const string QuestScriptablePath = ScriptablePath + "QuestScriptable/";

    // 프리펍 경로
    private const string PrefabPath = "Assets/Prefabs/";
    private const string MonsterPrefabPath = PrefabPath + "Monster/";
    private const string ItemPrefabPath = PrefabPath + "Item/";
    private readonly static string[] ItemPrefabPaths = new string[(int)EItemType.LAST] {
        ItemPrefabPath + "Weapon/",
        ItemPrefabPath + "Pattern/",
        ItemPrefabPath + "ThrowItem/",
        ItemPrefabPath + "Others/"
    };
    private const string SkillPrefabPath = PrefabPath + "PlayerSkill/";
    private const string NPCPrefabPath = PrefabPath + "NPC/";


    [MenuItem("Utilities/GenerateMonsters")]
    private static void GenerateMonsterData()
    {
        // 드랍 정보
        string[] allDropLines = File.ReadAllLines(CSVPath + DropCSVName);

        Dictionary<string, DropInfo> dropInfos = new();

        for (uint i = 1; i < allDropLines.Length; i++)
        {
            string si = allDropLines[i];
            string[] splitDropData = si.Split(',');

            string id = splitDropData[(int)EDropAttribute.MONSTER];
            int.TryParse(splitDropData[(int)EDropAttribute.STAT], out int stat);
            int.TryParse(splitDropData[(int)EDropAttribute.SOUL], out int soul);
            int.TryParse(splitDropData[(int)EDropAttribute.PURIFIED], out int purified);
            List<SDropItem> items = new();
            for (int j = 0; j<4; j++)
            {
                string item = splitDropData[(int)EDropAttribute.ITEM1 + j * 2];
                if(item == "") { continue; }
                float.TryParse(splitDropData[(int)EDropAttribute.RATE1 + j * 2], out float rate);
                SDropItem drop = new(item, rate);
                items.Add(drop);
            }

            DropInfo dropInfo = new(items, stat, soul, purified);
            dropInfos[id] = dropInfo;
        }


        // 몬스터 정보
        string[] allMonsterLines = File.ReadAllLines(CSVPath + MonsterCSVName);

        List<MonsterScriptable> datas = new();

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

            MonsterScriptable scriptable = AssetDatabase.LoadMainAssetAtPath($"{MonsterScriptablePath + id}.asset")as MonsterScriptable;

            if (scriptable == null)
            {
                scriptable = ScriptableObject.CreateInstance<MonsterScriptable>();
                AssetDatabase.CreateAsset(scriptable, $"{SkillScriptablePath + id}.asset");
            }
            datas.Add(scriptable);

            GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{MonsterPrefabPath + id}.prefab") as GameObject;
            if (prefab != null)
            {
                MonsterScript script = prefab.GetComponentInChildren<MonsterScript>();
                if (script == null) { Debug.LogError("몬스터에 스크립트 없음"); continue; }
                script.SetScriptable(scriptable);
            }

            scriptable.SetMonsterScriptable(idx, splitMonsterData, dropInfos[id], prefab);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
            if (prefab != null) { EditorUtility.SetDirty(prefab); }
        }

        GameObject gameManager = AssetDatabase.LoadMainAssetAtPath(GameManagerPath) as GameObject;
        MonsterManager monManager = gameManager.GetComponentInChildren<MonsterManager>();
        monManager.SetMonsterData(datas);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(gameManager);
    }

    [MenuItem("Utilities/GenerateItems")]
    private static void GenerateItemData()
    {
        // 아이템 정보
        string[] allItemLines = File.ReadAllLines(CSVPath + ItemCSVName);

        uint[] itemCnt = new uint[(int)EItemType.LAST];

        List<ItemScriptable>[] datas = new List<ItemScriptable>[(int)EItemType.LAST] { new(), new(), new(), new() };

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
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as WeaponScriptable;
                    if(scriptable == null)
                    {
                        scriptable = ScriptableObject.CreateInstance<WeaponScriptable>();
                        AssetDatabase.CreateAsset(scriptable, $"{ItemScriptablePaths[type] + id}.asset");
                    }
                    break;
                case ValueDefine.PATTERN_CODE:
                    type = (uint)EItemType.PATTERN;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as PatternScriptable;
                    if (scriptable == null)
                    {
                        scriptable = ScriptableObject.CreateInstance<PatternScriptable>();
                        AssetDatabase.CreateAsset(scriptable, $"{ItemScriptablePaths[type] + id}.asset");
                    }
                    break;
                case ValueDefine.THROW_ITEM_CODE:
                    type = (uint)EItemType.THROW;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as ThrowItemScriptable;
                    if (scriptable == null)
                    {
                        scriptable = ScriptableObject.CreateInstance<ThrowItemScriptable>();
                        AssetDatabase.CreateAsset(scriptable, $"{ItemScriptablePaths[type] + id}.asset");
                    }
                    break;
                case ValueDefine.OTHER_ITEM_CODE:
                    type = (uint)EItemType.OTHERS;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as OtherItemScriptable;
                    if (scriptable == null)
                    {
                        scriptable = ScriptableObject.CreateInstance<OtherItemScriptable>();
                        AssetDatabase.CreateAsset(scriptable, $"{ItemScriptablePaths[type] + id}.asset");
                    }
                    break;
                default: Debug.LogError("맞는 ID 없음"); return;
            }
            uint idx = itemCnt[type]++;
            datas[type].Add(scriptable);

            string prefabPath = ItemPrefabPaths[type];

            GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{prefabPath + id}.prefab") as GameObject;
            if (prefab != null)
            {
                switch (id[0])
                {
                    case ValueDefine.WEAPON_CODE:
                        WeaponScript weapon = prefab.GetComponent<WeaponScript>();
                        if (weapon != null) { weapon.SetScriptable((WeaponScriptable)scriptable); }
                        break;
                    case ValueDefine.PATTERN_CODE:

                        break;
                    case ValueDefine.THROW_ITEM_CODE:
                        ThrowItemScript throwItem = prefab.GetComponent<ThrowItemScript>();
                        if (throwItem != null) { throwItem.SetScriptable((ThrowItemScriptable)scriptable); }
                        break;
                    case ValueDefine.OTHER_ITEM_CODE:

                        break;
                    default: Debug.LogError("맞는 ID 없음"); return;
                }
            }

            scriptable.SetItemScriptable(idx, splitItemData, prefab);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
            if (prefab != null) { EditorUtility.SetDirty(prefab); }
        }

        GameObject gameManager = AssetDatabase.LoadMainAssetAtPath(GameManagerPath) as GameObject;
        ItemManager itemManager = gameManager.GetComponentInChildren<ItemManager>();
        itemManager.SetItemData(datas);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(gameManager);
    }

    [MenuItem("Utilities/GenerateSkills")]
    private static void GenerateSkillData()
    {
        // 스킬 정보
        string[] allSkillLines = File.ReadAllLines(CSVPath + SkillCSVName);

        List<SkillScriptable> datas = new();

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

            SkillScriptable scriptable = AssetDatabase.LoadMainAssetAtPath($"{SkillScriptablePath + id}.asset")as SkillScriptable;

            if (scriptable == null)
            {
                scriptable = ScriptableObject.CreateInstance<SkillScriptable>();
                AssetDatabase.CreateAsset(scriptable, $"{SkillScriptablePath + id}.asset");
            }
            datas.Add(scriptable);

            GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{SkillPrefabPath + id}.prefab") as GameObject;
            if (prefab != null)
            {
                PlayerSkillScript script = prefab.GetComponentInChildren<PlayerSkillScript>();
                if (script == null) { Debug.LogError("스킬에 스크립트 없음"); continue; }
                script.SetScriptable(scriptable);
            }

            scriptable.SetSkillScriptable(idx, splitSkillData, prefab);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
            if (prefab != null) { EditorUtility.SetDirty(prefab); }
        }

        GameObject gameManager = AssetDatabase.LoadMainAssetAtPath(GameManagerPath) as GameObject;
        SkillManager skillManager = gameManager.GetComponentInChildren<SkillManager>();
        skillManager.SetSkillData(datas);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(gameManager);
    }

    [MenuItem("Utilities/GenerateNPCs")]
    private static void GenerateNPCData()
    {
        // NPC 정보
        string[] allNPCLines = File.ReadAllLines(CSVPath + NPCCSVName);


        for (uint i = 1; i < allNPCLines.Length; i++)
        {
            uint idx = i - 1;
            string si = allNPCLines[i];
            string[] splitNPCData = si.Split(",");

            if (splitNPCData.Length != (int)EnpcAttribute.LAST)
            {
                Debug.Log(si + $"does not have {(int)EnpcAttribute.LAST} values.");
                return;
            }

            string[] npcData = new string[splitNPCData.Length - 1];     // NPC 정보
            List<string> dialogueData = new List<string>();             // 대사만 포함하고 있는 리스트

            for (int j = 0; j < (splitNPCData.Length - 1); j++)
            {
                if (splitNPCData[j] != "") npcData[j] = splitNPCData[j];
            }
            dialogueData.Add(splitNPCData[splitNPCData.Length - 1]);

            foreach (string npc in dialogueData)
            {
                Debug.Log(npc);
            }

            string id = splitNPCData[(int)EnpcAttribute.ID];

            NPCScriptable scriptable = AssetDatabase.LoadMainAssetAtPath($"{NPCScriptablePath + id}.asset") as NPCScriptable;

            bool IsExist = scriptable != null;
            if (!IsExist) { scriptable = ScriptableObject.CreateInstance<NPCScriptable>(); }

            scriptable.SetNPCScriptable(idx, npcData, dialogueData);

            if (!IsExist)
            {
                AssetDatabase.CreateAsset(scriptable, $"{NPCScriptablePath + id}.asset");
            }

            // npc 프리팹이 없으므로 일단 주석 처리

            //GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{NPCPrefabPath + id}.prefab") as GameObject;
            //if (prefab == null) { continue; }
            //if (!prefab.TryGetComponent<NPCScript>(out var script)) { Debug.Log(id + " 스크립트 없음"); continue; }

            //script.SetScriptable(scriptable);

            //AssetDatabase.SaveAssets();
            //EditorUtility.SetDirty(scriptable);
            //EditorUtility.SetDirty(prefab);
        }
    }

    [MenuItem("Utilities/GenerateQuests")]
    private static void GenerateQuestData()
    {
        // 퀘스트 정보
        string[] allQuestLines = File.ReadAllLines(CSVPath + QuestCSVName);

        List<QuestScriptable> datas = new();

        for (uint i = 1; i < allQuestLines.Length; i++)
        {
            uint idx = i - 1;
            string si = allQuestLines[i];
            string[] splitQuestData = si.Split(',');

            if (splitQuestData.Length != (int)EQuestAttribute.LAST)
            {
                Debug.Log(si + $"does not have {(int)EQuestAttribute.LAST} values.");
                return;
            }

            string id = splitQuestData[(int)EQuestAttribute.ID];

            QuestScriptable scriptable = AssetDatabase.LoadMainAssetAtPath($"{QuestScriptablePath + id}.asset") as QuestScriptable;

            if (scriptable == null)
            {
                scriptable = ScriptableObject.CreateInstance<QuestScriptable>();
                AssetDatabase.CreateAsset(scriptable, $"{QuestScriptablePath + id}.asset");
            }
            datas.Add(scriptable);

            scriptable.SetQuestScriptable(idx, splitQuestData);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
        }
    }
}
