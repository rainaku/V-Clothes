using System.Windows.Controls;
using System.Windows.Input;
using VClothes.ViewModels;

namespace VClothes.Views;

public partial class ProductManagementView : UserControl
{
    public ProductManagementView()
    {
        InitializeComponent();
    }

    private void MucDuocChon_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element && element.DataContext != null)
        {
            var vm = DataContext as QuanLySanPhamViewModel;
            vm?.GetType().GetProperty("SanPhamDuocChon")?.SetValue(vm, element.DataContext);
        }
    }

    private void SapXepTheoMa_Click(object sender, MouseButtonEventArgs e)
    {
        (DataContext as QuanLySanPhamViewModel)?.LenhSapXep.Execute("product_code");
    }

    private void SapXepTheoTen_Click(object sender, MouseButtonEventArgs e)
    {
        (DataContext as QuanLySanPhamViewModel)?.LenhSapXep.Execute("name");
    }

    private void SapXepTheoGia_Click(object sender, MouseButtonEventArgs e)
    {
        (DataContext as QuanLySanPhamViewModel)?.LenhSapXep.Execute("price");
    }

    private void SapXepTheoTon_Click(object sender, MouseButtonEventArgs e)
    {
        (DataContext as QuanLySanPhamViewModel)?.LenhSapXep.Execute("stock_quantity");
    }
}
