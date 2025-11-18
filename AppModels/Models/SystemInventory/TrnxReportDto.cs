namespace AppModels.Models.SystemInventory
{
    public class TrnxReportDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int TotalTransactions { get; set; }

        public TypeSummaryDto IncomeSummary { get; set; } = new();
        public TypeSummaryDto OutcomeSummary { get; set; } = new();

        public decimal NetQuantity { get; set; }
        public decimal TotalImpurities { get; set; }
        public decimal AverageImpurityPercentage { get; set; }

        public IEnumerable<MaterialSummaryDto> IncomeByMaterial { get; set; } = new List<MaterialSummaryDto>();
        public IEnumerable<MaterialSummaryDto> OutcomeByMaterial { get; set; } = new List<MaterialSummaryDto>();
        public IEnumerable<WarehouseSummaryDto> WarehouseSummaries { get; set; } = new List<WarehouseSummaryDto>();
        public IEnumerable<MerchantSummaryDto> TopMerchants { get; set; } = new List<MerchantSummaryDto>();

        public PaymentSummaryDto PaymentSummary { get; set; } = new();
        public IEnumerable<AnomalyDto> Anomalies { get; set; } = new List<AnomalyDto>();
    }

    public class TypeSummaryDto
    {
        public int TransactionCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalRemaining { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalImpurities { get; set; }
    }

    public class MaterialSummaryDto
    {
        public Guid MaterialTypeId { get; set; }
        public string MaterialTypeName { get; set; } = string.Empty;
        public int TransactionCount { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalRemaining { get; set; }
        public decimal TotalImpurities { get; set; }
        public decimal AvgPricePerUnit { get; set; }
    }

    public class WarehouseSummaryDto
    {
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public int TransactionCount { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class MerchantSummaryDto
    {
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; } = string.Empty;
        public int TransactionCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalQuantity { get; set; }
    }

    public class PaymentSummaryDto
    {
        public int FullyPaidCount { get; set; }
        public int UnpaidCount { get; set; }
        public decimal TotalRemainingAmount { get; set; }
    }

    public class AnomalyDto
    {
        public Guid TransactionId { get; set; }
        public string TransactionIdentifier { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public string MaterialTypeName { get; set; } = string.Empty;
        public string MerchantName { get; set; } = string.Empty;
        public decimal ExpectedQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal Difference { get; set; }
    }

}
