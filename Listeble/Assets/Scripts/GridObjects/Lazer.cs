using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Lazer : GridObject
{
  [SerializeField, Tooltip("Позиция выстрела")]
  private Transform _shotPosition;

  //==============================================

  private LineRenderer LineRenderer { get; set; }

  //==============================================

  private void Awake()
  {
    LineRenderer = GetComponent<LineRenderer>();
  }

  private void Start()
  {
    
  }

  private void Update()
  {
    CreateLaser();
  }

  //==============================================

  private Ray ray;
  private RaycastHit hit;
  private void CreateLaser()
  {
    ray = new Ray(transform.position, transform.forward);

    LineRenderer.positionCount = 1;
    LineRenderer.SetPosition(0, transform.position);
    float remainingLength = 100;

    for (int i = 0; i < 25; i++)
    {
      if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
      {
        LineRenderer.positionCount += 1;
        LineRenderer.SetPosition(LineRenderer.positionCount - 1, hit.point);
        remainingLength -= Vector3.Distance(ray.origin, hit.point);
        ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));

        if (!hit.transform.GetComponent<Mirror>())
        {
          if (hit.transform.GetComponent<Crystal>())
          {
            hit.transform.GetComponent<Crystal>().IsCheckRayBeam = true;
            hit.transform.GetComponent<Crystal>().HitCrystal();
          }
          break;
        }
      }
      else
      {
        LineRenderer.positionCount += 1;
        LineRenderer.SetPosition(LineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
      }
    }
  }

  //==============================================
}