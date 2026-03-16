using UnityEngine;

public class MenuUFOFloat : MonoBehaviour
{
    public float range = 2f;
    public float speed = 2f;

    float startX;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        float x = startX + Mathf.Sin(Time.time * speed) * range;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
