using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class save : MonoBehaviour
{
  public Object scriptableObject;

  public void dSave()
  {
    string type = scriptableObject.name;

    // 建立 ScriptableObject
    ScriptableObject d = ScriptableObject.CreateInstance(type);
  
    // 建立dateSave至Resources夾，其檔案為date.asset
    AssetDatabase.CreateAsset(d, @"Assets/Resources/date2.asset");

    DataBase d2 = d as DataBase;
    d2.SetData(new string[]{"1", "1", "1", "1", "1", "1" });
  }
}