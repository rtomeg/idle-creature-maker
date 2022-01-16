using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerSlot : MonoBehaviour
{
    [SerializeField] private GameObject deleteButton;

    public Decoration decoration;

    private float timeFromLastAction;
    [SerializeField] private float inactiveMaxSeconds;
    [SerializeField] private Vector3 loopRotation = new Vector3(0, 20, 20);
    [SerializeField] private float animTime = 1;
    [SerializeField] private Transform container;

    public string username { get; private set; } = "";
    public bool isFree { get; private set; } = true;
    public bool isInactive { get; private set; }

    private bool decorationBeingHeld;
    private TextMeshPro tmp;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        //TODO: Pasar la rotación a la criatura
        Vector3 startRotation = container.transform.localRotation.eulerAngles;
        container.localRotation = Quaternion.Euler(startRotation + loopRotation / 2);
        container.DOLocalRotate(startRotation - loopRotation / 2, animTime)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        deleteButton.SetActive(false);
    }

    private void Update()
    {
        if (!isInactive)
        {
            timeFromLastAction += Time.deltaTime;
            if (timeFromLastAction >= inactiveMaxSeconds)
            {
                BecomeInactive();
            }
        }
    }

    public void userJoin(string user)
    {
        PlayerAction();
        username = user;
        tmp.SetText(user);
        isFree = false;
        decoration = new GameObject("decoration").AddComponent<Decoration>();
        decoration.transform.SetParent(container);
        decoration.transform.localPosition = Vector3.zero;
        decoration.transform.localRotation = Quaternion.identity;
        decoration.transform.localScale = Vector3.one;
        decoration.SetAuthor(username);
        deleteButton.SetActive(true);
    }

    public void UserLeave()
    {
        username = "";
        tmp.SetText("Free");
        isFree = true;
        if (decoration != null)
        {
            decoration.ResetDecoration();
        }

        deleteButton.SetActive(false);
        BecomeInactive();
    }

    public void UserReady()
    {
        decoration.StartParty();
        decoration.transform.SetParent(null);
        decoration = null;
        UserLeave();
    }

    public void SetBody(BodyEnum bodyEnum)
    {
        PlayerAction();
        decoration.SetBody(bodyEnum);
    }

    public void SetPrimaryColor(Color color)
    {
        PlayerAction();
        decoration.SetPrimaryColor(color);
    }

    public void SetPart(PartEnum partEnum, GameController.CommandArg twitchcommandArgument)
    {
        PlayerAction();
        decoration.SetPart(partEnum, twitchcommandArgument);
    }

    public void SetSecondaryColor(Color color)
    {
        PlayerAction();
        decoration.SetSecondaryColor(color);
    }

    public void Randomize()
    {
        PlayerAction();
        decoration.Randomize();
    }

    private void PlayerAction()
    {
        if (isInactive)
        {
            tmp.alpha = 1f;
            isInactive = false;
        }

        timeFromLastAction = 0;
    }

    private void BecomeInactive()
    {
        isInactive = true;
        tmp.alpha = 0.3f;
    }

    public bool StreamerPickDecoration()
    {
        if (decoration != null)
        {
            decoration.transform.parent = null;
            decorationBeingHeld = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReturnDecorationToSlot()
    {
        decoration.transform.parent = container;
        decoration.transform.localPosition = Vector3.zero;
        decoration.transform.localRotation = Quaternion.identity;
        decoration.transform.localScale = Vector3.one;
        decorationBeingHeld = false;
    }

    public void HangDecorationOnTree()
    {
        decoration = null;
        decorationBeingHeld = false;
        UserLeave();
    }
}