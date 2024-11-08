using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



public partial class BaseViewModel : ObservableObject, INotifyPropertyChanging
{

    public BaseViewModel()
    {

    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(isNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string userName;

    [ObservableProperty]
    string password;

    public bool isNotBusy => !IsBusy;


    //public event PropertyChangedEventHandler? PropertyChanged;

    //bool isBusy;


    //public bool IsBusy
    //{
    //    get { return isBusy; }
    //    set
    //    {
    //        if (isBusy == value) { return; }
    //        isBusy = value;
    //        OnPropertyChanged();
    //        OnPropertyChanged(nameof(IsNotBusy));
    //    }
    //}

    //public bool IsNotBusy => !IsBusy;
    //public void OnPropertyChanged([CallerMemberName] string name = null)
    //{

    //    if (PropertyChanged == null)
    //    {
    //        return;
    //    }

    //    PropertyChanged(this, new PropertyChangedEventArgs(name));
    //}
}
