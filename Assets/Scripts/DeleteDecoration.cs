using System;
using UnityEngine;

public class DeleteDecoration : MonoBehaviour
{
    [SerializeField] private PlayerSlot ps;
    private void OnMouseUpAsButton()
    {
        ps.UserLeave();
    }
}
