using System.Windows.Input;
using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;

namespace ToDoList_Start.VistaModelo.Categorias.Emergentes
{
    public class EmergenteColorCategoriaVM : BaseVistaModelo
    {
        public EmergenteColorCategoriaVM(INavigation navigation)
        {
            Navigation = navigation;
        }

        public Color CalculateTextColorForBackground(Color backgroundColor)
        {
            // Calcula el contraste entre el color de fondo y blanco (1.0) o negro (0.0)
            double contrast = (backgroundColor.G * 0.299 + backgroundColor.R * 0.587 + backgroundColor.B * 0.114) > 0.5 ? 0.0 : 1.0;

            // Retorna blanco o negro según el contraste
            return contrast == 0.0 ? Color.Black : Color.White;
        }

        private Color _colorElegido;

        public Color ColorElegido
        {
            get { return _colorElegido; }
            set
            {
                if (_colorElegido != value)
                {
                    _colorElegido = value;
                    OnPropertyChanged(nameof(ColorElegido));
                    ColorHexAceptar = _colorElegido.ToHex();
                    ColorElegidoInvertido = CalculateTextColorForBackground(ColorElegido);
                }
            }
        }

        private Color _colorElegidoInvertido;

        public Color ColorElegidoInvertido
        {
            get { return _colorElegidoInvertido; }
            set
            {
                if (_colorElegidoInvertido != value)
                {
                    _colorElegidoInvertido = value;
                    OnPropertyChanged(nameof(ColorElegidoInvertido));
                }
            }
        }

        private Color _colorAnterior;

        public Color ColorAnterior
        {
            get { return _colorAnterior; }
            set
            {
                if (_colorAnterior != value)
                {
                    _colorAnterior = value;
                    OnPropertyChanged(nameof(ColorAnterior));
                    ColorHexCerrar = _colorAnterior.ToHex();
                    ColorAnteriorInvertido = CalculateTextColorForBackground(ColorAnterior);

                }
            }
        }

        private Color _colorAnteriorInvertido;

        public Color ColorAnteriorInvertido
        {
            get { return _colorAnteriorInvertido; }
            set
            {
                if (_colorAnteriorInvertido != value)
                {
                    _colorAnteriorInvertido = value;
                    OnPropertyChanged(nameof(ColorAnteriorInvertido));
                }
            }
        }

        public Action<Color> ColorElegidoCallback { get; set; }

        private async Task AceptarColor()
        {
            ColorElegidoCallback?.Invoke(ColorElegido);
            await Navigation.PopAsync();
        }

        public ICommand AceptarColorCommand => new Command(async () => await AceptarColor());

        private async Task CerrarColor()
        {
            await Navigation.PopAsync();
        }

        public ICommand CerrarColorCommand => new Command(async () => await CerrarColor());


        private string _colorHexAceptar;

        public string ColorHexAceptar
        {
            get { return ColorElegido.ToHex(); }
            set
            {
                if (_colorHexAceptar != value)
                {
                    _colorHexAceptar = value;
                    OnPropertyChanged(nameof(ColorHexAceptar));
                }
            }
        }

        private string _colorHexCerrar;

        public string ColorHexCerrar
        {
            get { return ColorElegido.ToHex(); }
            set
            {
                if (_colorHexCerrar != value)
                {
                    _colorHexCerrar = value;
                    OnPropertyChanged(nameof(ColorHexCerrar));
                }
            }
        }
    }
}
