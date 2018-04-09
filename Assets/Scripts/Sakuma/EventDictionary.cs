using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventTable:Serialize.TableBase<string,bool, EventPair>{ }

[System.Serializable]
public class EventPair:Serialize.KeyAndValue<string, bool>
{
    public EventPair(string key, bool value):base(key,value)
    {

    }
}
public class EventDictionary : SingletonMonoBehaviour<EventDictionary>
{
    public EventTable eventFlagDictionary;

    protected override void Awake()
    {
        foreach(KeyValuePair<string, bool> pair in eventFlagDictionary.GetTable())
        {
            Debug.Log("Key : " + pair.Key + " Value : " + pair.Value);
        }
    }
}