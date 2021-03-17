using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    private Camera cam;

    private bool isHoldingDecoration = false;
    private bool isOverFreeTreeSlot;
    private PlayerSlot currentPS;
    private TreeSlot currentTS;

    private Vector3 screenPosition;
    private Vector3 offset;
    [SerializeField] private float dragCameraDistance = 20;


    private void Start()
    {
        cam = Camera.main;
    }
    
    public void PickDecoration(PlayerSlot ps)
    {
        if (ps.StreamerPickDecoration())
        {
            currentPS = ps;
            //Cursor.visible = false;
            screenPosition = cam.WorldToScreenPoint(ps.decoration.transform.position);
            offset = ps.decoration.transform.position -
                     cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                         screenPosition.z));
            isHoldingDecoration = true;
        }
    }

    public void DraggingDecoration()
    {
        if (!isOverFreeTreeSlot)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Vector3 currentPosition = ray.origin + ray.direction * dragCameraDistance;
            currentPS.decoration.transform.position = currentPosition;
        }
    }

    public void OnEnterTreeSlot(TreeSlot ts)
    {
        if (isHoldingDecoration && ts.isFree)
        {
            isOverFreeTreeSlot = true;
            currentTS = ts;
            currentPS.decoration.transform.SetParent(ts.transform);
            currentPS.decoration.transform.localPosition = Vector3.zero;
            currentPS.decoration.transform.localRotation = Quaternion.identity;
            currentPS.decoration.transform.localScale = Vector3.one;
        }
    }

    public void OnExitTreeSlot(TreeSlot ts)
    {
        isOverFreeTreeSlot = false;
        if (isHoldingDecoration)
        {
            
            currentPS.decoration.transform.SetParent(null);
            currentPS.decoration.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            currentPS.decoration.transform.localScale = Vector3.one;
        }
        currentTS = null;
    }

    public void OnMouseDragEnd()
    {
        Cursor.visible = true;
        isHoldingDecoration = false;
        if (isOverFreeTreeSlot)
        {
            currentPS.HangDecorationOnTree();
            currentTS.HangDecoration();
        }
        else
        {
            currentPS.ReturnDecorationToSlot();
        }

        currentPS = null;
    }
}