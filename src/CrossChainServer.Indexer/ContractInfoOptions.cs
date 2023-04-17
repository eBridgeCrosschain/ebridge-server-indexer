namespace CrossChainServer.Indexer;

public class ContractInfoOptions
{
    public Dictionary<string,ContractInfo> ContractInfos { get; set; }
}

public class ContractInfo
{
    public string BridgeContractAddress { get; set; }
    public string OracleContractAddress { get; set; }
    public string ReportContractAddress { get; set; }
    public string CrossChainContractAddress { get; set; }
    public string TokenContractAddress { get; set; }
}