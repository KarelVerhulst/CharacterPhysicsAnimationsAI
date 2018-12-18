using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ParallelNode : CompositeNode
{
    //public delegate NodeResult Policy(NodeResult result);
    public delegate ParallelNodePoliceAccumulator Policy();

    private Policy _policy;

    public ParallelNode(Policy policy, params INode[] nodes) : base(nodes)
    {
        _policy = policy;
    }

    public override IEnumerator<NodeResult> Tick()
    {
        ParallelNodePoliceAccumulator acc = _policy();

        NodeResult returnNodeResult = NodeResult.Failure;

        foreach (INode node in _nodes)
        {
            IEnumerator<NodeResult> result = node.Tick();

            while (result.MoveNext() && result.Current == NodeResult.Running)
            {
                yield return NodeResult.Running; // als result IsRunning is komt je hierin terecht
            }

            returnNodeResult = acc.Policy(result.Current);
        }

        yield return returnNodeResult;
    }

    //public override NodeResult Tick()
    //{
    //    NodeResult result = NodeResult.Failure;

    //    int count = 0;

    //    foreach (INode node in _nodes)
    //    {
    //        result = node.Tick();

    //        if (result == NodeResult.Failure)
    //        {
    //            count++;
    //        }
    //    }

    //    if (count >= 2)
    //    {
    //        result = NodeResult.Failure;
    //    }
    //    else
    //    {
    //        result = NodeResult.Success;
    //    }

    //    return result;
    //}

    //public override NodeResult Tick()
    //{
    //    NodeResult result = NodeResult.Failure;

    //    int count = 0;

    //    foreach (INode node in _nodes)
    //    {
    //        result = node.Tick();

    //        if (result == NodeResult.Failure)
    //        {
    //            count++;
    //        }
    //    }

    //    if (count >= 4)
    //    {
    //        result = NodeResult.Failure;
    //    }
    //    else
    //    {
    //        result = NodeResult.Success;
    //    }

    //    return result;
    //}
}
