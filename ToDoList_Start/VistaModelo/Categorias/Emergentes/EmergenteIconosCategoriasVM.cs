using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ToDoList_Start.VistaModelo.Categorias.Emergentes
{
    public class EmergenteIconosCategoriasVM : BaseVistaModelo
    {
        private Color _colorIconos;

        public Color ColorIconos
        {
            get { return _colorIconos; }
            set
            {
                if (_colorIconos != value)
                {
                    _colorIconos = value;
                    OnPropertyChanged(nameof(ColorIconos));
                }
            }
        }

        private Color _colorOpacidad;

        public Color ColorOpacidad
        {
            get { return _colorOpacidad; }
            set
            {
                if (_colorOpacidad != value)
                {
                    _colorOpacidad = value;
                    OnPropertyChanged(nameof(ColorOpacidad));
                }
            }
        }

        private string _iconoElegido;

        public string IconoElegido
        {
            get { return _iconoElegido; }
            set
            {
                if (_iconoElegido != value)
                {
                    _iconoElegido = value;
                    OnPropertyChanged(nameof(IconoElegido));
                }
            }
        }

        public ObservableCollection<string> ListaIconos { get; set; }
        public EmergenteIconosCategoriasVM(INavigation navigation)
        {

            Navigation = navigation;

            ListaIconos = new ObservableCollection<String>()
            {

                WebUtility.HtmlDecode("&#xE01C;"), //Documento
                WebUtility.HtmlDecode("&#xE011;"), //Casita
                WebUtility.HtmlDecode("&#xE01D;"), //Persona
                WebUtility.HtmlDecode("&#xE003;"), //Personas 
                WebUtility.HtmlDecode("&#xE013;"), //Reloj

                WebUtility.HtmlDecode("&#xE016;"), //Coche
                WebUtility.HtmlDecode("&#xE014;"), //Avión
                WebUtility.HtmlDecode("&#xE00F;"), //!\
                WebUtility.HtmlDecode("&#xE015;"), //Cubiertos
                WebUtility.HtmlDecode("&#xE012;"), //Pata mascota

                WebUtility.HtmlDecode("&#xE00C;"), //Mancuerna
                WebUtility.HtmlDecode("&#xE01A;"), //Rugby
                WebUtility.HtmlDecode("&#xE007;"), //Secador pelo
                WebUtility.HtmlDecode("&#xE00E;"), //Birrete
                WebUtility.HtmlDecode("&#xE00D;"), //Billete

                WebUtility.HtmlDecode("&#xE00B;"), //Nota musical
                WebUtility.HtmlDecode("&#xE002;"), //Paleta
                WebUtility.HtmlDecode("&#xE019;"), //Claqueta
                WebUtility.HtmlDecode("&#xE004;"), //Libro
                WebUtility.HtmlDecode("&#xE022;"), //Mando consola
                
                WebUtility.HtmlDecode("&#xE00A;"), //Bolos
                WebUtility.HtmlDecode("&#xE009;"), //Ordenador
                WebUtility.HtmlDecode("&#xE018;"), //Herramientas
                WebUtility.HtmlDecode("&#xE008;"), //Pastilla
                WebUtility.HtmlDecode("&#xE000;"), //Carro compra

                WebUtility.HtmlDecode("&#xE006;"), //Maleta
                WebUtility.HtmlDecode("&#xE01B;"), //Tarta
                WebUtility.HtmlDecode("&#xE01E;"), //Cabeza pensando
                WebUtility.HtmlDecode("&#xE021;"), //Bombilla
                WebUtility.HtmlDecode("&#xE005;"), //Variados

                WebUtility.HtmlDecode("&#xE001;"), //Corazón
                WebUtility.HtmlDecode("&#xE010;"), //Estrella
                WebUtility.HtmlDecode("&#xE020;"), //Rayo
                WebUtility.HtmlDecode("&#xE017;"), //Pacman
                WebUtility.HtmlDecode("&#xE01F;") //Alien
                
            };

            
        }

        public Action<string> IconoElegidoCallback { get; set; }

        private async Task AceptarIcono()
        {
            IconoElegidoCallback?.Invoke(IconoElegido);
            await Navigation.PopAsync();
        }

        public ICommand AceptarIconoCommand => new Command(async () => await AceptarIcono());

        private async Task CerrarIcono()
        {
            await Navigation.PopAsync();
        }

        public ICommand CerrarIconoCommand => new Command(async () => await CerrarIcono());
    }
}
