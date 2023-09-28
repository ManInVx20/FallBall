using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);
        void SaveData(GameData data);
    }
}
