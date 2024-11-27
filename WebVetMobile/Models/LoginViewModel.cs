using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVetMobile.Models
{
    public partial class LoginViewModel : ObservableObject, INotifyPropertyChanging
    {
        [ObservableProperty]
        string? username;

        [ObservableProperty]
        string? password;
    }
}
