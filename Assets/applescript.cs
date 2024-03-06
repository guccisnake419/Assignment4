using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class applescript : MonoBehaviour
{
    public GameObject mycamera;

    [SerializeField] private BillboardType billboardType;

  [Header("Lock Rotation")]
  [SerializeField] private bool lockX;
  [SerializeField] private bool lockY;
  [SerializeField] private bool lockZ;

  private Vector3 originalRotation;
bool printed;
  public enum BillboardType { LookAtCamera, CameraForward };

  private void Awake() {
    originalRotation = transform.rotation.eulerAngles;
  }

  // Use Late update so everything should have finished moving.
  void LateUpdate() {
    if(mycamera==null) {mycamera= GameObject.Find("Player(Clone)");}
    if (mycamera != null){
        if(!printed){
            Debug.Log("Player found");
            printed= true;
        }
        switch (billboardType) {
            case BillboardType.LookAtCamera:
                transform.LookAt(mycamera.transform.position, Vector3.up);
                break;
            case BillboardType.CameraForward:
                transform.forward = mycamera.transform.forward;
                break;
            default:
                break;
        }
        // Modify the rotation in Euler space to lock certain dimensions.
        Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX) { rotation.x = originalRotation.x; }
        if (lockY) { rotation.y = originalRotation.y; }
        if (lockZ) { rotation.z = originalRotation.z; }
        transform.rotation = Quaternion.Euler(rotation);
    }
    // There are two ways people billboard things.
    
  }
}
