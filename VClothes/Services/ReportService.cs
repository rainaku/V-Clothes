using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VClothes.Data;
using VClothes.Models;

namespace VClothes.Services;

public class ReportService
{
    public static string GenerateSimpleReport(DateTime fromDate, DateTime toDate)
    {
        using var context = new VClothesDbContext();
        var invoices = context.SalesInvoices
            .Include(s => s.Employee)
            .Include(s => s.Customer)
            .Include(s => s.Details)
            .ThenInclude(d => d.Product)
            .Where(s => s.InvoiceDate >= fromDate && s.InvoiceDate <= toDate)
            .OrderBy(s => s.InvoiceDate)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine("╔══════════════════════════════════════════════════════════════════╗");
        sb.AppendLine("║           CỬA HÀNG ÁO THUN V-CLOTHES                           ║");
        sb.AppendLine("║           BÁO CÁO DOANH THU                                    ║");
        sb.AppendLine("╠══════════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║  Từ ngày: {fromDate:dd/MM/yyyy}  Đến ngày: {toDate:dd/MM/yyyy}              ║");
        sb.AppendLine("╠══════════════════════════════════════════════════════════════════╣");
        sb.AppendLine("║  STT │ Mã HĐ      │ Ngày       │ Khách hàng      │ Tổng tiền   ║");
        sb.AppendLine("╠══════════════════════════════════════════════════════════════════╣");

        int stt = 1;
        decimal totalRevenue = 0;
        foreach (var inv in invoices)
        {
            var customerName = inv.Customer?.FullName ?? "Khách lẻ";
            sb.AppendLine($"║  {stt,-3} │ {inv.InvoiceCode,-10} │ {inv.InvoiceDate:dd/MM/yyyy} │ {customerName,-15} │ {inv.FinalAmount,11:N0} ║");
            totalRevenue += inv.FinalAmount;
            stt++;
        }

        sb.AppendLine("╠══════════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║  TỔNG CỘNG: {totalRevenue,52:N0} ║");
        sb.AppendLine($"║  Số hóa đơn: {invoices.Count,-50} ║");
        sb.AppendLine("╠══════════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║  Ngày lập: {DateTime.Now:dd/MM/yyyy HH:mm}                                     ║");
        sb.AppendLine($"║  Người lập: {AuthService.CurrentUser?.DisplayName ?? "N/A",-40}       ║");
        sb.AppendLine("╚══════════════════════════════════════════════════════════════════╝");

        return sb.ToString();
    }

    public static string GenerateAdvancedReport(DateTime fromDate, DateTime toDate)
    {
        using var context = new VClothesDbContext();
        var invoices = context.SalesInvoices
            .Include(s => s.Employee)
            .Include(s => s.Customer)
            .Include(s => s.Details)
            .ThenInclude(d => d.Product)
            .ThenInclude(p => p.Category)
            .Where(s => s.InvoiceDate >= fromDate && s.InvoiceDate <= toDate)
            .OrderBy(s => s.InvoiceDate)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine("╔══════════════════════════════════════════════════════════════════════════╗");
        sb.AppendLine("║              CỬA HÀNG ÁO THUN V-CLOTHES                                ║");
        sb.AppendLine("║              BÁO CÁO DOANH THU CHI TIẾT                                ║");
        sb.AppendLine("╠══════════════════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║  Từ ngày: {fromDate:dd/MM/yyyy}  Đến ngày: {toDate:dd/MM/yyyy}                    ║");
        sb.AppendLine("╠══════════════════════════════════════════════════════════════════════════╣");

        // Group by category
        var allDetails = invoices.SelectMany(i => i.Details).ToList();
        var groupedByCategory = allDetails
            .GroupBy(d => d.Product.Category?.Name ?? "Không phân loại")
            .OrderByDescending(g => g.Sum(d => d.SubTotal));

        sb.AppendLine("║                                                                          ║");
        sb.AppendLine("║  ▸ DOANH THU THEO LOẠI SẢN PHẨM                                         ║");
        sb.AppendLine("║  ─────────────────────────────────────────────────────────────────────── ║");
        sb.AppendLine("║  Loại sản phẩm          │ Số lượng bán │ Doanh thu        │ Tỷ lệ       ║");
        sb.AppendLine("║  ─────────────────────────────────────────────────────────────────────── ║");

        decimal totalRevenue = allDetails.Sum(d => d.SubTotal);
        foreach (var group in groupedByCategory)
        {
            var qty = group.Sum(d => d.Quantity);
            var revenue = group.Sum(d => d.SubTotal);
            var percentage = totalRevenue > 0 ? (revenue / totalRevenue * 100) : 0;
            sb.AppendLine($"║  {group.Key,-23} │ {qty,12:N0} │ {revenue,16:N0} │ {percentage,6:F1}%     ║");
        }

        // Group by employee
        sb.AppendLine("║                                                                          ║");
        sb.AppendLine("║  ▸ DOANH THU THEO NHÂN VIÊN                                             ║");
        sb.AppendLine("║  ─────────────────────────────────────────────────────────────────────── ║");
        sb.AppendLine("║  Nhân viên              │ Số hóa đơn   │ Doanh thu        │ Tỷ lệ       ║");
        sb.AppendLine("║  ─────────────────────────────────────────────────────────────────────── ║");

        var groupedByEmployee = invoices
            .GroupBy(i => i.Employee?.FullName ?? "N/A")
            .OrderByDescending(g => g.Sum(i => i.FinalAmount));

        decimal totalFinal = invoices.Sum(i => i.FinalAmount);
        foreach (var group in groupedByEmployee)
        {
            var count = group.Count();
            var revenue = group.Sum(i => i.FinalAmount);
            var percentage = totalFinal > 0 ? (revenue / totalFinal * 100) : 0;
            sb.AppendLine($"║  {group.Key,-23} │ {count,12:N0} │ {revenue,16:N0} │ {percentage,6:F1}%     ║");
        }

        // Summary statistics
        sb.AppendLine("║                                                                          ║");
        sb.AppendLine("║  ▸ THỐNG KÊ TỔNG HỢP                                                   ║");
        sb.AppendLine("║  ─────────────────────────────────────────────────────────────────────── ║");
        sb.AppendLine($"║  Tổng số hóa đơn:        {invoices.Count,-48} ║");
        sb.AppendLine($"║  Tổng doanh thu:          {totalFinal,15:N0} VNĐ                              ║");
        sb.AppendLine($"║  Trung bình/hóa đơn:      {(invoices.Count > 0 ? totalFinal / invoices.Count : 0),15:N0} VNĐ                              ║");
        sb.AppendLine($"║  Tổng sản phẩm bán:       {allDetails.Sum(d => d.Quantity),-48} ║");
        sb.AppendLine($"║  Hóa đơn cao nhất:        {(invoices.Any() ? invoices.Max(i => i.FinalAmount) : 0),15:N0} VNĐ                              ║");
        sb.AppendLine($"║  Hóa đơn thấp nhất:       {(invoices.Any() ? invoices.Min(i => i.FinalAmount) : 0),15:N0} VNĐ                              ║");

        sb.AppendLine("╠══════════════════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║  Ngày lập: {DateTime.Now:dd/MM/yyyy HH:mm}                                           ║");
        sb.AppendLine($"║  Người lập: {AuthService.CurrentUser?.DisplayName ?? "N/A",-48}          ║");
        sb.AppendLine("╚══════════════════════════════════════════════════════════════════════════╝");

        return sb.ToString();
    }

    public static void ExportToFile(string content, string filePath)
    {
        File.WriteAllText(filePath, content, Encoding.UTF8);
    }
}
