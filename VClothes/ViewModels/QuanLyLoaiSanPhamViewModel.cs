using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VClothes.Data;

namespace VClothes.ViewModels;

public class QuanLyLoaiSanPhamViewModel : ViewModelCha
{
    private ObservableCollection<LoaiSanPhamDto> _danhSachLoai = new();
    private LoaiSanPhamDto? _loaiDuocChon;
    private string _timKiem = string.Empty;
    private string _suaTen = string.Empty;
    private string _suaMoTa = string.Empty;
    private bool _suaDangHoatDong = true;
    private bool _dangSua;
    private string _thongBaoXacThuc = string.Empty;
    private string _cotSapXep = "name";
    private bool _sapXepTang = true;

    public ObservableCollection<LoaiSanPhamDto> DanhSachLoai { get => _danhSachLoai; set => GanGiaTri(ref _danhSachLoai, value); }
    public string CotSapXep { get => _cotSapXep; set => GanGiaTri(ref _cotSapXep, value); }
    public bool SapXepTang { get => _sapXepTang; set => GanGiaTri(ref _sapXepTang, value); }
    public LoaiSanPhamDto? LoaiDuocChon
    {
        get => _loaiDuocChon;
        set
        {
            if (GanGiaTri(ref _loaiDuocChon, value) && value != null)
            {
                SuaTen = value.Ten;
                SuaMoTa = value.MoTa ?? string.Empty;
                SuaDangHoatDong = value.DangHoatDong;
                DangSua = true;
                ThongBaoXacThuc = string.Empty;
            }
        }
    }
    public string TimKiem { get => _timKiem; set { if (GanGiaTri(ref _timKiem, value)) TimKiemLoai(); } }
    public string SuaTen { get => _suaTen; set => GanGiaTri(ref _suaTen, value); }
    public string SuaMoTa { get => _suaMoTa; set => GanGiaTri(ref _suaMoTa, value); }
    public bool SuaDangHoatDong { get => _suaDangHoatDong; set => GanGiaTri(ref _suaDangHoatDong, value); }
    public bool DangSua { get => _dangSua; set => GanGiaTri(ref _dangSua, value); }
    public string ThongBaoXacThuc { get => _thongBaoXacThuc; set => GanGiaTri(ref _thongBaoXacThuc, value); }

    public ICommand LenhThem { get; }
    public ICommand LenhLuu { get; }
    public ICommand LenhXoa { get; }
    public ICommand LenhHuy { get; }
    public ICommand LenhTimKiem { get; }
    public ICommand LenhSapXep { get; }

    public QuanLyLoaiSanPhamViewModel()
    {
        LenhThem = new LenhRelay(_ => ThucHienThem());
        LenhLuu = new LenhRelay(_ => ThucHienLuu());
        LenhXoa = new LenhRelay(_ => ThucHienXoa());
        LenhHuy = new LenhRelay(_ => ThucHienHuy());
        LenhTimKiem = new LenhRelay(_ => TimKiemLoai());
        LenhSapXep = new LenhRelay(ThucHienSapXep);
        DangTai = true;
        TaiAsync();
    }

    private async void TaiAsync()
    {
        try { await Task.Run(() => TaiDanhSachLoai()); }
        catch { }
        finally { DangTai = false; }
    }

    private void TaiDanhSachLoai()
    {
        var sapXep = $"order={CotSapXep}.{(SapXepTang ? "asc" : "desc")}";
        var danhSach = SupabaseClient.LayDanhSach<LoaiSanPhamDto>("categories", sapXep);
        DanhSachLoai = new ObservableCollection<LoaiSanPhamDto>(danhSach);
    }

