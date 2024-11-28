using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVetMobile.Models
{
    public partial class User : ObservableObject, INotifyPropertyChanging
    {
        public string Id { get; set; }

        [ObservableProperty]
        string? email;

        [ObservableProperty]
        string? firstName;

        [ObservableProperty]
        string? lastName;


        [ObservableProperty]
        string? address;

        [ObservableProperty]
        int contact;

        [ObservableProperty]
        Guid imageId;

        public string ImageFullPath => imageId == Guid.Empty
    ? $"https://supersho2tpsi.blob.core.windows.net/users/noimage.png"
    : $"https://supersho2tpsi.blob.core.windows.net/users/{imageId}";
    }
}
