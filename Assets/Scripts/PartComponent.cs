using System;
using UnityEngine;

public class PartComponent : MonoBehaviour
{
    [SerializeField] private Renderer[] primarySkins;
    [SerializeField] private Renderer[] secondarySkins;
    [SerializeField] private bool hasMultipleMaterials = false;
    
    //QUE NOS PERDONE DIOS A TODES
    private string author = "";
    
    public void SetPrimaryColor(Color col)
    {
        if (hasMultipleMaterials)
        {
            primarySkins[0].materials[1].color = col;
            return;
        }
        foreach (Renderer mr in primarySkins)
        {
            mr.material.color = col;
        }
    }
    
    public void SetSecondaryColor(Color col)
    {
        if (hasMultipleMaterials)
        {
            primarySkins[0].materials[0].color = col;
            return;
        }
        foreach (Renderer mr in secondarySkins)
        {
            mr.material.color = col;
        }
    }

    public string GetAuthor()
    {
        return author;
    }

    public void SetAuthor(string aut)
    {
        author = aut;
    }
}
