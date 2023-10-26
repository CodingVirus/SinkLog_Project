using System;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyObj;
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private GameObject minimapObject;
    public GameObject[] Enemys { get => enemys; }
    public void Initialize(EnemyCreateInfo[] enemyInfos)
    {
        enemys = new GameObject[enemyInfos.Length];
        if (enemys.Length > 0)
        {
            for (int i = 0; i < enemyInfos.Length; i++)
            {
                enemys[i] = Instantiate(enemyObj, transform.position, Quaternion.identity, transform);
                enemys[i].transform.localPosition = enemyInfos[i].CreatePosition + new Vector3(0.5f, 0.5f);
                enemys[i].layer = 10;
                switch (enemyInfos[i].CreateType)
                {
                    case EnemyType.SmallGround:
                        enemys[i].name = $"SmallGroundEnemy[{i}]";
                        break;
                    case EnemyType.SmallFly:
                        enemys[i].name = $"SmallFlyEnemy[{i}]";
                        break;
                }
                var icon = Instantiate(minimapObject, enemys[i].transform);
                icon.GetComponent<UIMinimapObj>().Initialize(enemys[i].transform, GlobalEnums.IconObjType.Enemy);
            }
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            foreach(GameObject enemyObj in enemys)
            {
                Destroy(enemyObj);
            }
        }
    }

    public void SetActiveEnemys(GlobalEnums.SetActive isActive = GlobalEnums.SetActive.Active)
    {
        if(enemys.Count(i => i == null) < enemys.Length - 1 && enemys.Length > 0)
        {
            foreach (GameObject enemy in enemys)
            {
                enemy.SetActive(Convert.ToBoolean(isActive));
            }
        }
    }

    public int GetAliveEnemyCount() => enemys.Count(i => i != null);
}
