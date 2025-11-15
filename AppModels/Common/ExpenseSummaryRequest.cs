using AppModels.Common.Enums;

namespace AppModels.Common
{
    public class ExpenseSummaryRequest
    {
        public ExpenseSummaryType Type { get; set; }
        public DateTime? Date { get; set; } // تستخدم لـ Daily و Monthly
        public DateTime? From { get; set; } // لـ Range
        public DateTime? To { get; set; }   // لـ Range
    }
}
