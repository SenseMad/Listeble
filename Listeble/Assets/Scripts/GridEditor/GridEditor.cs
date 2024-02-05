using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// ������� �������
/// </summary>
public class GridEditor : MonoBehaviour
{
  [SerializeField, Tooltip("������ ����")]
  private Vector3Int _fieldSize;
  [SerializeField, Tooltip("������ ������")]
  private int _gridSize = 1;
  [SerializeField, Tooltip("������� ����")]
  private int _gridLevel = 0;

  [SerializeField, Tooltip("������ ����� �����")]
  private ListBlocks _listBlocks;

  /*[SerializeField, Tooltip("������ ��������")]
  private List<GameObject> _listObjects = new List<GameObject>();*/

  /*[Header("UI")]
  [SerializeField, Tooltip("������ ��������� ������")]
  private Button _levelUpButton;
  [SerializeField, Tooltip("")]
  private TextMeshProUGUI _levelText;
  [SerializeField, Tooltip("������ ��������� ������")]
  private Button _levelDownButton;
  [SerializeField, Tooltip("������ ���������")]
  private Button _saveButton;
  [SerializeField, Tooltip("������ ��������� �������")]
  private Button _loadButton;

  [SerializeField, Tooltip("���������� ��� ���������")]
  private Toggle _displayAllSubLevelToogle;

  [SerializeField, Tooltip("������ ���� X")]
  private TMP_InputField _fieldSizeX;
  [SerializeField, Tooltip("������ ���� Z")]
  private TMP_InputField _fieldSizeZ;*/

  //----------------------------------------------

  //private Dictionary<string, BlockData> ListCreatedBlocks = new Dictionary<string, BlockData>();
  private GridObject[,,] gridObjects;
  private BoxCollider BoxCollider { get; set;}

  //==============================================

  /// <summary>
  /// ������� ��������� ����
  /// </summary>
  public GridObject CurrentSelectedBlock { get; set; }

  public TypeObject CurrentTypeObject { get; set; }

  /// <summary>
  /// ������ ����
  /// </summary>
  public Vector3Int FieldSize
  {
    get => _fieldSize;
    private set
    {
      _fieldSize = value;
      OnChangeFieldSize?.Invoke(value);
    }
  }

  /// <summary>
  /// ������� ����
  /// </summary>
  public int GridLevel
  {
    get => _gridLevel;
    private set
    {
      _gridLevel = value;
      OnChangeGridLevel?.Invoke(value);
    }
  }

  //==============================================

  /// <summary>
  /// �������: ��������� ������ ����
  /// </summary>
  public CustomUnityEvent<int> OnChangeGridLevel { get; } = new CustomUnityEvent<int>();

  /// <summary>
  /// �������: ��������� ������� ����
  /// </summary>
  public CustomUnityEvent<Vector3Int> OnChangeFieldSize { get; } = new CustomUnityEvent<Vector3Int>();

  //==============================================

  private void Awake()
  {
    BoxCollider = GetComponent<BoxCollider>();
  }

  private void Start()
  {
    //CurrentTypeObject = TypeObject.block;
    Test2();
    gridObjects = new GridObject[_fieldSize.x, _fieldSize.y, _fieldSize.z];

    ChangeSizeBoxCollider();
  }

  private void OnEnable()
  {

  }

  private void OnDisable()
  {

  }

