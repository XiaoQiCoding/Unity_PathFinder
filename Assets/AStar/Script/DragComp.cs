using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragComp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag() {
        Vector2 mousePos = Input.mousePosition;
        transform.position = TranlateScreenToWorld(mousePos);
    }

    private void OnMouseUp()
    {
        Vector2 mousePos = Input.mousePosition;
        Collider2D[] col = Physics2D.OverlapPointAll(TranlateScreenToWorld(mousePos));
        // 遍历碰撞体
        foreach (Collider2D c in col)
        {
            // 判断物体为“土地”，并且土地上没有其他物体
            if (c.tag == "Land" && c.transform.childCount == 0)
            {
                transform.parent = c.transform;
                transform.localPosition = Vector3.zero;
                // 防止鼠标检测被land挡住
                transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
            }
        }
    }

    public static Vector3 TranlateScreenToWorld(Vector3 position)
    {
        Vector3 cameraTranslatePos = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(cameraTranslatePos.x, cameraTranslatePos.y, 0);
    }
}
