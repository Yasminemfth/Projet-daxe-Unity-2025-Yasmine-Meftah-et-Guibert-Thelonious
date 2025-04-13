using System.Collections.Generic;
using UnityEngine;

//SO Ennemy => database

namespace A1_24_25
{
    [CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Database/EnemyDatabase")]
    public class EnemyDatabase : ScriptableObject
    {
        public List<EnemyData> datas = new();

        public EnemyData GetData(int id)
        {
            id = Mathf.Clamp(id, 0, datas.Count - 1);
            return datas[id];
        }
    }
}
