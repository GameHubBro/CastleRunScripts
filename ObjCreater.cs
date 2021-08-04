using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjCreater : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private float speed = 0.05f;
    public static float yScale = 1f;
    private GameObject target;
    private float yPos = 0.4f;
    private float zPos = 10f;

    // Start is called before the first frame update
    void Start()
    {
        target = Instantiate(obj);
        target.transform.position = new Vector3(0, yPos, zPos);
        target.transform.localScale = new Vector3(1, yScale, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target.transform.position += Vector3.forward * -speed;
    }
}
