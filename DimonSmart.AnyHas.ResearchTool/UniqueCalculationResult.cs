namespace DimonSmart.AnyHas.ResearchTool;

public record UniqueCalculationResult(string AlgorithmName, int BlocksHashed, int UniqueHashes, int BufferSize,
    int HashLength);