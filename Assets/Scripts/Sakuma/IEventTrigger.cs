interface IEventTrigger
{
    bool End { set; }
    bool StartEvent();
    bool NowEvent();
    bool EndEvent();
}