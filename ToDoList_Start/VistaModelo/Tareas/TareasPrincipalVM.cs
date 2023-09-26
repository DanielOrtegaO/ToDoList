using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoList_Start.Modelos.Datos;
using ToDoList_Start.VistaModelo.MicroTareas;
using ToDoList_Start.Vistas.MicroTareas;
using ToDoList_Start.Vistas.Tareas;
using Xamarin.Forms;

namespace ToDoList_Start.VistaModelo.Tareas
{
    class TareasPrincipalVM : BaseVistaModelo
    {
        public TareasPrincipalVM(INavigation navigation)
        {
            Navigation = navigation;
            ColeccionTareas = new ObservableCollection<Tarea>();
            _desmarcar = false;
            IconoMarcar = WebUtility.HtmlDecode("&#xE000;");
                //ordenTexto[CategoriaTareas.OrdenTareas];
            //{
            //    new Tarea("No decir nada",false,3,2,DateTime.Now,false),
            //    new Tarea("Estornudar",false,3,2,DateTime.Now,false),
            //    new Tarea("Dar cosas",false,3,2,DateTime.Now,false)
            //};
            //LanzarActualizarTareas();
        }

        private ObservableCollection<Tarea> _coleccionTareas;
        public ObservableCollection<Tarea> ColeccionTareas
        {
            get { return _coleccionTareas; }
            set
            {
                _coleccionTareas = value;
                OnPropertyChanged(nameof(ColeccionTareas));
            }
        }

        public ICommand ActualizarTareasCommand => new Command(async () => await ActualizarColeccionTareas());

        public void LanzarActualizarTareas()
        {
            ActualizarColeccionTareas();
        }

        string[] ordenTexto =
{
        "Orden de creación",
        "Prioridad",
        "Alfabéticamente",
        "Fecha"
        };