  private void Update()
  {
    // ���������� ����
    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit hitInfo))
      {
        GetXY(hitInfo.point, out Vector3Int vector3);
        RemoveBlock(vector3);
        AddBlock(vector3, GetObjectBlock(CurrentTypeObject));
      }
    }

    // ������� ����
    if (Input.GetMouseButtonDown(1))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit hitInfo))
      {
        GetXY(hitInfo.point, out Vector3Int vector3);
        RemoveBlock(vector3);
      }
    }
  }

  /// <summary>
  /// ��������� �������
  /// </summary>
  /// <param name="typeLocation">��� �������</param>
  public void SaveLevel(TypeLocation typeLocation)
  {
    MakeScriptableObject.CreateLevel(typeLocation, _fieldSize, gridObjects);
  }

  /// <summary>
  /// ��������� �������
  /// </summary>
  /// <param name="typeLocation">��� �������</param>
  /// <param name="levelIndex">������ ������</param>
  public void LoadLevel(TypeLocation typeLocation, int levelIndex)
  {
    var levelData = MakeScriptableObject.LoadLevel(typeLocation, levelIndex);

    _fieldSize = levelData.fieldSize;
    gridObjects = new GridObject[_fieldSize.x, _fieldSize.y, _fieldSize.z];

    foreach (var blockObject in levelData.listBlockObjects)
    {
      Enum.TryParse(blockObject.typeObject, out TypeObject typeObject);
      gridObjects[blockObject.positionObject.x, blockObject.positionObject.y, blockObject.positionObject.z] = GetObjectBlock(typeObject);
      gridObjects[blockObject.positionObject.x, blockObject.positionObject.y, blockObject.positionObject.z].TypeObject = typeObject;
      gridObjects[blockObject.positionObject.x, blockObject.positionObject.y, blockObject.positionObject.z].PositionObject = blockObject.positionObject;

      var block = Instantiate(gridObjects[blockObject.positionObject.x, blockObject.positionObject.y, blockObject.positionObject.z], transform);
      gridObjects[blockObject.positionObject.x, blockObject.positionObject.y, blockObject.positionObject.z] = block;
    }
  }

  /// <summary>
  /// �������� ����
  /// </summary>
  private void AddBlock(Vector3Int pos, GridObject gridObject)
  {
    var block = Instantiate(gridObject, transform);
    block.transform.position = pos;
    block.PositionObject = pos;
    gridObjects[pos.x, pos.y, pos.z] = block;
  }

  /// <summary>
  /// ������� ����
  /// </summary>
  /// <param name="pos">������� �����</param>
  private void RemoveBlock(Vector3Int pos)
  {
    if (gridObjects[pos.x, pos.y, pos.z] == null) { return; }

    Destroy(gridObjects[pos.x, pos.y, pos.z].gameObject);
    gridObjects[pos.x, pos.y, pos.z] = null;
  }

  private void GetXY(Vector3 worldPosition, out Vector3Int vector3Int)
  {
    vector3Int = new Vector3Int
    {
      x = Mathf.RoundToInt((worldPosition.x - transform.position.x) / _gridSize),
      y = _gridLevel,
      z = Mathf.RoundToInt((worldPosition.z - transform.position.z) / _gridSize)
    };
  }

  /// <summary>
  /// �������� ���� �� ����������
  /// </summary>
  /// <param name="typeObject">��� �������</param>
  public GridObject GetObjectBlock(TypeObject typeObject)
  {
    if (keyValuePairs.TryGetValue(typeObject, out GridObject gridObject))
    {
      return gridObject;
    }

    return null;
  }

  private Dictionary<TypeObject, GridObject> keyValuePairs = new Dictionary<TypeObject, GridObject>();
  private void Test2()
  {
    foreach (var block in _listBlocks.ListGroundBlocks)
    {
      keyValuePairs.Add(block.TypeObject, block);
    }
  }

  /// <summary>
  /// �������� ������ ����
  /// </summary>
  public void ChangeFieldSize(Vector3Int vector3Int)
  {
    _fieldSize = vector3Int;

    var tempGridObjects = gridObjects;
    gridObjects = new GridObject[_fieldSize.x, _fieldSize.y, _fieldSize.z];
    gridObjects = tempGridObjects;

    if (_gridLevel + 1 > _fieldSize.y) { GridLevel = _fieldSize.y - 1; }

    ChangeSizeBoxCollider();
  }

  /// <summary>
  /// �������� ������� ����
  /// </summary>
  public void ChangeGridLevel(bool parValue)
  {
    if (parValue && _gridLevel + 1 <= _fieldSize.y - 1)
    {
      GridLevel++;
    }
    else if (!parValue && _gridLevel - 1 >= 0)
    {
      GridLevel--;
    }

    ChangeSizeBoxCollider();
  }

  /// <summary>
  /// �������� ������� �����
  /// </summary>
  /*public void ChangeLevel(int parValue)
  {
    if (!_displayAllSubLevelToogle.isOn)
    {
      foreach (var item in ListCreatedBlocks)
      {
        if (item.Value._vector3Int.y == _gridLevel)
        {
          GetBlock(item.Value._typeBlock).SetActive(false);
        }
      }
    }

    _gridLevel += parValue;
    if (_gridLevel < 0) { _gridLevel = 0; }

    if (!_displayAllSubLevelToogle.isOn)
    {
      foreach (var item in ListCreatedBlocks)
      {
        if (item.Value._vector3Int.y == _gridLevel)
        {
          GetBlock(item.Value._typeBlock).SetActive(true);
        }
      }
    }

    _levelText.text = $"{_gridLevel}";

    UpdateSizeBoxCollider();
  }

  /// <summary>
  /// ����������/������ ��� ���������
  /// </summary>
  private void ShowHideAllSublevels(bool parValue)
  {
    foreach (var item in ListCreatedBlocks)
    {
      if (parValue)
      {
        GetBlock(item.Value._typeBlock).SetActive(true);
      }
      else
      {
        if (item.Value._vector3Int.y != _gridLevel)
        {
          GetBlock(item.Value._typeBlock).SetActive(false);
        }
      }
    }
  }*/

  /// <summary>
  /// �������� ������ BoxCollider
  /// </summary>
  private void ChangeSizeBoxCollider()
  {
    BoxCollider.size = new Vector3(_fieldSize.x, 1, _fieldSize.z) * _gridSize;
    BoxCollider.center = (BoxCollider.size - new Vector3(_gridSize, _gridSize, _gridSize)) * 0.5f;
    BoxCollider.center += new Vector3(0, _gridLevel, 0);
  }

  private void LoadLevel()
  {
    //if (levelData == null) { return; }

    //ListCreatedBlocks = levelData.ListBlocks;
    //Debug.Log(levelData.ListBlocks.Count);

    ChangeSizeBoxCollider();
  }

  //==============================================

  /// <summary>
  /// ������� ������ ��������
  /// </summary>
  private void CreateListObjects()
  {
    
  }

  /// <summary>
  /// ������� ������
  /// </summary>
  /// <param name="indexObject">������ �������</param>
  public void SelectObject(TypeObject indexObject)
  {
    //CurrentSelectedBlock = _listObjects[indexObject];
    CurrentTypeObject = indexObject;
  }

  //==============================================

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;

    for (int x = 0; x < _fieldSize.x; x++)
    {
      for (int y = 0; y < _fieldSize.z; y++)
      {
        Gizmos.DrawWireCube(transform.position + new Vector3(x * _gridSize, _gridLevel, y * _gridSize), new Vector3(_gridSize, _gridSize, _gridSize));
      }
    }
  }

  //==============================================
}