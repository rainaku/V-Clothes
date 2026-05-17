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

    private void Item_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element &&
            element.DataContext is SalesInvoiceDto invoice &&
            DataContext is SalesInvoiceViewModel vm)
        {
            vm.ViewDetailCommand.Execute(invoice);
        }
    }
}
