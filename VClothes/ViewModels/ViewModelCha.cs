using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace VClothes.ViewModels;

public abstract class ViewModelCha : INotifyPropertyChanged
{
    private bool _dangTai;

    public bool DangTai
    {
        get => _dangTai;
        set => GanGiaTri(ref _dangTai, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void ThongBaoThayDoi([CallerMemberName] string? tenThuocTinh = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(tenThuocTinh));
    }

    protected bool GanGiaTri<T>(ref T truong, T giaTri, [CallerMemberName] string? tenThuocTinh = null)
    {
        if (EqualityComparer<T>.Default.Equals(truong, giaTri))
            return false;

        truong = giaTri;
        ThongBaoThayDoi(tenThuocTinh);
        return true;
    }
}

public class LenhRelay : ICommand
{
    private readonly Action<object?> _thucThi;
    private readonly Func<object?, bool>? _coTheThucThi;

    public LenhRelay(Action<object?> thucThi, Func<object?, bool>? coTheThucThi = null)
    {
        _thucThi = thucThi ?? throw new ArgumentNullException(nameof(thucThi));
        _coTheThucThi = coTheThucThi;
    }

    public LenhRelay(Action thucThi, Func<bool>? coTheThucThi = null)
        : this(_ => thucThi(), coTheThucThi != null ? _ => coTheThucThi() : null)
    {
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => _coTheThucThi?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => _thucThi(parameter);
}
