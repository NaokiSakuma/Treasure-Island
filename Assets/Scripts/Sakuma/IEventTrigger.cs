interface IEventTrigger
{
    bool IsEnd { set; }
    bool StartEvent();
    bool NowEvent();
    bool EndEvent();
}