using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MakeScriptableObject
{
  private static readonly string PATH = $"Assets/LevelData";

  /// <summary>
  /// Создать уровень
  /// </summary>
  /// <param name="fieldSize">Размер поля</param>
  /// <param name="gridObjects">Данные о блоках</param>
  public static void CreateLevel(TypeLocation typeLocation, Vector3Int fieldSize, GridObject[,,] gridObjects)
  {
    int tempIndexLevel = 0; // Индекс нового уровня
    List<EditorLevelData> editorLevelData = new List<EditorLevelData>();

    string path = $"{PATH}/{typeLocation}.json";
    if (File.Exists(path))
    {
      editorLevelData = JsonHelper.FromJson<EditorLevelData>(File.ReadAllText(path));

      // Создание нового индекса уровня (без повторений)
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
      // Сортировка уровней по возрастанию
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
  /// Загрузить уровень
  /// </summary>
  /// <param name="levelIndex">Индекс уровня</param>
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

    Debug.LogError("Не удалось загрузить уровень!");

    return null;
  }

  /// <summary>
  /// Обновить уровень
  /// </summary>
  /// <param name="levelIndex">Индекс уровня</param>
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
  /// Удалить уровень
  /// </summary>
  /// <param name="levelIndex">Индекс уровня</param>
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
  /// Получить список объектов блоков
  /// </summary>
  /// <param name="gridObjects">Данные о блоках</param>
  /// <returns>Списка данных о блоках</returns>
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
  public string typeLocation; // Тип локации
  public int levelIndex; // Индекс уровня
  public Vector3Int fieldSize; // Размер уровня
  public List<BlockObject> listBlockObjects = new List<BlockObject>(); // Список блоков на уровне
}

[System.Serializable]
public class BlockObject
{
  public string typeObject;
  public Vector3Int positionObject;
}