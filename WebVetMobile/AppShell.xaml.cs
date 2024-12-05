using WebVetMobile.Pages;
using WebVetMobile.Services;
using WebVetMobile.Validations;

namespace WebVetMobile
{
    public partial class AppShell : Shell
    {
        private readonly ApiService _apiService;
        private readonly IValidator _validator;
        public AppShell(ApiService apiService, IValidator validator)
        {
            InitializeComponent();
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _validator = validator;
            ConfigureShell();
        }


        private void ConfigureShell()
        {
            var homePage = new AppointmentHistory(_apiService, _validator);
            var animalsPage = new Animals(_apiService, _validator);
            var teamPage = new TeamPage(_apiService, _validator);
            var perfilPage = new ProfilePage(_apiService, _validator);

            Items.Add(new TabBar
            {
                Items =
            {
                new ShellContent { Title = "Appointments",Icon = "home",Content = homePage  },
                new ShellContent { Title = "Animals",Icon = "home",Content = animalsPage  },
                new ShellContent { Title = "The Team", Icon = "cart",Content = teamPage },
                //new ShellContent { Title = "Favoritos",Icon = "heart",Content = favoritosPage },
                new ShellContent { Title = "Perfil",Icon = "profile",Content = perfilPage }
            }
            });
        }
    }
}
