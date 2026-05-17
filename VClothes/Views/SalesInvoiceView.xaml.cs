using System.Windows.Controls;
using System.Windows.Input;
using VClothes.Data;
using VClothes.ViewModels;

namespace VClothes.Views;

public partial class SalesInvoiceView : UserControl
{
    public SalesInvoiceView()
    {
        InitializeComponent();
    }

    private void MucDuocChon_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element &&
            element.DataContext is HoaDonBanDto hoaDon &&
            DataContext is HoaDonBanViewModel vm)
        {
            vm.LenhXemChiTiet.Execute(hoaDon);
        }
    }
}
