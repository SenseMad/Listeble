using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
  [SerializeField, Tooltip("Тип объекта")]
  private TypeObject _typeObject;
  [SerializeField, Tooltip("ID объекта")]
  private int _idObject;
  [SerializeField, Tooltip("Позиция объекта")]
  private Vector3Int _positionObject;

  //==============================================

  /// <summary>
  /// Тип объекта
  /// </summary>
  public TypeObject TypeObject { get => _typeObject; set => _typeObject = value; }
  /// <summary>
  /// ID объекта
  /// </summary>
  public int IdObject { get => _idObject; set => _idObject = value; }
  /// <summary>
  /// Позиция объекта
  /// </summary>
  public Vector3Int PositionObject { get => _positionObject; set => _positionObject = value; }

  //==============================================

  private void Start()
  {
    //MakeScriptableObject.DeleteLevel(7);
    //MakeScriptableObject.CreateLevel(new Vector3Int(1, 1, 1), new GridObjects[2, 1, 3]);
    //MakeScriptableObject.UpdateLevel(1);
  }

  //==============================================



  //==============================================



  //==============================================
}