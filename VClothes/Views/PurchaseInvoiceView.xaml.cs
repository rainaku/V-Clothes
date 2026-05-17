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

    private void Item_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element &&
            element.DataContext is PurchaseInvoiceDto invoice &&
            DataContext is PurchaseInvoiceViewModel vm)
        {
            vm.ViewDetailCommand.Execute(invoice);
        }
    }
}