    private void TimKiemLoai()
    {
        try
        {
            var sapXep = $"order={CotSapXep}.{(SapXepTang ? "asc" : "desc")}";
            var truyVan = sapXep;
            if (!string.IsNullOrWhiteSpace(TimKiem))
                truyVan = $"or=(name.ilike.%25{Uri.EscapeDataString(TimKiem)}%25,description.ilike.%25{Uri.EscapeDataString(TimKiem)}%25)&{sapXep}";

            var danhSach = SupabaseClient.LayDanhSach<LoaiSanPhamDto>("categories", truyVan);
            DanhSachLoai = new ObservableCollection<LoaiSanPhamDto>(danhSach);

            if (!danhSach.Any() && !string.IsNullOrWhiteSpace(TimKiem))
                ThongBaoXacThuc = "Không tìm thấy loại sản phẩm nào phù hợp";
            else
                ThongBaoXacThuc = string.Empty;
        }
        catch { }
    }

    private void ThucHienSapXep(object? thamSo)
    {
        var cot = thamSo?.ToString();
        if (string.IsNullOrEmpty(cot)) return;
        if (CotSapXep == cot)
            SapXepTang = !SapXepTang;
        else
        {
            CotSapXep = cot;
            SapXepTang = true;
        }
        TimKiemLoai();
    }

    private void ThucHienThem()
    {
        LoaiDuocChon = null;
        SuaTen = string.Empty;
        SuaMoTa = string.Empty;
        SuaDangHoatDong = true;
        DangSua = true;
        ThongBaoXacThuc = string.Empty;
    }

    private void ThucHienLuu()
    {
        ThongBaoXacThuc = string.Empty;
        if (string.IsNullOrWhiteSpace(SuaTen))
        { ThongBaoXacThuc = "Tên loại sản phẩm không được để trống"; return; }
        if (SuaTen.Length > 100)
        { ThongBaoXacThuc = "Tên loại sản phẩm không được quá 100 ký tự"; return; }

        try
        {
            var daTonTai = SupabaseClient.LayDanhSach<LoaiSanPhamDto>("categories",
                $"name=eq.{Uri.EscapeDataString(SuaTen)}&id=neq.{LoaiDuocChon?.Id ?? 0}");
            if (daTonTai.Any())
            { ThongBaoXacThuc = "Tên loại sản phẩm đã tồn tại"; return; }

            if (LoaiDuocChon != null)
            {
                SupabaseClient.CapNhat("categories", $"id=eq.{LoaiDuocChon.Id}", new
                {
                    name = SuaTen,
                    description = string.IsNullOrWhiteSpace(SuaMoTa) ? (string?)null : SuaMoTa,
                    is_active = SuaDangHoatDong
                });
            }
            else
            {
                SupabaseClient.ThemMoi<LoaiSanPhamDto>("categories", new
                {
                    name = SuaTen,
                    description = string.IsNullOrWhiteSpace(SuaMoTa) ? (string?)null : SuaMoTa,
                    is_active = SuaDangHoatDong
                });
            }

            TaiDanhSachLoai();
            ThucHienHuy();
            ThongBaoXacThuc = "Lưu thành công!";
        }
        catch (Exception ex)
        {
            ThongBaoXacThuc = $"Lỗi: {ex.Message}";
        }
    }

    private void ThucHienXoa()
    {
        if (LoaiDuocChon == null) return;
        var ketQua = MessageBox.Show($"Bạn có chắc muốn xóa loại sản phẩm '{LoaiDuocChon.Ten}'?",
            "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (ketQua == MessageBoxResult.Yes)
        {
            try
            {
                var coSanPham = SupabaseClient.LayDanhSach<SanPhamDto>("products",
                    $"category_id=eq.{LoaiDuocChon.Id}&select=id&limit=1");
                if (coSanPham.Any())
                { ThongBaoXacThuc = "Không thể xóa loại sản phẩm đang có sản phẩm liên kết"; return; }

                SupabaseClient.Xoa("categories", $"id=eq.{LoaiDuocChon.Id}");
                TaiDanhSachLoai();
                ThucHienHuy();
            }
            catch (Exception ex) { ThongBaoXacThuc = $"Lỗi: {ex.Message}"; }
        }
    }

    private void ThucHienHuy()
    {
        LoaiDuocChon = null;
        SuaTen = string.Empty;
        SuaMoTa = string.Empty;
        SuaDangHoatDong = true;
        DangSua = false;
    }
}
