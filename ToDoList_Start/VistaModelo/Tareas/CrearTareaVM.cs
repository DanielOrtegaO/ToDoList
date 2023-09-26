using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoList_Start.Modelos.Datos;
using Xamarin.Forms;

namespace ToDoList_Start.VistaModelo.Tareas
{
    public class CrearTareaVM : BaseVistaModelo
    {
        private bool _editar;
        public bool Editar
        {
            get { return _editar; }
            set
            {
                if (_editar != value)
                {
                    _editar = value;
                    OnPropertyChanged(nameof(Editar));
                }
            }
        }

        public ICommand BorrarTareaCommand => new Command(async () => await BorrarTarea());
        private async Task BorrarTarea()
        {
            if (await DisplayAlert("Borrar", $"¿Eliminar tarea?", "Aceptar", "Cancelar"))
            {
                await App.SQLiteDB.EliminarTareaAsync(TareaEditar);
                ActualizadorTareas?.Invoke(this, EventArgs.Empty);
                await Navigation.PopAsync();
            }
        }

        public CrearTareaVM(INavigation navigation)
        {
            Navigation = navigation;
            FechaFinCampo = DateTime.Now;
            HoraFinCampo = DateTime.Now.TimeOfDay;
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

        string _tituloCrearTarea;
        public string TituloCrearTarea
        {
            get { return _tituloCrearTarea; }
            set
            {
                if (_tituloCrearTarea != value)
                {
                    _tituloCrearTarea = value;
                    OnPropertyChanged(nameof(TituloCrearTarea));
                }
            }
        }

        private Tarea _tareaEditar;

        public Tarea TareaEditar
        {
            get { return _tareaEditar; }
            set
            {
                if (_tareaEditar != value)
                {
                    _tareaEditar = value;
                    OnPropertyChanged(nameof(_tareaEditar));
                }
            }
        }

        public event EventHandler ActualizadorTareas;
        public ICommand NuevaTareaAsyncCommand => new Command(async () => await NuevaTareaAsync());
        public async Task NuevaTareaAsync()
        {
            if (NombreTareaCampo != null)
            {
                if (NombreTareaCampo.Length < 1)
                {
                    await DisplayAlert("ERROR", "El campo de \"Nombre tarea\" no debe estar vacío", "Vale");
                }
                else if (NombreTareaCampo.Length > 75)
                {
                    await DisplayAlert("ERROR", "El campo de \"Nombre tarea\" no puede tener más de 100 caracteres", "Vale");
                }
                else
                {
                    if (TareaEditar == null)
                    {
                        Tarea tarea = new Tarea(
                        NombreTareaCampo,
                        false,
                        CategoriaNuevaTarea.IdCategoria,
                        (int)Math.Round(PrioridadTarea),
                        IconoPrioridadSelector(),
                        construirFecha(),
                        false);
                        await App.SQLiteDB.GuardarTareaAsync(tarea);
                    }
                    else
                    {
                        TareaEditar.NombreTarea = NombreTareaCampo;
                        TareaEditar.Prioridad = (int)Math.Round(PrioridadTarea);
                        TareaEditar.IconoPrioridad = IconoPrioridadSelector();
                        TareaEditar.FechaFin = construirFecha();

                        await App.SQLiteDB.GuardarTareaAsync(TareaEditar);
                    }
                    ActualizadorTareas?.Invoke(this, EventArgs.Empty);
                    await CerrarCrearTarea();
                }
            }
            else
            {
                await DisplayAlert("ERROR", "El campo de \"Nombre Tarea\" no debe estar vacío", "Vale");
            }
        }

        private string IconoPrioridadSelector()
        {
           switch ((int)Math.Round(PrioridadTarea))
            {
                case 1:
                    return WebUtility.HtmlDecode("&#xE03F;");
                case 2:
                    return WebUtility.HtmlDecode("&#xE03E;");
                default:
                    return WebUtility.HtmlDecode("&#xE040;");
            }
        }

        public ICommand CerrarCrearTareaCommand => new Command(async () => await CerrarCrearTarea());
        private async Task CerrarCrearTarea()
        {
            await Navigation.PopAsync();
        }

        string _nombreTareaCampo;
        public string NombreTareaCampo
        {
            get { return _nombreTareaCampo; }
            set
            {
                if (_nombreTareaCampo != value)
                {
                    _nombreTareaCampo = value;
                    OnPropertyChanged(nameof(NombreTareaCampo));
                }
            }
        }

        private Categoria _categoriaNuevaTarea;

        public Categoria CategoriaNuevaTarea
        {
            get { return _categoriaNuevaTarea; }
            set
            {
                _categoriaNuevaTarea = value;
                OnPropertyChanged(nameof(CategoriaNuevaTarea));
            }
        }


        private double _prioridadTarea = 1;
        public double PrioridadTarea
        {
            get { return _prioridadTarea; }
            set
            {
                if (_prioridadTarea != value)
                {
                    _prioridadTarea = value;
                    OnPropertyChanged(nameof(PrioridadTarea));
                }
            }
        }

        public ICommand RedondearPrioridadCommand => new Command(RedondearPrioridad);
        private void RedondearPrioridad()
        {
            PrioridadTarea = (int)Math.Round(PrioridadTarea);
        }

        private DateTime _fechaFinCampo;

        public DateTime FechaFinCampo
        {
            get { return _fechaFinCampo; }
            set
            {
                if (_fechaFinCampo != value)
                {
                    _fechaFinCampo = value;
                    OnPropertyChanged(nameof(FechaFinCampo));
                }
            }
        }

        private TimeSpan _horaFinCampo;

        public TimeSpan HoraFinCampo
        {
            get { return _horaFinCampo; }
            set
            {
                if (_horaFinCampo != value)
                {
                    _horaFinCampo = value;
                    OnPropertyChanged(nameof(HoraFinCampo));
                }
            }
        }

        private DateTime construirFecha()
        {
            return new DateTime(FechaFinCampo.Year, FechaFinCampo.Month, FechaFinCampo.Day,
                                         HoraFinCampo.Hours, HoraFinCampo.Minutes, HoraFinCampo.Seconds);
        }
    }
}
