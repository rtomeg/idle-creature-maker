using System;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Camera cam;
    private float verticalOffset = 100;
    
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        cam = Camera.main;
    }

    private void Update()
    {
        transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y - verticalOffset);
    }

    void FixedUpdate()
    {
        int layerMask = ~9;
        
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            text.SetText(hit.collider.GetComponent<PartComponent>().GetAuthor());
        }
        else
        {
            text.SetText("");

        }
    }
    
}
