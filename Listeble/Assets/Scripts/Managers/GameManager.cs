using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
  private static GameManager _instance;

  //----------------------------------------------

  private int _countUnlockedLevel = 1; // Количество открытых уровней

  //==============================================

  public static GameManager Instance
  {
    get
    {
      if (_instance == null)
      {
        GameObject obj = new GameObject("GameManager");
        _instance = obj.AddComponent<GameManager>();
        DontDestroyOnLoad(obj);
      }

      return _instance;
    }
  }

  //----------------------------------------------

  /// <summary>
  /// Список всех локаций с уровнями
  /// </summary>
  public Dictionary<TypeLocation, List<LevelData>> ListAllLocationsWithLevels = new Dictionary<TypeLocation, List<LevelData>>()
  {
     { TypeLocation.Summer, new List<LevelData>() },
     { TypeLocation.Winter, new List<LevelData>() }
  };

  /// <summary>
  /// Количество открытых уровней
  /// </summary>
  internal int CountUnlockedLevel
  {
    get => _countUnlockedLevel;
    set
    {
      _countUnlockedLevel = value;
      ChangeCountUnlockedLevel?.Invoke(value);
    }
  }

  //==============================================

  /// <summary>
  /// Событие: Изменение количества открытых уровней
  /// </summary>
  public CustomUnityEvent<int> ChangeCountUnlockedLevel { get; } = new CustomUnityEvent<int>();

  //==============================================

  private void Awake()
  {
    if (_instance == null || _instance != this)
    {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
      return;
    }
  }

  private void Start()
  {
    //UnlockNextLevelGame(TypeLocation.Summer, 5);
  }

  //==============================================

  /// <summary>
  /// Открыть все уровни игры
  /// </summary>
  public void OpenAllLevelsGame(TypeLocation curTypeLocation)
  {
    if (ListAllLocationsWithLevels.TryGetValue(curTypeLocation, out List<LevelData> levelData))
    {
      for (int i = 0; i < levelData.Count; i++)
      {
        levelData[i].levelComplete = true;
      }
    }
  }

  /// <summary>
  /// Открыть следующий уровень игры
  /// </summary>
  /// <param name="curTypeLocation">Текущий тип локации</param>
  /// <param name="curLevelGame">Текущий уровень игры</param>
  public void OpenNextLevelGame(TypeLocation curTypeLocation, int curLevelGame)
  {
    if (ListAllLocationsWithLevels.TryGetValue(curTypeLocation, out List<LevelData> levelData))
    {
      if (levelData[curLevelGame].levelComplete) { return; }

      levelData[curLevelGame].levelComplete = true;
      /*foreach (var level in levelData)
      {
        if (level.levelIndex != curLevelGame) { continue; }

        levelData[curLevelGame].levelComplete = true;
        break;
      }*/
    }
  }

  //==============================================

  [System.Serializable]
  public class LevelData
  {
    public int levelIndex; // Индекс уровня
    public bool levelComplete; // True, если уровень завершен
    public float bestTime; // Лучшее время прохождения уровня
  }
}

/// <summary>
/// Тип локации
/// </summary>
[System.Serializable]
public enum TypeLocation
{
  Summer,
  Winter
}