namespace NewsApp.Machine.State
{
    public interface IState
    {
        void OnCancel(bool shutdown);
        void OnMessage(NewsMessage msg);
        void OnReset();
        void OnStart();
        void OnStateEnter();
        void OnTimer();
    }
}