using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDebugger : MonoBehaviour
{
    [SerializeField]
    private bool AbleDebug = true;
    [SerializeField]
    private List<MonsterScript> m_monsterList = new();
    [SerializeField]
    private MonsterSpawnPoint m_spawnPoint;


    private void DrawDebugs()                               // 오브젝트별 디버깅
    {
        if(!AbleDebug) { return; }
        for (int i=0;i<m_monsterList.Count;i++)
        {
            MonsterScript monster = m_monsterList[i];
            if(monster == null || monster.IsDead) { m_monsterList.Remove(monster); continue; }
            DrawView(monster);
            DrawTarget(monster);
        }
    }

    private void DrawView(MonsterScript _monster)             // 시야각 표시
    {
        Vector3 position = _monster.transform.position;
        float range = _monster.ViewRange;
        Gizmos.DrawWireSphere(position, range);

        float deg = _monster.ViewAngle;
        float rot = _monster.Rotation;
        Vector3 right = FunctionDefine.AngleToDir(rot + deg * 0.5f);
        Vector3 left = FunctionDefine.AngleToDir(rot - deg * 0.5f);
        Vector3 look = FunctionDefine.AngleToDir(rot);

        Debug.DrawRay(position, right * range, Color.blue);
        Debug.DrawRay(position, left * range, Color.blue);
        Debug.DrawRay(position, look * range, Color.cyan);
    }
    private void DrawTarget(MonsterScript _monster)           // 공격 대상 표시
    {
        if(!AbleDebug) { return; }
        ObjectScript target = _monster.CurTarget;
        if(target == null) { return; }

        float range = _monster.ViewRange;
        Vector3 pos = _monster.transform.position;
        float deg = _monster.ViewAngle;
        float rot = _monster.Rotation;
        Vector3 look = FunctionDefine.AngleToDir(rot);

        Collider[] targets = Physics.OverlapSphere(pos, range);

        if (targets.Length == 0) return;
        foreach (Collider col in targets)
        {
            if (!col.TryGetComponent<PlayerController>(out var player)) { continue; }
            Vector3 targetPos = col.transform.position;
            Vector3 targetDir = (targetPos - pos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(look, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= deg * 0.5f && !Physics.Raycast(pos, targetDir, range))
            {
                Debug.DrawLine(pos, targetPos, Color.red);
            }
        }
    }


    private bool ShowingCreateMonster { get; set; }
    private void ShowCreateMonster()
    {
        int height = 32 * (MonsterCount / 2 + 2) + 8;
        GUI.Box(new Rect(20, 100, 280, height), "몬스터 생성");

        for (int i = 0; i<MonsterCount; i++)
        {
            int xPos = i%2 == 0 ? 30 : 165;
            int yPos = 128 + 32 * (i / 2);
            if (GUI.Button(new Rect(xPos, yPos, 125, 30), $"{m_monsterNames[i]}"))
            {
                CreateMonster(i);
            }
        }
    }

    private void CreateMonster(int _idx)
    {
        GameObject prefab = PlayManager.GetMonsterPrefab((EMonsterName)_idx);
        if(prefab == null) { Debug.Log("몬스터 미완성"); return; }

        Vector3 point;
        if (m_spawnPoint == null) { point = m_spawnPoint.SpawnPosition; }
        else { point = Vector3.zero; }

        GameObject monster = Instantiate(prefab, point, Quaternion.Euler(0, Random.Range(-180f, 180f), 0));
        MonsterScript script = monster.GetComponent<MonsterScript>();
        m_monsterList.Add(script);
        Debug.Log($"{script.ObjectName} 생성됨");
    }


    private const int MonsterCount = (int)EMonsterName.LAST;
    private readonly string[] m_monsterNames = new string[MonsterCount];

    public void StartDebugger()
    {
        m_monsterList.Clear();
        MonsterScript[] monsters = FindObjectsOfType<MonsterScript>();
        foreach(MonsterScript monster in monsters) { m_monsterList.Add(monster); }
    }
    private void SetInfos()
    {
        for (int i = 0; i<MonsterCount; i++)
        {
            m_monsterNames[i] = PlayManager.GetMonsterInfo((EMonsterName)i).MonsterName;
        }
    }

    private void Awake()
    {
        SetInfos();
    }
    private void Start()
    {
        StartDebugger();
    }
    private void OnDrawGizmos()
    {
        DrawDebugs();
    }
    private void OnGUI()
    {
        if (ShowingCreateMonster)
        {
            ShowCreateMonster();
        }
        else if(GUI.Button(new Rect(20, 100, 150, 50), "몬스터 생성"))
        {
            ShowingCreateMonster = !ShowingCreateMonster;
        }
    }
}
