
public interface ParallelNodePoliceAccumulator
{
    NodeResult Policy(NodeResult result);
}


public class NSuccessIsSuccesAccumulator: ParallelNodePoliceAccumulator
{
    //public static ParallelNodePoliceAccumulator Factory()
    //{
    //    return new NSuccessIsSuccesAccumulator(2);
    //}

    public static ParallelNode.Policy Factory(int n) //currying
    {
        return () => new NSuccessIsSuccesAccumulator(n);
    }

    private readonly int _n;
    private int _count = 0;

    public NSuccessIsSuccesAccumulator(int n = 1)
    {
        _n = n;
    }
    public NodeResult Policy(NodeResult result)
    {
        if (result == NodeResult.Success)
            _count++;

        return (_count >= _n?NodeResult.Success:NodeResult.Failure);
    }
}


