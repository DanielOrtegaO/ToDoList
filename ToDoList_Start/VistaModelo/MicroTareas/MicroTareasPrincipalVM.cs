using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoList_Start.Modelos.Datos;
using Xamarin.Forms;

namespace ToDoList_Start.VistaModelo.MicroTareas
{
    public class MicroTareasPrincipalVM : BaseVistaModelo
    {
        public MicroTareasPrincipalVM(INavigation navigation) 
        {
            Navigation = navigation;
            ColeccionMicroTareas = new ObservableCollection<MicroTarea> {
                new MicroTarea ("Salvar al Papa",1,false),
                new MicroTarea ("Comer niños",1,false),
                new MicroTarea ("Asustarse",1,false),
                new MicroTarea ("¡DIABLOS!",1,false),
            };
            
            //ActualizarColeccionMicroTareas();
        }

        private ObservableCollection<MicroTarea> _coleccionMicroTareas;
        public ObservableCollection<MicroTarea> ColeccionMicroTareas
        {
            get { return _coleccionMicroTareas; }
            set
            {
                _coleccionMicroTareas = value;
                OnPropertyChanged(nameof(ColeccionMicroTareas));
            }
        }

        public void ActualizadorMicroTareasExternoEventHandler(object sender, EventArgs e)
        {
            ActualizarColeccionMicroTareas();
        }

        public async Task ActualizarColeccionMicroTareas()
        {
            ColeccionMicroTareas.Clear();
            var microTareaList = await App.SQLiteDB.GetListaMicroTareasPorTareaAsync(TareaMicroTareas);

            foreach (MicroTarea microTarea in microTareaList)
            {
                ColeccionMicroTareas.Add(microTarea);
            }
            ListaVacia = (ColeccionMicroTareas.Count == 0);
            //Cuenta = microTareaList.Count;
        }

        private Categoria _categoriaMicroTareas;

        public Categoria CategoriaMicroTareas
        {
            get { return _categoriaMicroTareas; }
            set
            {
                _categoriaMicroTareas = value;
                OnPropertyChanged(nameof(CategoriaMicroTareas));
            }
        }

        private Tarea _tareaMicroTareas;

        public Tarea TareaMicroTareas
        {
            get { return _tareaMicroTareas; }
            set
            {
                _tareaMicroTareas = value;
                OnPropertyChanged(nameof(TareaMicroTareas));
            }
        }

        private static Entry _nombreMicroTareaEntry;
        public static Entry NombreMicroTareaEntry
        {
            get { return _nombreMicroTareaEntry; }
            set
            {
                if (_nombreMicroTareaEntry != value)
                {
                    _nombreMicroTareaEntry = value;
                    //OnPropertyChanged(nameof(NombreMicroTareaCampo));
                }
            }
        }

        private string _nombreMicroTarea;
        public string NombreMicroTarea
        {
            get { return _nombreMicroTarea; }
            set
            {
                if (_nombreMicroTarea != value)
                {
                    _nombreMicroTarea = value;
                    OnPropertyChanged(nameof(NombreMicroTarea));
                }
            }
        }

        public ICommand ActualizarMicroTareaBDCommand => new Command<MicroTarea>(async (microTarea) => await ActualizarMicroTareaBD(microTarea));

        public async Task ActualizarMicroTareaBD(MicroTarea microTarea)
        {
            await App.SQLiteDB.ActualizarMicroTareaAsync(microTarea);
        }

        public ICommand GuardarMicroTareaBDCommand => new Command(async () => await GuardarMicroTareaAsync());

        public async Task GuardarMicroTareaAsync()
        {
            if (NombreMicroTarea != null)
            {
                if (NombreMicroTarea.Length > 0)
                {
                    //NombreMicroTareaCampo.Focus();
                    MicroTarea microTarea = new MicroTarea(
                        NombreMicroTarea,
                        TareaMicroTareas.IdTarea,
                        false);

                    await App.SQLiteDB.GuardarMicroTareaAsync(microTarea);

                    ColeccionMicroTareas.Add(microTarea);

                    NombreMicroTarea = "";
                    ListaVacia = false;
                    //NombreMicroTareaCampo.
                    //await ActualizarColeccionMicroTareas();
                }
            }
        }

        private bool _listaVacia;

        public bool ListaVacia
        {
            get { return _listaVacia; }
            set
            {
                if (_listaVacia != value)
                {
                    _listaVacia = value;
                    OnPropertyChanged(nameof(ListaVacia));
                }
            }
        }

        public ICommand BorrarMicroTareaAsyncCommand => new Command<MicroTarea>(async (microTarea) => await BorrarMicroTareaAsync(microTarea));
        private async Task BorrarMicroTareaAsync(MicroTarea microTarea)
        {
            ColeccionMicroTareas.Remove(microTarea);
            await App.SQLiteDB.EliminarMicroTareaAsync(microTarea);
            ListaVacia = (ColeccionMicroTareas.Count == 0);
        }

        public ICommand CerrarMicroTareasPrincipalCommand => new Command(async () => await CerrarMicroTareasPrincipal());
        private async Task CerrarMicroTareasPrincipal()
        {
            await Navigation.PopAsync();
        }
    }
}
