using System.Windows.Controls;
using System.Windows.Input;

namespace VClothes.Views;

public partial class CategoryManagementView : UserControl
{
    public CategoryManagementView()
    {
        InitializeComponent();
    }

    private void MucDuocChon_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is System.Windows.FrameworkElement element && element.DataContext != null)
        {
            var vm = DataContext as VClothes.ViewModels.QuanLyLoaiSanPhamViewModel;
            if (vm != null)
            {
                vm.LoaiDuocChon = element.DataContext as VClothes.Data.LoaiSanPhamDto;
            }
        }
    }
}
