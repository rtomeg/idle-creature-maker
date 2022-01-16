using UnityEngine.Events;
using System.Collections.Generic;

public class OnTwitchCommandReceived : UnityEvent<string, string, List<string>> { }