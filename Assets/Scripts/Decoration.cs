using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using CommandArg = GameController.CommandArg;

public class Decoration : MonoBehaviour
{
    private EntitiesPrefabs _entitiesPrefabs;
    public string author = "";
    private PartComponent body;
    private PartComponent topPart;
    private PartComponent bottomPart;
    private PartComponent frontPart;
    private PartComponent leftPart;
    private PartComponent rightPart;
    private Color primaryColor = new Color(1, 0.8f, 0);
    private Color secondaryColor = new Color(0.1f, 0.5f, 1f);


    //TODO: conseguir esto por código. Deja de hacer ñapas, por favor
    private float topLimit = 6.5f;
    private float bottomLimit = -4.5f;
    private float leftLimit = -10.5f;
    private float rightLimit = 10.5f;
    private float zOffset = 10f;

    private LastWall _lastWall;
    private Tweener animReady;

    private void Start()
    {
        //Que me perdone Dios
        _entitiesPrefabs = Resources.Load<EntitiesPrefabs>("EntitiesPrefabs");
    }

    public void SetAuthor(string username)
    {
        author = username;
    }

    public void SetBody(BodyEnum bodyEnum)
    {
        if (body == null)
        {
            body = Instantiate(_entitiesPrefabs.GetPrefabFromEnum(bodyEnum), transform).GetComponent<PartComponent>();
            body.SetPrimaryColor(primaryColor);
            //GOD FORGIVE KILLING SPREE
            body.SetAuthor(author);
        }
        else
        {
            ChangeBody(bodyEnum);
        }
    }

    private void ChangeBody(BodyEnum newBody)
    {
        DetachPartsFromBody(topPart, bottomPart, frontPart, leftPart, rightPart);
        Destroy(body.gameObject);
        body = Instantiate(_entitiesPrefabs.GetPrefabFromEnum(newBody), transform).GetComponent<PartComponent>();
        body.SetPrimaryColor(primaryColor);
        //GOD FORGIVE KILLING SPREE
        body.SetAuthor(author);

        AttachPartToBody(topPart, CommandArg.TOP);
        AttachPartToBody(frontPart, CommandArg.FRONT);
        AttachPartToBody(bottomPart, CommandArg.BOTTOM);
        AttachPartToBody(leftPart, CommandArg.LEFT);
        AttachPartToBody(rightPart, CommandArg.RIGHT);
    }

    private void DetachPartsFromBody(params PartComponent[] parts)
    {
        foreach (PartComponent partComponent in parts)
        {
            if (partComponent != null)
            {
                partComponent.transform.SetParent(null);
            }
        }
    }

    private void UpdatePartsPrimaryColor(Color col, params PartComponent[] parts)
    {
        foreach (PartComponent partComponent in parts)
        {
            if (partComponent != null)
            {
                partComponent.SetPrimaryColor(col);
            }
        }
    }

    private void UpdatePartsSecondaryColor(Color col, params PartComponent[] parts)
    {
        foreach (PartComponent partComponent in parts)
        {
            if (partComponent != null)
            {
                partComponent.SetSecondaryColor(col);
            }
        }
    }

    private void AttachPartToBody(PartComponent o, CommandArg commandArg)
    {
        if (o == null) return;
        Transform targetTransform = GetPartPosition(commandArg);
        o.transform.SetParent(targetTransform);
        o.transform.localPosition = Vector3.zero;
        o.transform.localRotation = Quaternion.identity;
        o.transform.localScale = Vector3.one;
    }

    public void ResetDecoration()
    {
        author = "";
        Destroy(gameObject);
    }

    public void SetPart(PartEnum partEnum, CommandArg arg)
    {
        if (body == null) return;
        switch (arg)
        {
            case CommandArg.TOP:
                InstantiatePart(ref topPart, partEnum, arg);
                break;
            case CommandArg.FRONT:
                InstantiatePart(ref frontPart, partEnum, arg);
                break;
            case CommandArg.LEFT:
                InstantiatePart(ref leftPart, partEnum, arg);
                break;
            case CommandArg.RIGHT:
                InstantiatePart(ref rightPart, partEnum, arg);
                break;
            case CommandArg.BOTTOM:
                InstantiatePart(ref bottomPart, partEnum, arg);
                break;
            default:
                break;
        }
    }

