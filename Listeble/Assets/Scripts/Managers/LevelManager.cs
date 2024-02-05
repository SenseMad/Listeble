using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
  private static LevelManager _instance;

  private float _timeSpentAtLevel = 0; // Время проведенное на уровне

  //==============================================

  public static LevelManager Instance
  {
    get
    {
      if (_instance == null) { _instance = FindObjectOfType<LevelManager>(); }
      return _instance;
    }
  }

  /// <summary>
  /// Время проведенное на уровне
  /// </summary>
  public float TimeSpentAtLevel
  {
    get => _timeSpentAtLevel;
    private set
    {
      _timeSpentAtLevel = value;
    }
  }

  //----------------------------------------------

  private GameManager GameManager { get; set; }

  /// <summary>
  /// Список кристаллов на уровне
  /// </summary>
  public List<Crystal> ListCristalsAtLevel { get; } = new List<Crystal>();

  /// <summary>
  /// True, если уровень пройден
  /// </summary>
  public bool IsLevelComplite { get; private set; }

  //==============================================

  /// <summary>
  /// Событие: Уровень завершен
  /// </summary>
  public CustomUnityEvent OnLevelComplite { get; } = new CustomUnityEvent();

  //==============================================

  private void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this);
      return;
    }
    _instance = this;

    GameManager = GameManager.Instance;
  }

  private void OnEnable()
  {
    OnLevelComplite.AddListener(CompliteLevel);
  }

  private void OnDisable()
  {
    OnLevelComplite.RemoveListener(CompliteLevel);
  }

  private void Update()
  {
    if (!IsLevelComplite) { _timeSpentAtLevel += Time.deltaTime; }
  }

  //==============================================

  /// <summary>
  /// Завершить уровень
  /// </summary>
  public void CompliteLevel()
  {
    foreach (var crystal in ListCristalsAtLevel)
    {
      if (!crystal.IsCrystalHit) { return; }
    }

    IsLevelComplite = true;
    //GameManager.OpenNextLevelGame();
    Debug.Log("Уровень пройден");
  }

  /// <summary>
  /// Определить количество кристаллов на уровне
  /// </summary>
  public void DitermineNumberCrystalsAtLevel(Crystal crystal)
  {
    ListCristalsAtLevel.Add(crystal);
  }

  //==============================================
}