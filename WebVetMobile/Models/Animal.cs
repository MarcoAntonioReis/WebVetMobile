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
   public partial class Animal : ObservableObject, INotifyPropertyChanging
    {
        public int Id { get; set; }

        [ObservableProperty]
        string? name;

        [ObservableProperty]
        User? owner;

        [ObservableProperty]
        string? species;

        [ObservableProperty]
        Guid imageId;

        public string ImageFullPath => imageId == Guid.Empty
    ? $"https://supersho2tpsi.blob.core.windows.net/animals/noimage.png"
    : $"https://supersho2tpsi.blob.core.windows.net/animals/{imageId}";
    }
}
