using Application.Interface.SystemInventory;
using AppModels.Common;
using AppModels.Models.Employees;
using AppModels.Models.SystemInventory;
using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.SystemInventory
{
    public class SystemInventoryServices : ISystemInventoryServices
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public SystemInventoryServices(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        // 🔹 هذه الدالة تقوم بإنشاء تقرير كامل عن حركة المعاملات خلال فترة زمنية
        public async Task<TrnxReportDto> GetTrnxReportAsync(DateTime from, DateTime to)
        {
            // 🎯 ضبط التاريخ ليشمل اليوم بالكامل (من بداية اليوم إلى نهايته)
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            // 🧾 بناء الاستعلام الأساسي، مع تضمين البيانات اللازمة (مادة - تاجر - مخزن)
            var baseQuery = _unit.Transaction
                .All
                .Include(t => t.MaterialType)
                .Include(t => t.Merchant)
                .Include(t => t.Warehouse)
                .Where(t => t.CreateDate >= fromDate && t.CreateDate <= toDate);

            // 🔸 فلترة المعاملات حسب نوعها (وارد / صادر)
            var incomeQuery = baseQuery.Where(t => t.Type == TransactionType.Income);
            var outcomeQuery = baseQuery.Where(t => t.Type == TransactionType.Outcome);

            // 📊 إحصاءات أولية (عدد العمليات)
            var totalTransactions = await baseQuery.CountAsync();
            var incomeCount = await incomeQuery.CountAsync();
            var outcomeCount = await outcomeQuery.CountAsync();

            // 💰 حساب إجمالي المبالغ للعمليات حسب نوعها
            var totalIncomeAmount = await incomeQuery.Select(t => (decimal?)t.TotalAmount).SumAsync() ?? 0m;
            var totalOutcomeAmount = await outcomeQuery.Select(t => (decimal?)t.TotalAmount).SumAsync() ?? 0m;

            // 💵 المبالغ المدفوعة من كل نوع معاملة
            var totalIncomePaid = await incomeQuery.Select(t => (decimal?)t.AmountPaid).SumAsync() ?? 0m;
            var totalOutcomePaid = await outcomeQuery.Select(t => (decimal?)t.AmountPaid).SumAsync() ?? 0m;

            // 🧮 حساب المتبقي من كل جهة
            var totalIncomeRemaining = totalIncomeAmount - totalIncomePaid;
            var totalOutcomeRemaining = totalOutcomeAmount - totalOutcomePaid;

            // 📦 كميات المواد الواردة والصادرة وصافي الحركة
            var totalIncomeQty = await incomeQuery.Select(t => (decimal?)t.Quantity).SumAsync() ?? 0m;
            var totalOutcomeQty = await outcomeQuery.Select(t => (decimal?)t.Quantity).SumAsync() ?? 0m;
            var netQuantity = totalIncomeQty - totalOutcomeQty;

            // 🧪 حساب نسبة ووزن الشوائب
            var totalImpurities = await baseQuery.Select(t => (decimal?)t.WeightOfImpurities).SumAsync() ?? 0m;
            var avgImpurityPercent = await baseQuery.Select(t => (decimal?)t.PercentageOfImpurities).AverageAsync() ?? 0m;

            // 📌 تجميع المعاملات حسب نوع المادة (الوارد فقط)
            var incomeByMaterial = await incomeQuery
                .GroupBy(t => new { t.MaterialTypeId, Name = t.MaterialType != null ? t.MaterialType.Name : string.Empty })
                .Select(g => new MaterialSummaryDto
                {
                    MaterialTypeId = g.Key.MaterialTypeId,
                    MaterialTypeName = g.Key.Name,
                    TransactionCount = g.Count(),
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalAmount = g.Sum(x => x.TotalAmount),
                    TotalPaid = g.Sum(x => x.AmountPaid),
                    TotalRemaining = g.Sum(x => x.TotalAmount - x.AmountPaid),
                    TotalImpurities = g.Sum(x => x.WeightOfImpurities),
                    AvgPricePerUnit = g.Average(x => x.PricePerUnit)
                })
                .ToListAsync();

            // 📌 تجميع المعاملات حسب نوع المادة (الصادر فقط)
            var outcomeByMaterial = await outcomeQuery
                .GroupBy(t => new { t.MaterialTypeId, Name = t.MaterialType != null ? t.MaterialType.Name : string.Empty })
                .Select(g => new MaterialSummaryDto
                {
                    MaterialTypeId = g.Key.MaterialTypeId,
                    MaterialTypeName = g.Key.Name,
                    TransactionCount = g.Count(),
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalAmount = g.Sum(x => x.TotalAmount),
                    TotalPaid = g.Sum(x => x.AmountPaid),
                    TotalRemaining = g.Sum(x => x.TotalAmount - x.AmountPaid),
                    TotalImpurities = g.Sum(x => x.WeightOfImpurities),
                    AvgPricePerUnit = g.Average(x => x.PricePerUnit)
                })
                .ToListAsync();

            // 🏭 تجميع المعاملات حسب المخزن
            var byWarehouse = await baseQuery
                .GroupBy(t => new { t.WarehouseId, Name = t.Warehouse != null ? t.Warehouse.Name : string.Empty })
                .Select(g => new WarehouseSummaryDto
                {
                    WarehouseId = g.Key.WarehouseId,
                    WarehouseName = g.Key.Name,
                    TransactionCount = g.Count(),
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalAmount = g.Sum(x => x.TotalAmount)
                })
                .ToListAsync();

            // 🥇 أفضل التجار حسب الكمية والقيمة
            var topMerchantsByAmount = await baseQuery
                .GroupBy(t => new { t.MerchantId, Name = t.Merchant != null ? t.Merchant.Name : string.Empty })
                .Select(g => new MerchantSummaryDto
                {
                    MerchantId = g.Key.MerchantId,
                    MerchantName = g.Key.Name,
                    TransactionCount = g.Count(),
                    TotalAmount = g.Sum(x => x.TotalAmount),
                    TotalPaid = g.Sum(x => x.AmountPaid),
                    TotalQuantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(m => m.TotalAmount)
                .Take(10)
                .ToListAsync();

            // 💳 حساب حالة السداد (مدفوع بالكامل / غير مدفوع)
            var fullyPaidCount = baseQuery.AsEnumerable().Where(t => t.TotalAmount - t.AmountPaid <= 0).Count();
            var unpaidCount = baseQuery.AsEnumerable().Where(t => t.TotalAmount - t.AmountPaid > 0).Count();
            var totalRemainingAmount = await baseQuery.Select(t => (decimal?)(t.TotalAmount - t.AmountPaid)).SumAsync() ?? 0m;

            // ⚠ اكتشاف العمليات الشاذة (فرق بين الوزن المتوقع والفعلي)
            var anomalies = await baseQuery
                .Where(t => t.CarAndMatrerialWeight - t.CarWeight - t.WeightOfImpurities != t.Quantity)
                .Select(t => new AnomalyDto
                {
                    TransactionId = t.Id,
                    TransactionIdentifier = t.TransactionIdentifier,
                    CreateDate = t.CreateDate,
                    MaterialTypeName = t.MaterialType != null ? t.MaterialType.Name : string.Empty,
                    MerchantName = t.Merchant != null ? t.Merchant.Name : string.Empty,
                    ExpectedQuantity = t.CarAndMatrerialWeight - t.CarWeight - t.WeightOfImpurities,
                    ActualQuantity = t.Quantity,
                    Difference = (t.CarAndMatrerialWeight - t.CarWeight - t.WeightOfImpurities) - t.Quantity
                })
                .ToListAsync();

            // 📦 تجميع البيانات النهائية في DTO التقرير
            var report = new TrnxReportDto
            {
                From = fromDate,
                To = toDate,
                TotalTransactions = totalTransactions,

                IncomeSummary = new TypeSummaryDto
                {
                    TransactionCount = incomeCount,
                    TotalAmount = totalIncomeAmount,
                    TotalPaid = totalIncomePaid,
                    TotalRemaining = totalIncomeRemaining,
                    TotalQuantity = totalIncomeQty,
                    TotalImpurities = await incomeQuery.Select(t => (decimal?)t.WeightOfImpurities).SumAsync() ?? 0m
                },

                OutcomeSummary = new TypeSummaryDto
                {
                    TransactionCount = outcomeCount,
                    TotalAmount = totalOutcomeAmount,
                    TotalPaid = totalOutcomePaid,
                    TotalRemaining = totalOutcomeRemaining,
                    TotalQuantity = totalOutcomeQty,
                    TotalImpurities = await outcomeQuery.Select(t => (decimal?)t.WeightOfImpurities).SumAsync() ?? 0m
                },

                NetQuantity = netQuantity,
                TotalImpurities = totalImpurities,
                AverageImpurityPercentage = avgImpurityPercent,

                IncomeByMaterial = incomeByMaterial,
                OutcomeByMaterial = outcomeByMaterial,
                WarehouseSummaries = byWarehouse,
                TopMerchants = topMerchantsByAmount,

                PaymentSummary = new PaymentSummaryDto
                {
                    FullyPaidCount = fullyPaidCount,
                    UnpaidCount = unpaidCount,
                    TotalRemainingAmount = totalRemainingAmount
                },

                Anomalies = anomalies
            };

            return report;
        }

        // 🔹 هذه الدالة تقوم بإنشاء تقرير كامل عن عامل خلال فترة زمنية
        public async Task<EmployeeFullFinancialReportDto> GetEmployeeFullFinancialReportAsync(Guid employeeId, DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);
            // Employee Validation
            var employee = await _unit.Employees.FindAsync(employeeId)
                ?? throw new Exception("⚠ الموظف غير موجود.");

            // Cash Advances total
            var cashAdvances = await _unit.EmployeeCashAdvance.All
                .Where(x => x.EmployeeId == employeeId
                            && x.CreateDate >= fromDate
                            && x.CreateDate <= toDate)
                .SumAsync(x => x.Amount);

            // Personal Expenses total
            var personalExpenses = await _unit.EmployeePersonalExpense.All
                .Where(x => x.EmployeeId == employeeId
                            && x.CreateDate >= fromDate
                            && x.CreateDate <= toDate)
                .SumAsync(x => x.Amount);

            //// Payroll history filtered by date range
            //var payrollHistory = await _unit.EmployeeMonthlyPayroll.All
            //    .Where(x => x.EmployeeId == employeeId
            //                && new DateTime(x.Year, x.Month, 1) >= new DateTime(fromDate.Year, fromDate.Month, 1)
            //                && new DateTime(x.Year, x.Month, 1) <= new DateTime(toDate.Year, toDate.Month, 1))
            //    .OrderByDescending(x => x.Year)
            //    .ThenByDescending(x => x.Month)
            //    .AsNoTracking()
            //    .ToListAsync();

            return new EmployeeFullFinancialReportDto
            {
                EmployeeId = employeeId,
                EmployeeName = employee.Name,
                BaseSalary = employee.BaseSalary,
                PeriodFrom = fromDate,
                PeriodTo = toDate,
                TotalCashAdvances = cashAdvances,
                TotalPersonalExpenses = personalExpenses,
                //PayrollHistory = _mapper.Map<IEnumerable<EmployeeMonthlyPayrollDto>>(payrollHistory)
            };
        }
    }
}