    private void InstantiatePart(ref PartComponent partComponent, PartEnum partEnum, CommandArg arg)
    {
        if (partComponent != null)
        {
            Destroy(partComponent.gameObject);
        }

        if (partEnum != PartEnum.NONE)
        {
            partComponent = Instantiate(_entitiesPrefabs.GetPrefabFromEnum(partEnum), GetPartPosition(arg))
                .GetComponent<PartComponent>();

            partComponent.SetPrimaryColor(primaryColor);
            partComponent.SetSecondaryColor(secondaryColor);
        }
    }

    private Transform GetPartPosition(CommandArg arg)
    {
        if (body != null)
        {
            switch (arg)
            {
                case CommandArg.TOP:
                    return body.transform.Find("TopPosition");
                case CommandArg.FRONT:
                    return body.transform.Find("FrontPosition");
                case CommandArg.LEFT:
                    return body.transform.Find("LeftPosition");
                case CommandArg.RIGHT:
                    return body.transform.Find("RightPosition");
                case CommandArg.BOTTOM:
                    return body.transform.Find("BottomPosition");
                default:
                    return null;
            }
        }

        return null;
    }

    public void SetPrimaryColor(Color col)
    {
        primaryColor = col;
        UpdatePartsPrimaryColor(col, body, topPart, bottomPart, rightPart, leftPart, frontPart);
    }

    public void SetSecondaryColor(Color col)
    {
        secondaryColor = col;
        UpdatePartsSecondaryColor(col, topPart, bottomPart, rightPart, leftPart, frontPart);
    }

    public void StartParty()
    {
        EventsManager.onCreatureStartParty(this, author);

        transform.position = getNewWallTarget();
        animReady = transform.DOMove(getNewWallTarget(), 3).SetEase(Ease.Linear).SetSpeedBased().SetAutoKill(false).OnComplete(() =>
        {
            animReady.ChangeEndValue(getNewWallTarget(), 3, true).Restart();
        });
    }

    public void Randomize()
    {
        ColorUtility.TryParseHtmlString(
            GameController.customColors.ElementAt(Random.Range(0, GameController.customColors.Count)).Value,
            out Color col);
        SetPrimaryColor(col);
        ColorUtility.TryParseHtmlString(
            GameController.customColors.ElementAt(Random.Range(0, GameController.customColors.Count)).Value,
            out col);
        SetSecondaryColor(col);

        //Que nos perdone Dios 2.0
        SetBody((BodyEnum) Random.Range(1, Enum.GetValues(typeof(BodyEnum)).Length));
        SetPart((PartEnum) Random.Range(0, Enum.GetValues(typeof(PartEnum)).Length), CommandArg.TOP);
        SetPart((PartEnum) Random.Range(0, Enum.GetValues(typeof(PartEnum)).Length), CommandArg.BOTTOM);
        SetPart((PartEnum) Random.Range(0, Enum.GetValues(typeof(PartEnum)).Length), CommandArg.LEFT);
        SetPart((PartEnum) Random.Range(0, Enum.GetValues(typeof(PartEnum)).Length), CommandArg.RIGHT);
        SetPart((PartEnum) Random.Range(0, Enum.GetValues(typeof(PartEnum)).Length), CommandArg.FRONT);
    }

    private Vector3 getNewWallTarget()
    {
        //Elena me ha perdonado por esto.
        LastWall newWall = _lastWall;

        while (newWall == _lastWall)
        {
            newWall = (LastWall) Random.Range(0, 4);
        }

        _lastWall = newWall;

        switch (_lastWall)
        {
            case LastWall.TOP:
                return new Vector3(Random.Range(leftLimit, rightLimit), topLimit, zOffset);
            case LastWall.BOTTOM:
                return new Vector3(Random.Range(leftLimit, rightLimit), bottomLimit, zOffset);
            case LastWall.LEFT:
                return new Vector3(leftLimit, Random.Range(topLimit, bottomLimit), zOffset);
            case LastWall.RIGHT:
                return new Vector3(rightLimit, Random.Range(topLimit, bottomLimit), zOffset);
        }

        return Vector3.zero;
    }

    private enum LastWall
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }
}