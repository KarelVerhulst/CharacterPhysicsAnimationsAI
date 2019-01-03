using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConditionNode : INode
{
    public delegate bool Condition(); // type is Condition waarbij je een methode kan instoppen die geen parameters aanvaard

    private readonly Condition _condition;

    public ConditionNode(Condition condition)
    {
        _condition = condition;
    }

    public IEnumerator<NodeResult> Tick()
    {
        if (_condition())
        {
           yield return NodeResult.Success;
        }
        else
        {
           yield return NodeResult.Failure;
        }
    }
}
