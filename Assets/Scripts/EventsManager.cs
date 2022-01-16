
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public delegate void OnCreatureStartParty(Decoration decoration, string author);
    public static OnCreatureStartParty onCreatureStartParty;
    
    
}
