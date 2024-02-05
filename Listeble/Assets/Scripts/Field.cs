using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
  [SerializeField, Tooltip("Объект который находится на клетке")]
  private GameObject _objectCell;

  //----------------------------------------------

  public Vector2Int m_CellPosition; // Координаты сетки

  //==============================================
}