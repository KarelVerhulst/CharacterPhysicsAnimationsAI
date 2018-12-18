using System;
using System.Collections.Generic;

public class ActionNode : INode
{
    public delegate IEnumerator<NodeResult> Action();

    private readonly Action _action;

    public ActionNode(Action action)
    {
        _action = action;
    }

    public IEnumerator<NodeResult> Tick()
    {
        return _action();
    }
}
