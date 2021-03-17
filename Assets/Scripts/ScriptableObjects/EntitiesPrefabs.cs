using UnityEngine;
[CreateAssetMenu(fileName = "Entities Prefabs", menuName = "ScriptableObjects/Global Variables/Entities Prefabs")]
public class EntitiesPrefabs : ScriptableObject
{
    [Header("Entities Parts")]
    public GameObject penisTentacleLike;
    public GameObject toiletBrushHair;
    public GameObject wormsInAHole;
    public GameObject crazyEyeball;
    public GameObject voidBall;
    public GameObject palmPlant;

    [Header("Entities Bodies")]
    public GameObject fullMoon;
    public GameObject regularSquare;
    public GameObject simpleSphere;
    public GameObject cubicCube;
    public GameObject longCube;
    public GameObject capsulePill;

    public GameObject GetPrefabFromEnum(PartEnum part)
    {
        switch (part)
        {
            case PartEnum.NONE:
                return null;
            case PartEnum.PENIS_TENTACLE_LIKE:
                return penisTentacleLike;
            case PartEnum.TOILET_BRUSH_HAIR:
                return toiletBrushHair;
            case PartEnum.WORMS_IN_A_HOLE:
                return wormsInAHole;
            case PartEnum.CRAZY_EYEBALL:
                return crazyEyeball;
            case PartEnum.VOID_BALL:
                return voidBall;
            case PartEnum.PALM_PLANT:
                return palmPlant;
            default:
                Debug.Log("Missing Prefab for " + part + " in ScriptableObject");
                return null;
        }
    }
    public GameObject GetPrefabFromEnum(BodyEnum part)
    {
        switch (part)
        {
            case BodyEnum.NONE:
                return null;
            case BodyEnum.FULL_MOON:
                return fullMoon;
            case BodyEnum.REGULAR_SQUARE:
                return regularSquare;
            case BodyEnum.SIMPLE_SPHERE:
                return simpleSphere;
            case BodyEnum.CUBIC_CUBE:
                return cubicCube;
            case BodyEnum.LONG_CUBE:
                return longCube;
            case BodyEnum.CAPSULE_PILL:
                return capsulePill;
            default:
                Debug.Log("Missing Prefab for " + part + " in ScriptableObject");
                return null;
        }
    }
}
