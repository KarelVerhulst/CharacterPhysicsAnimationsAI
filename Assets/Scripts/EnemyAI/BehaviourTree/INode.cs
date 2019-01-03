using System;
using System.Collections.Generic;
using UnityEngine;


public interface INode
{
    IEnumerator<NodeResult> Tick();
}
