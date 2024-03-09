using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManageable
{
    void ArraySet();
    void DataSave();
    void DataLoad();
    void SpawnReady(int index);
    IEnumerator Spawn(int index);
}
