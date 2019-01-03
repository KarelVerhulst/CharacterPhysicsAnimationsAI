using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SelectorNode : CompositeNode
{


    public SelectorNode(params INode[] nodes):base (nodes)
    {
  
    }

    public override IEnumerator<NodeResult> Tick()
    {
        NodeResult returnNodeResult = NodeResult.Failure;

        foreach (INode node in _nodes)
        {
            IEnumerator<NodeResult> result = node.Tick();

            while (result.MoveNext() && result.Current == NodeResult.Running)
            {
                yield return NodeResult.Running; // als result IsRunning is komt je hierin terecht
            }

            returnNodeResult = result.Current;

            if (returnNodeResult == NodeResult.Failure)
                continue; //start bij de lus volgende waarde

            if (returnNodeResult == NodeResult.Success)
                break;
        }

        //documenteer dat als er geen nodes aanwezig zijn dat dit een succes terug geeft
        yield return returnNodeResult;
    }
}
