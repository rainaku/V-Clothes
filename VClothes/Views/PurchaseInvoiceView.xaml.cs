using System.Windows.Controls;
using System.Windows.Input;
using VClothes.Data;
using VClothes.ViewModels;

namespace VClothes.Views;

public partial class PurchaseInvoiceView : UserControl
{
    public PurchaseInvoiceView()
    {
        InitializeComponent();
    }

    private void MucDuocChon_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element &&
            element.DataContext is PhieuNhapDto phieu &&
            DataContext is PhieuNhapViewModel vm)
        {
            vm.LenhXemChiTiet.Execute(phieu);
        }
    }
}
