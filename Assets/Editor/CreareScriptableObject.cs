using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Excel;
using System.Data;

public class CreareScriptableObject : EditorWindow
{
    public Object scriptableObject;
    private string xlxs;  // Excel 檔案位置
    private string worksheet;
    private string save;

    [MenuItem("Tools/CreareScriptableObject")]
    static void show()
    {
        GetWindow<CreareScriptableObject>().Show();
    }

    void OnGUI()
    {
        //--------------------------------------------
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ScriptableObject:", GUILayout.Width(90));
        scriptableObject = EditorGUILayout.ObjectField(scriptableObject, typeof(Object), true, GUILayout.Width(160));
        EditorGUILayout.EndHorizontal();

        worksheet = EditorGUILayout.TextField("讀取 Excel的工作表:", worksheet,  GUILayout.Width(250));

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("選擇 Excel檔案", GUILayout.Width(125)))
            xlxs = EditorUtility.OpenFilePanel("選擇檔案", "", "");

        if (GUILayout.Button("選擇檔案儲存位置", GUILayout.Width(125)))
        {
            save = EditorUtility.OpenFolderPanel("選擇檔案儲存位置", "", "");

            string[] s = Application.dataPath.Split('/');
            string projectName = s[s.Length - 2] + "/Assets";

            if (!save.Contains(projectName))
            {
                EditorUtility.DisplayDialog("警告", "請選擇該專案下的資料夾", "確定");
                save = string.Empty;
            }
            else
            {
                int i = save.IndexOf("/Assets");
                save = save.Substring(i + 7);
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        //--------------------------------------------

        if (GUILayout.Button("輸出檔案", GUILayout.Width(250)))
        {
            if (scriptableObject == null)
            {
                EditorUtility.DisplayDialog("警告", "腳本未擺放", "確定");
                return;
            }

            if (xlxs == string.Empty)
            {
                EditorUtility.DisplayDialog("警告", "Excel檔案未選擇", "確定");
                return;
            }

            if (save == string.Empty)
            {
                EditorUtility.DisplayDialog("警告", "檔案存放位置未選擇", "確定");
                return;
            }

            LoadData();
        }
    }

    private void LoadData()
    {
        // 讀取 Excel檔案
        FileStream stream = File.Open(xlxs, FileMode.Open, FileAccess.Read);

        // 創建讀取 Excel檔
        IExcelDataReader excelRead = ExcelReaderFactory.CreateOpenXmlReader(stream);

        // 將讀取到 Excel檔暫存至內存
        DataSet result = excelRead.AsDataSet();

        // 獲得 Excel檔的行與列的數目
        int columns = result.Tables[worksheet].Columns.Count;
        int rows = result.Tables[worksheet].Rows.Count;

        string[] data = null;

        // 將資料讀取出來
        for (int i = 1; i < rows; i++)
        {
            data = new string[columns];

            for (int j = 0; j < columns; j++)
            {
                data[j] = result.Tables[worksheet].Rows[i][j].ToString();
            }

            if (data != null)
            {
                string type = scriptableObject.name;

                // 建立 ScriptableObject
                ScriptableObject d = ScriptableObject.CreateInstance(type);

                // 建立ScriptableObject，其檔案為data[0].asset
                AssetDatabase.CreateAsset(d, @"Assets" + save + "/" + data[0] + ".asset");

                // 更新 ScriptableObject資料
                DataBase d2 = d as DataBase;
                d2.SetData(data);
            }

            float p = (float)i / (float)rows;

            EditorUtility.DisplayProgressBar("進度", "ScriptableObject輸出中 " + (int)p * 100 +"%", p);
        }

        scriptableObject = null;
        xlxs = string.Empty;
        save = string.Empty;
        worksheet = string.Empty;

        // 讀取完後一定要關閉
        excelRead.Close();

        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("警告", "ScriptableObject建立完成", "確定");
    }

    void  OnInspectorUpdate()
    {
        Repaint();
    }
}
