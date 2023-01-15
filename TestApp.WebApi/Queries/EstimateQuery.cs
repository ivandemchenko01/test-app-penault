namespace TestApp.WebApi.Queries;

public class EstimateQuery
{
    public decimal InputAmount { get; set; }
    public string InputCurrency { get; set; }
    public string OutputCurrency { get; set; }
}
