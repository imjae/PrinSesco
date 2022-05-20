public class StateMachine<T> where T : class
{
    private T ownerEnity;           // StateMachine의 소유주
    private State<T> currentState;  // 현재 상태
    private State<T> previousState; // 이전 상태
    private State<T> globalState;   // 전역 상태

    public void Setup(T owner, State<T> entryState)
    {
        ownerEnity = owner;
        currentState = null;
        previousState = null;
        globalState = null;

        // entryState 상태로 변경
        ChangeState(entryState);
    }

    public void Execute()
    {
        if (globalState != null)
        {
            globalState.Execute(ownerEnity);
        }

        if (currentState != null)
        {
            // 상태가 변경되면 현재 상태는 이전 상태가 되기 때문에 previousState에 저장
            previousState = currentState;

            currentState.Execute(ownerEnity);
        }
    }

    public void ChangeState(State<T> newState)
    {
        // 새로 바꾸려는 상태가 비어있으면 상태를 바꾸지 않는다.
        if (newState == null) return;

        // 현재 재생중인 상태가 있으면 Exit() 함수 호출
        if (currentState != null)
        {
            currentState.Exit(ownerEnity);
        }

        currentState = newState;
        currentState.Enter(ownerEnity);
    }
    public void SetGlobalState(State<T> newState)
    {
        globalState = newState;
    }
    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }
}
