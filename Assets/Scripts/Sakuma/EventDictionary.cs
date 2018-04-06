using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDictionary : SingletonMonoBehaviour<EventDictionary>
{
    public Dictionary<string, object> eventFlagDictionary = new Dictionary<string, object>();
}