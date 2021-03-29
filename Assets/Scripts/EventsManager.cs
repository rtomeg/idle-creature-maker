
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public delegate void OnTwitchCommandReceived(TwitchCommand twitchCommand);
    public static OnTwitchCommandReceived onTwitchCommandReceived;

    public delegate void OnCreatureStartParty(Decoration decoration, string author);
    public static OnCreatureStartParty onCreatureStartParty;
    
    
}
