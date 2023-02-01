using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxComp : MonoBehaviour
{
    public GameObject WallnutPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (transform.childCount == 0)
        {
            GameObject wallnut = Instantiate(WallnutPrefab) ;
            wallnut.transform.parent = transform;
            wallnut.transform.localPosition = Vector3.zero;
            wallnut.name = "Wallnut";
        }
        else if (transform.GetChild(0).name == "Wallnut")
        {
            Destroy(transform.Find("Wallnut").gameObject);
        }
    }
}
