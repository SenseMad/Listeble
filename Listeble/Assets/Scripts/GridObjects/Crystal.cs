using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{


  //==============================================

  private LevelManager LevelManager { get; set; }

  /// <summary>
  /// True, если в кристалл попали
  /// </summary>
  public bool IsCrystalHit { get; set; }

  /// <summary>
  /// Проверить луч попадания
  /// </summary>
  public bool IsCheckRayBeam { get; set; }

  //==============================================

  private void Awake()
  {
    LevelManager = LevelManager.Instance;
  }

  private void Start()
  {
    LevelManager.DitermineNumberCrystalsAtLevel(this);
  }

  private void Update()
  {
    if (!IsCheckRayBeam) { IsCrystalHit = false; }

    IsCheckRayBeam = false;
  }

  //==============================================

  /// <summary>
  /// Попали в кристалл
  /// </summary>
  public void HitCrystal()
  {
    if (IsCrystalHit) { return; }
    IsCrystalHit = true;
    LevelManager.OnLevelComplite?.Invoke();
  }

  //==============================================
}