namespace NewsApp.Machine.State
{
    public interface IState
    {
        void OnCancel();
        void OnMessage(NewsMessage msg);
        void OnReset();
        void OnStart();
        void OnStateEnter();
        void OnTimer();
    }
}