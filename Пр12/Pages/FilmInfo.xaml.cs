using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Пр12.Pages
{
    /// <summary>
    /// Логика взаимодействия для FilmInfo.xaml
    /// </summary>
    public partial class FilmInfo : Page
    {
        public FILM CurrentFilm { get; set; }

        public List<SESSIONS> Seans { get; set; }

        public List<GENRE> curgenre { get; set; }

        public FilmInfo()
        {
            InitializeComponent();
        }

        public FilmInfo(FILM film) : this()
        {
            CurrentFilm = film;
            DataContext = this;
            Seans = Core.Context.SESSIONS.Where(s => s.id_film == CurrentFilm.ID_FILM).ToList();

            curgenre = Core.Context.FILM_IN_GENRE.Where(g => g.id_film == CurrentFilm.ID_FILM).Select(s => s.GENRE).ToList();

            LoadFilmData();
        }

        private void LoadFilmData()
        {
            if (CurrentFilm != null)
            {
                LoadSessions(CurrentFilm.ID_FILM);
            }
        }

        private void LoadSessions(int filmId)
        {

        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService?.GoBack();
            }
        }
    }
}
