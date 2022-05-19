using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
iEnumrator 배열을 받아 동시에 실행시키거나 순차적으로 실행시키는 클래스
반환된 객체를 사용해 yield 할 수 있음

exmaple 1)
//원하는 경우 언제나 중단 가능
var myCoroutine = Coroutines.Create(this, GoLeft(), GoRight()).Parallel();
myCoroutine.stop();

exmaple 2)
//iEnumrator 를 상속한 객체는 뭐든 활용가능. 별도의 코루틴없이 종료 시 실행 될 콜백 지원
var wait1 = new WaitWhile(() => IsSpinning);
var wait2 = new WaitUntil(() => IsButtonClicked);
Coroutines.Create(this, wait1, wait2, A()).Parallel(() =>
{
    //스핀이 끝나고 버튼도 눌렀다면 종료되어 콜백실행
    Debug.LogFormat("CustomYields Done);
});

example 3)
//체인 지원
var myCoroutine = Coroutines.Create(this);
myCoroutine.Add(GoLeft())
            .Add(GoRight())
            .Parallel();

exmaple 4)
//Func<iEnumerator> 를 이용하여 반복적인 재사용 가능
testCoroutines ??= Coroutines.Create(this, MyFunc1, MyFunc2);

if( Input.GetKeyDown(KeyCoad.A))
{
    testCoroutines?.Sequence(() => { Debug.LogFormat("coroutines reused."); });
}
*/

public class Coroutines : CustomYieldInstruction
{
    #region static
    public static Coroutines Create(MonoBehaviour monoBehaviour, params IEnumerator[] routines)
    {
        return Create(new Coroutines(monoBehaviour), routines);
    }

    public static Coroutines Create(Coroutines instance, params IEnumerator[] routines)
    {
        instance.Clear();

        for (int i = 0; i < routines.Length; ++i)
        {
            instance.Add(routines[i]);
        }

        return instance;
    }

    public static Coroutines Create(MonoBehaviour monoBehaviour)
    {
        return new Coroutines(monoBehaviour);
    }

    public static Coroutines Create(MonoBehaviour monoBehaviour, params Func<IEnumerator>[] routines)
    {
        return Create(new Coroutines(monoBehaviour), routines);
    }

    public static Coroutines Create(Coroutines instance, params Func<IEnumerator>[] routines)
    {
        instance.Clear();

        for (int i = 0; i < routines.Length; ++i)
        {
            instance.Add(routines[i]);
        }

        return instance;
    }
    #endregion


    public bool IsRunning { get; private set; }
    public override bool keepWaiting { get { return !isDone; } }

    private bool isDone;
    private MonoBehaviour mono;
    private List<ChildRoutine> routineList;
    private Coroutine processCoroutine;
    private Action onComplete;

    private Coroutines(MonoBehaviour monoBehaviour)
    {
        mono = monoBehaviour;
        routineList = new List<ChildRoutine>();
    }

    public void Clear()
    {
        Stop();
        routineList.Clear();
    }

    public Coroutines Add(IEnumerator routine)
    {
        routineList.Add(new ChildRoutine(mono, routine));
        return this;
    }

    public Coroutines Add(Func<IEnumerator> routineFunc)
    {
        routineList.Add(new ChildRoutine(mono, routineFunc));
        return this;
    }

    public Coroutines Sequence(Action onComplete = null)
    {
        this.onComplete = onComplete;
        processCoroutine = mono.StartCoroutine(SequenceProcess());
        return this;
    }

    private IEnumerator SequenceProcess()
    {
        ReadyToProcess();

        for (int i = 0; i < routineList.Count; ++i)
        {
            yield return routineList[i].Start();
        }

        Done();

        yield break;
    }

    public Coroutines Parallel(Action onComplete = null)
    {
        this.onComplete = onComplete;
        processCoroutine = mono.StartCoroutine(ParallelProcess());
        return this;
    }

    private IEnumerator ParallelProcess()
    {
        ReadyToProcess();

        for (int i = 0; i < routineList.Count; ++i)
        {
            routineList[i].Start();
        }

        yield return new WaitWhile(IsAnyRoutineRunning);

        Done();

        yield break;
    }

    private void ReadyToProcess()
    {
        isDone = false;
        IsRunning = true;
    }

    private void Done()
    {
        isDone = true;
        IsRunning = false;

        if (onComplete != null)
        {
            onComplete.Invoke();
            onComplete = null;
        }
    }

    private bool IsAnyRoutineRunning()
    {
        for (int i = 0; i < routineList.Count; ++i)
        {
            if (routineList[i].keepWaiting == true)
            {
                return true;
            }
        }

        return false;
    }

    public void Stop()
    {
        for (int i = 0; i < routineList.Count; ++i)
        {
            routineList[i].Stop();
        }

        if (processCoroutine != null)
        {
            mono.StopCoroutine(processCoroutine);
            processCoroutine = null;
        }

        Done();
    }

    private class ChildRoutine : CustomYieldInstruction
    {
        public override bool keepWaiting { get { return !isDone; } }

        private bool isDone;
        private Coroutine processCoroutine;
        private MonoBehaviour mono;
        private IEnumerator routine;
        private Func<IEnumerator> routineFunc;

        public ChildRoutine(MonoBehaviour mono, IEnumerator routine)
        {
            this.mono = mono;
            this.routine = routine;
        }

        public ChildRoutine(MonoBehaviour mono, Func<IEnumerator> routineFunc)
        {
            this.mono = mono;
            this.routineFunc = routineFunc;
        }

        public ChildRoutine Start()
        {
            if (processCoroutine == null)
            {
                mono.StartCoroutine(Process(routineFunc == null ? routine : routineFunc()));
            }

            return this;
        }

        private IEnumerator Process(IEnumerator routine)
        {
            isDone = false;

            processCoroutine = mono.StartCoroutine(routine);
            yield return processCoroutine;

            processCoroutine = null;
            isDone = true;
        }

        public void Stop()
        {
            if (processCoroutine != null)
            {
                mono.StopCoroutine(processCoroutine);
                processCoroutine = null;
            }

            isDone = true;
        }
    }
}