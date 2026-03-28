using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBelowCamera : MonoBehaviour
{
    public float destroyOffset = 8f; // thấp hơn camera bao nhiêu thì xóa

    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        if (cam == null) return;

        if (transform.position.y < cam.position.y - destroyOffset)
        {
            Destroy(gameObject);
        }
    }
}
