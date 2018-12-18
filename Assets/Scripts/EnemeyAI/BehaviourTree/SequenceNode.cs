using System;
using System.Collections.Generic;


public class SequenceNode : CompositeNode
{
 
    public SequenceNode(params INode[] nodes) : base(nodes)
    {
    }

    public override IEnumerator<NodeResult> Tick()
    {
        NodeResult returnNodeResult = NodeResult.Success;

        foreach (INode node in _nodes)
        {
            IEnumerator<NodeResult> result = node.Tick();

            while (result.MoveNext() && result.Current == NodeResult.Running)
            {
                yield return NodeResult.Running; // als result IsRunning is komt je hierin terecht
            }

            returnNodeResult = result.Current;

            if (returnNodeResult == NodeResult.Success)
                continue;

            if (returnNodeResult == NodeResult.Failure)
                break;
        }

        yield return returnNodeResult;
    }
}
