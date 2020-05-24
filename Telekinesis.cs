using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies a first person interface for simulating telekinesis
/// on moveable objects. Holding left click reels and holds an object.
/// Holding right click throws the object. The longer that the crosshair
/// is pointed at the object, the farther the object is thrown.
/// </summary>
/// <param name="objectPoint">
///     The reference transform where the selected
///     object will go once reeled.
///     Developer's Suggestion:
///     Position the objectPoint in the editor 1.5f away from the camera.
///     Make sure that the object point is a child of the player's camera.
/// </param>
/// <param name="threshold">
///     This threshold sets the maximum amount of distance the object
///     must have from the reeled object point for it to just hover as
///     a selected object in front of the player.
///     Developer's Suggestion:
///     Set to 1.5f for best results.
/// </param>
/// <param name="throwStrength">
///     This sets how strong the throw
///     must have from the reeled object point for it to just hover as
///     a selected object in front of the player.
///     Developer's Suggestion:
///     Set to 10f for best results.
/// </param>
public class Telekinesis : MonoBehaviour
{
    public Transform objectPoint;
    public float threshold = 1.5f;
    public float throwStrength;

    private const string moveableTag = "Moveable";
    private Vector3 objectVector;
    private bool throwing = false;

    void Update()
    {
        objectVector = objectPoint.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            GameObject target = hit.transform.gameObject;
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            if(target.tag == moveableTag)
            {
                if(Input.GetKey(KeyCode.Mouse1))
                {
                    Throw(target);
                    throwing = true;
                }
                else
                {
                    throwing = false;
                }

                if(Input.GetKey(KeyCode.Mouse0))
                {
                    if(!throwing)
                        Grab(target);
                }
            }
        }
    }

    void Grab(GameObject target)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        Vector3 targetPosition = target.transform.position;
        Vector3 direction = objectVector - target.transform.position;

        if(Vector3.Distance(objectVector, targetPosition) >= threshold)
        {
            targetRb.AddForce(direction);
        }
        else
        {
            targetRb.velocity = Vector3.zero;
            target.transform.position = objectVector;
        }
    }

    public void Throw(GameObject target)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        Vector3 direction = objectVector - transform.position;
        targetRb.AddForce(direction * throwStrength);
    }
}