        public async Task ActualizarColeccionTareas()
        {
            ColeccionTareas.Clear();
            var tareaList = await App.SQLiteDB.GetListaTareasPorCategoriaAsync(CategoriaTareas);

            switch (CategoriaTareas.OrdenTareas)
            {
                case 0://Por orden de adición
                    tareaList = tareaList.OrderBy(x => x.IdTarea).ToList();
                    break;
                case 1: //Por prioridad
                    tareaList = tareaList.OrderByDescending(x => x.Prioridad).ToList();
                    break;
                case 2: //Alfabéticamente
                    tareaList = tareaList.OrderBy(x => x.NombreTarea).ToList();
                    break;
                case 3: //Por fecha
                    tareaList = tareaList.OrderBy(x => x.FechaFin).ToList();
                    break;
                default: //Por orden de adición
                    tareaList = tareaList.OrderBy(x => x.IdTarea).ToList();
                    break;
            }

            //tareaList = tareaList.OrderBy(t => t.NombreTarea).ToList();

            _desmarcar = true;
            IconoMarcar = WebUtility.HtmlDecode("&#xE003;");
            foreach (Tarea tarea in tareaList)
            {
                if (tarea.Realizada == false)
                {
                    _desmarcar = false;
                    IconoMarcar = WebUtility.HtmlDecode("&#xE000;");
                }
                ColeccionTareas.Add(tarea);
            }
            TextoOrden = ordenTexto[CategoriaTareas.OrdenTareas];
            ListaVacia = (ColeccionTareas.Count == 0);
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

        public void ActualizadorTareasExternoEventHandler(object sender, EventArgs e)
        {
            ActualizarColeccionTareas();
        }

        public ICommand EscogerOrdenTareasCommand => new Command(async () => await EscogerOrdenTareas());

        private string _textOrden;

        public string TextoOrden
        {
            get { return _textOrden; }
            set
            {
                _textOrden = value;
                OnPropertyChanged(nameof(TextoOrden));
            }
        }
        
        public async Task EscogerOrdenTareas()
        {            
            int indiceSeleccionado = Array.IndexOf(ordenTexto, await DisplayActionSheet("Elige cómo ordenar la lista:", "Cancelar", null, ordenTexto));
            if (indiceSeleccionado != -1)
            {
                CategoriaTareas.OrdenTareas = indiceSeleccionado;
                await App.SQLiteDB.GuardarCategoriaAsync(CategoriaTareas);
                await ActualizarColeccionTareas();
            }
        }

        private bool _desmarcar;

        private string _iconoMarcar;

        public string IconoMarcar
        {
            get { return _iconoMarcar; }
            set
            {
                _iconoMarcar = value;
                OnPropertyChanged(nameof(IconoMarcar));
            }
        }

        public ICommand MarcarTareasAsyncCommand => new Command<Categoria>(async (Categoria categoria) => await MarcarTareasAsync(categoria));

        public async Task MarcarTareasAsync(Categoria categoria)
        {
            int i = 0;
            if (_desmarcar)
            {
                await App.SQLiteDB.DesmarcarTodasTareasAsync(categoria);
                //foreach(Tarea tar in ColeccionTareas)
                //{
                //    ColeccionTareas.Remove(tar);
                //    tar.Realizada = false;
                //    ColeccionTareas.Insert(i, tar);
                //    i++;
                //}
                //_desmarcar = false;
                //IconoMarcar = WebUtility.HtmlDecode("&#xE003;");
            }
            else
            {
                await App.SQLiteDB.MarcarTodasTareasAsync(categoria);
                //foreach (Tarea tar in ColeccionTareas)
                //{
                //    ColeccionTareas.Remove(tar);
                //    tar.Realizada = true;
                //    ColeccionTareas.Insert(i, tar);
                //    i++;
                //}
                //_desmarcar = true;
                //IconoMarcar = WebUtility.HtmlDecode("&#xE023;");
            }
                await ActualizarColeccionTareas();
        }

        public ICommand EliminarTareaAsyncCommand => new Command<Tarea>(async (Tarea categoria) => await EliminarTareaAsync(categoria));
        public async Task EliminarTareaAsync(Tarea tarea)
        {
            var MicroTareaList = await App.SQLiteDB.GetListaMicroTareasPorTareaAsync(tarea);
            int cuenta = MicroTareaList.Count;
            bool eliminar = false;

            if (cuenta == 0)
            {
                eliminar = await DisplayAlert("Eliminar Tarea", $"¿Deseas eliminar la tarea \"{tarea.NombreTarea}\"?", "Aceptar", "Cancelar");
            }
            else if (cuenta == 1)
            {
                eliminar = await DisplayAlert("Eliminar Tarea", $"La tarea \"{tarea.NombreTarea}\" tiene 1 MicroTarea. ¿Deseas eliminarla?", "Aceptar", "Cancelar");
            }
            else
            {
                eliminar = await DisplayAlert("Eliminar Tarea", $"La tarea \"{tarea.NombreTarea}\" tiene {cuenta} MicroTareas. ¿Deseas eliminarla?", "Aceptar", "Cancelar");
            }

            if (eliminar)
            {
                await App.SQLiteDB.EliminarTareaYMicroTareasAsync(tarea, MicroTareaList);
                ColeccionTareas.Remove(tarea);
                ListaVacia = (ColeccionTareas.Count == 0);
                //await ActualizarCategorias();
            }
        }

        public string IconoPrioridad()
        {
            return WebUtility.HtmlDecode("&#xE003;");
        }

        public ICommand EliminarTareasAsyncCommand => new Command<Categoria>(async (Categoria categoria) => await EliminarTareasAsync(categoria));

        public async Task EliminarTareasAsync(Categoria categoria)
        {
            if (await DisplayAlert("Borrar", $"¿Eliminar todas las tareas marcadas?", "Aceptar", "Cancelar"))
            {
                DisplayAlert("Debug", $"Se han borrado {await App.SQLiteDB.EliminarTareasHechasAsync(categoria)} tareas", "Ok"); ;
                await ActualizarColeccionTareas();
            }
        }

        private Categoria _categoriaTareas;

        public Categoria CategoriaTareas
        {
            get { return _categoriaTareas; }
            set
            {
                _categoriaTareas = value;
                OnPropertyChanged(nameof(CategoriaTareas));
            }
        }

        //public ICommand CheckTareaAsyncCommand => new Command<Categoria>(async (Categoria categoria) => await AbrirEditarCategoriaAsync(categoria));

        public ICommand ActualizarTareaBDCommand => new Command<Tarea>(async (tarea) => await ActualizarTareaBD(tarea));

        public async Task ActualizarTareaBD(Tarea tarea)
        {
            if (tarea.Realizada == false)
            {
                _desmarcar = false;
                IconoMarcar = WebUtility.HtmlDecode("&#xE000;");
            } else
            {
                if (!_desmarcar)
                {
                    _desmarcar = true;
                    IconoMarcar = WebUtility.HtmlDecode("&#xE003;");
                    foreach (Tarea tar in ColeccionTareas)
                    {
                        if (!tar.Realizada)
                        {
                            _desmarcar = false;
                            IconoMarcar = WebUtility.HtmlDecode("&#xE003;");
                        }
                    }
                }
            }
            await App.SQLiteDB.ActualizarTareaAsync(tarea);
        }

        public ICommand ActualizarTareasBDCommand => new Command(async () => await ActualizarTareasBD());

        public async Task ActualizarTareasBD()
        {
            await App.SQLiteDB.ActualizarTareasAsync(ColeccionTareas);
            await ActualizarColeccionTareas();
        }

        void OnCheck(object sender, CheckedChangedEventArgs e)
        {
            //App.SQLiteDB.ActualizarTareasAsync(ColeccionTareas);
        }

        //public Task<int> UpdateAllAsync(IEnumerable objects, bool runInTransaction = true)
        //{
        //    return WriteAsync((SQLiteConnectionWithLock conn) => conn.UpdateAll(objects, runInTransaction));
        //}

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

        public ICommand AbrirNuevaTareaAsyncCommand => new Command(async () => await AbrirNuevaTareaAsync());

        private bool ventanaAbriendo = false;
        public async Task AbrirNuevaTareaAsync()
        {
            if (ventanaAbriendo)
                return;

            ventanaAbriendo = true;
            var crearTarea = new CrearTarea();
            var crearTareaVM = crearTarea.BindingContext as CrearTareaVM;
            crearTareaVM.TituloCrearTarea = "Nueva Tarea";
            crearTareaVM.CategoriaNuevaTarea = CategoriaTareas;
            //Color col = Color.FromHex(CategoriaTareas.ColorCategoria);
            crearTareaVM.ActualizadorTareas += ActualizadorTareasExternoEventHandler;
            crearTareaVM.ColorOpacidad = ColorOpacidad;
            crearTareaVM.Editar = false; 
            await Navigation.PushAsync(crearTarea);
            ventanaAbriendo = false;
        }

        public ICommand AbrirEditarTareaAsyncCommand => new Command<Tarea>(async (Tarea tarea) => await AbrirEditarTareaAsync(tarea));

        public async Task AbrirEditarTareaAsync(Tarea tarea)
        {
            if (ventanaAbriendo)
                return;
            ventanaAbriendo = true;

            var editarTarea = new CrearTarea();
            var editarTareaVM = editarTarea.BindingContext as CrearTareaVM;
            editarTareaVM.TareaEditar = tarea;

            editarTareaVM.CategoriaNuevaTarea = CategoriaTareas;
            editarTareaVM.NombreTareaCampo = tarea.NombreTarea;
            editarTareaVM.PrioridadTarea = tarea.Prioridad;
            editarTareaVM.FechaFinCampo = tarea.FechaFin;
            editarTareaVM.HoraFinCampo = tarea.FechaFin.TimeOfDay;
            
            editarTareaVM.TituloCrearTarea = "Editar Tarea";
            editarTareaVM.ActualizadorTareas += ActualizadorTareasExternoEventHandler;
            editarTareaVM.ColorOpacidad = ColorOpacidad;
            editarTareaVM.Editar = true;
            await Navigation.PushAsync(editarTarea);
            ventanaAbriendo = false;
        }

        public event EventHandler ActualizadorMicroTareas;

        public ICommand AbrirMicroTareasAsyncCommand => new Command<Tarea>(async (tarea) => await AbrirMicroTareasAsync(tarea));
        public async Task AbrirMicroTareasAsync(Tarea tarea)
        {
            if (ventanaAbriendo)
                return;

            ventanaAbriendo = true;
            var abrirMicroTareas = new MicroTareasPrincipal();
            var abrirMicroTareasVM = abrirMicroTareas.BindingContext as MicroTareasPrincipalVM;
            abrirMicroTareasVM.TareaMicroTareas = tarea;
            abrirMicroTareasVM.CategoriaMicroTareas = CategoriaTareas;
            //abrirMicroTareasVM.NombreMicroTareaCampo = new Entry();
            ActualizadorMicroTareas += abrirMicroTareasVM.ActualizadorMicroTareasExternoEventHandler;
            ActualizadorMicroTareas?.Invoke(this, EventArgs.Empty);
            await Navigation.PushAsync(abrirMicroTareas);
            ventanaAbriendo = false;
        }
    }
}
