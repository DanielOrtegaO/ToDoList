using ColorPicker;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoList_Start.Modelos.Datos;
using ToDoList_Start.VistaModelo.Categorias.Emergentes;
using ToDoList_Start.Vistas.Categorias.Emergentes;
using Xamarin.Forms;

namespace ToDoList_Start.VistaModelo.Categorias
{
    public class CrearCategoriaVM : BaseVistaModelo
    {
        public event EventHandler ActualizadorCategorias;
        public CrearCategoriaVM(INavigation navigation)
        {
            Navigation = navigation;
            ColorCategoriaCampo = Color.Gray;//new Color(127,127,127);
            IconoCampo = WebUtility.HtmlDecode("&#xE01C;");
        }

        #region COLOR

        private Color _colorCategoriaCampo;

        public Color ColorCategoriaCampo
        {
            get { return _colorCategoriaCampo; }
            set
            {
                if (_colorCategoriaCampo != value)
                {
                    _colorCategoriaCampo = value;
                    OnPropertyChanged(nameof(ColorCategoriaCampo));

                }
            }
        }

        private Categoria _categoriaEditar;

        public Categoria CategoriaEditar
        {
            get { return _categoriaEditar; }
            set
            {
                if (_categoriaEditar != value)
                {
                    _categoriaEditar = value;
                    OnPropertyChanged(nameof(_categoriaEditar));
                }
            }
        }

        //ABRIR EMERGENTE COLOR

        public async Task AbrirColor()
        {
            var emergenteColor = new EmergenteColorCategoria();
            var emergenteColorVM = emergenteColor.BindingContext as EmergenteColorCategoriaVM;

            emergenteColorVM.ColorElegido = ColorCategoriaCampo;
            emergenteColorVM.ColorAnterior = ColorCategoriaCampo;
            emergenteColorVM.ColorElegidoCallback = ColorSeleccionado;

            var colorPicker = emergenteColor.FindByName<ColorWheel>("colorPicker"); // Obtener el ColorPicker por su nombre
            colorPicker.SelectedColor = ColorCategoriaCampo; // Establecer el valor inicial del ColorPicker

            await Navigation.PushAsync(emergenteColor);
        }

        public ICommand AbrirColorCommand => new Command(async () => await AbrirColor());

        //------------
        private void ColorSeleccionado(Color color)
        {
            ColorCategoriaCampo = color;
        }

        #endregion

        #region ICONO

        private string _iconoCampo;

        public string IconoCampo
        {
            get { return _iconoCampo; }
            set
            {
                if (_iconoCampo != value)
                {
                    _iconoCampo = value;
                    OnPropertyChanged(nameof(IconoCampo));

                }
            }
        }

        //ABRIR EMERGENTE ICONO

        private async Task AbrirIconos()
        {
            var popupPage = new EmergenteIconosCategorias();
            var emergenteIconoVM = popupPage.BindingContext as EmergenteIconosCategoriasVM;

            emergenteIconoVM.ColorIconos = ColorCategoriaCampo;
            emergenteIconoVM.ColorOpacidad = Color
                .FromRgba(ColorCategoriaCampo.R, ColorCategoriaCampo.G, ColorCategoriaCampo.B,0.5);
            emergenteIconoVM.IconoElegidoCallback = IconoSeleccionado;

            await Navigation.PushAsync(popupPage);
        }

        public ICommand AbrirIconosCommand => new Command(async () => await AbrirIconos());

        private void IconoSeleccionado(string icono)
        {
            IconoCampo = icono;
        }
        #endregion

        #region CREAR_CATEGORIA

        string _nombreCategoriaCampo;
        public string NombreCategoriaCampo
        {
            get { return _nombreCategoriaCampo; }
            set
            {
                if (_nombreCategoriaCampo != value)
                {
                    _nombreCategoriaCampo = value;
                    OnPropertyChanged(nameof(NombreCategoriaCampo));
                }
            }
        }

        //

        string _tituloCrearCategoria;
        public string TituloCrearCategoria
        {
            get { return _tituloCrearCategoria; }
            set
            {
                if (_tituloCrearCategoria != value)
                {
                    _tituloCrearCategoria = value;
                    OnPropertyChanged(nameof(TituloCrearCategoria));
                }
            }
        }
        
        public ICommand NuevaCategoriaAsyncCommand => new Command(async () => await NuevaCategoriaAsync());
        public async Task NuevaCategoriaAsync()
        {
            if (NombreCategoriaCampo != null)
            {
                if (NombreCategoriaCampo.Length < 1)
                {
                    await DisplayAlert("ERROR", "El campo de \"Nombre categoria\" no debe estar vacío", "Vale");
                }
                else if (NombreCategoriaCampo.Length > 40)
                {
                    await DisplayAlert("ERROR", "El campo de \"Nombre categoria\" no puede tener más de 40 caracteres", "Vale");
                }
                else
                {
                    if (CategoriaEditar == null)
                    {
                        Categoria categoria = new Categoria(
                        NombreCategoriaCampo,
                        IconoCampo,
                        ColorCategoriaCampo.ToHex(),
                        0);
                        await App.SQLiteDB.GuardarCategoriaAsync(categoria);
                    }
                    else
                    {
                        CategoriaEditar.NombreCategoria = NombreCategoriaCampo;
                        CategoriaEditar.IconoCategoria = IconoCampo;
                        CategoriaEditar.ColorCategoria = ColorCategoriaCampo.ToHex();

                        await App.SQLiteDB.GuardarCategoriaAsync(CategoriaEditar);
                    }
                    ActualizadorCategorias?.Invoke(this, EventArgs.Empty);
                    await CerrarCrearCategoria();
                }

            }
            else
            {
                await DisplayAlert("ERROR", "El campo de \"Nombre Tarea\" no debe estar vacío", "Vale");
            }
        }

        private async Task CerrarCrearCategoria()
        {
            await Navigation.PopAsync();
        }

        public ICommand CerrarCrearCategoriaCommand => new Command(async () => await CerrarCrearCategoria());
        #endregion
    }
}

