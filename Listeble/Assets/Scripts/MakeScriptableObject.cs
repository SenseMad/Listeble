using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MakeScriptableObject
{
  private static readonly string PATH = $"Assets/LevelData";

  /// <summary>
  /// ������� �������
  /// </summary>
  /// <param name="fieldSize">������ ����</param>
  /// <param name="gridObjects">������ � ������</param>
  public static void CreateLevel(TypeLocation typeLocation, Vector3Int fieldSize, GridObject[,,] gridObjects)
  {
    int tempIndexLevel = 0; // ������ ������ ������
    List<EditorLevelData> editorLevelData = new List<EditorLevelData>();

    string path = $"{PATH}/{typeLocation}.json";
    if (File.Exists(path))
    {
      editorLevelData = JsonHelper.FromJson<EditorLevelData>(File.ReadAllText(path));

      // �������� ������ ������� ������ (��� ����������)
      foreach (var item in editorLevelData)
      {
        if (item.levelIndex == tempIndexLevel)
        {
          tempIndexLevel++;
          continue;
        }
        break;
      }
    }

    //Enum.TryParse("lazer", out TypeObject typeObject);

    editorLevelData.Add(new EditorLevelData()
    {
      typeLocation = typeLocation.ToString(),
      levelIndex = tempIndexLevel,
      fieldSize = fieldSize,
      listBlockObjects = GetListDataAboutBlocks(gridObjects)
    });

    if (editorLevelData.Count > 1)
    {
      // ���������� ������� �� �����������
      for (int i = 0; i < editorLevelData.Count; i++)
      {
        for (int j = 0; j < editorLevelData.Count - 1; j++)
        {
          if (editorLevelData[j].levelIndex > editorLevelData[j + 1].levelIndex)
          {
            var temp = editorLevelData[j];
            editorLevelData[j] = editorLevelData[j + 1];
            editorLevelData[j + 1] = temp;
          }
        }
      }
    }

    string jsonToSave = JsonHelper.ToJson(editorLevelData, true);
    File.WriteAllText(path, jsonToSave);

    /*LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
    AssetDatabase.CreateAsset(levelData, $"Assets/ScriptableObjects/Levels/LevelData_{5}.asset");
    AssetDatabase.SaveAssets();*/
  }

  /// <summary>
  /// ��������� �������
  /// </summary>
  /// <param name="levelIndex">������ ������</param>
  public static EditorLevelData LoadLevel(TypeLocation typeLocation, int levelIndex)
  {
    string path = $"{PATH}/{typeLocation}.json";
    if (File.Exists(path))
    {
      List<EditorLevelData> editorLevelData = JsonHelper.FromJson<EditorLevelData>(File.ReadAllText(path));

      foreach (var item in editorLevelData)
      {
        if (item.levelIndex != levelIndex) { continue; }

        return item;
      }
    }

    Debug.LogError("�� ������� ��������� �������!");

    return null;
  }

  /// <summary>
  /// �������� �������
  /// </summary>
  /// <param name="levelIndex">������ ������</param>
  public static void UpdateLevel(TypeLocation typeLocation, int levelIndex, Vector3Int fieldSize, GridObject[,,] gridObjects)
  {
    string path = $"{PATH}/{typeLocation}.json";
    if (File.Exists(path))
    {
      List<EditorLevelData> editorLevelData = JsonHelper.FromJson<EditorLevelData>(File.ReadAllText(path));

      foreach (var item in editorLevelData)
      {
        if (item.levelIndex != levelIndex) { continue; }

        item.fieldSize = fieldSize;
        item.listBlockObjects = GetListDataAboutBlocks(gridObjects);
        break;
      }

      string jsonToSave = JsonHelper.ToJson(editorLevelData, true);
      File.WriteAllText(path, jsonToSave);
    }
  }

  /// <summary>
  /// ������� �������
  /// </summary>
  /// <param name="levelIndex">������ ������</param>
  public static void DeleteLevel(TypeLocation typeLocation, int levelIndex)
  {
    string path = $"{PATH}/{typeLocation}.json";
    if (File.Exists(path))
    {
      List<EditorLevelData> editorLevelData = JsonHelper.FromJson<EditorLevelData>(File.ReadAllText(path));

      foreach (var item in editorLevelData)
      {
        if (item.levelIndex != levelIndex) { continue; }

        editorLevelData.Remove(item);
        break;
      }

      string jsonToSave = JsonHelper.ToJson(editorLevelData, true);
      File.WriteAllText(path, jsonToSave);
    }
  }

  /// <summary>
  /// �������� ������ �������� ������
  /// </summary>
  /// <param name="gridObjects">������ � ������</param>
  /// <returns>������ ������ � ������</returns>
  private static List<BlockObject> GetListDataAboutBlocks(GridObject[,,] gridObjects)
  {
    var blockObjects = new List<BlockObject>();
    foreach (var gridObject in gridObjects)
    {
      if (gridObject == null) { continue; }

      blockObjects.Add(new BlockObject()
      {
        typeObject = gridObject.TypeObject.ToString(),
        positionObject = gridObject.PositionObject
      });
    }

    return blockObjects;
  }
}

[System.Serializable]
public class EditorLevelData
{
  public string typeLocation; // ��� �������
  public int levelIndex; // ������ ������
  public Vector3Int fieldSize; // ������ ������
  public List<BlockObject> listBlockObjects = new List<BlockObject>(); // ������ ������ �� ������
}

[System.Serializable]
public class BlockObject
{
  public string typeObject;
  public Vector3Int positionObject;
}