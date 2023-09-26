using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoList_Start.Modelos.Datos;
using Xamarin.Forms;
using ToDoList_Start.Vistas.Categorias;
using System;
using ToDoList_Start.Vistas.Tareas;
using ToDoList_Start.VistaModelo.Tareas;
using System.Threading;

namespace ToDoList_Start.VistaModelo.Categorias
{
    public class CategoriasPrincipalVM : BaseVistaModelo
    {
        private static ObservableCollection<Categoria> _coleccionCategorias;
        public ObservableCollection<Categoria> ColeccionCategorias
        {
            get { return _coleccionCategorias; }
            set
            {
                _coleccionCategorias = value;
                OnPropertyChanged(nameof(ColeccionCategorias));
            }
        }

        public CategoriasPrincipalVM(INavigation navigation)
        {
            Navigation = navigation;
            //ColeccionCategorias = new ObservableCollection<Categoria> { new Categoria("Default", WebUtility.HtmlDecode("&#xE011;"), "#FFFF3F01") };
            ColeccionCategorias = new ObservableCollection<Categoria>();
            ActualizarCategorias();        
        }

        public ICommand ActualizarCategoriasCommand => new Command(async () => await ActualizarCategorias());

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

        private string _cantidadTareas;
        public string CantidadTareas
        {
            get { return _cantidadTareas; }
            set
            {
                if (_cantidadTareas != value)
                {
                    _cantidadTareas = value;
                    OnPropertyChanged(nameof(CantidadTareas));
                }
            }
        }

        public async Task ActualizarCategorias()
        {
            ColeccionCategorias.Clear();
            var categoriaList = await App.SQLiteDB.GetListaCategoriasAsync();
            foreach (Categoria categoria in categoriaList)
            {
                ColeccionCategorias.Add(categoria);
            }
            ListaVacia = (ColeccionCategorias.Count == 0);
        }

        //ELIMINAR CATEGORIA
        public ICommand EliminarCategoriaAsyncCommand => new Command<Categoria>(async (Categoria categoria) => await EliminarCategoriaAsync(categoria));

        public async Task EliminarCategoriaAsync(Categoria categoria)
        {
            var tareaList = await App.SQLiteDB.GetListaTareasPorCategoriaAsync(categoria);
            int cuenta = tareaList.Count;
            bool eliminar = false;

            if (cuenta == 0)
            {
                eliminar = await DisplayAlert("Eliminar Categoría", $"¿Deseas eliminar la categoría \"{categoria.NombreCategoria}\"?", "Aceptar", "Cancelar");
            }
            else if (cuenta == 1)
            {
                eliminar = await DisplayAlert("Eliminar Categoría", $"La categoría \"{categoria.NombreCategoria}\" tiene 1 tarea. ¿Deseas eliminarla?", "Aceptar", "Cancelar");
            }
            else
            {
                eliminar = await DisplayAlert("Eliminar Categoría", $"La categoría \"{categoria.NombreCategoria}\" tiene {cuenta} tareas. ¿Deseas eliminarla?", "Aceptar", "Cancelar");
            }

            if (eliminar)
            {
                await App.SQLiteDB.EliminarCategoriaYTareasAsync(categoria,tareaList);
                ColeccionCategorias.Remove(categoria);
                ListaVacia = (ColeccionCategorias.Count == 0);
                //await ActualizarCategorias();
            }            
        }

        //ABRIR VENTANA CATEGORIAS

        private bool ventanaAbriendo = false;

        public ICommand AbrirNuevaCategoriaAsyncCommand => new Command(async () => await AbrirNuevaCategoriaAsync());
        public async Task AbrirNuevaCategoriaAsync()
        {
            if (ventanaAbriendo)
                return;

            ventanaAbriendo = true;
            var nuevaCategoria = new CrearCategoria();
            var nuevaCategoriaVM = nuevaCategoria.BindingContext as CrearCategoriaVM;
            nuevaCategoriaVM.TituloCrearCategoria = "Nueva Categoría";
            nuevaCategoriaVM.ActualizadorCategorias += ActualizadorCategoriasExternoEventHandler;
            await Navigation.PushAsync(nuevaCategoria);
            
            ventanaAbriendo = false;
        }

        private async void ActualizadorCategoriasExternoEventHandler(object sender, EventArgs e)
        {
            await ActualizarCategorias();
        }

        public ICommand AbrirEditarCategoriaAsyncCommand => new Command<Categoria>(async (Categoria categoria) => await AbrirEditarCategoriaAsync(categoria));

        public async Task AbrirEditarCategoriaAsync(Categoria categoria)
        {
            if (ventanaAbriendo)
                return;
            ventanaAbriendo = true;

            var editarCategoria = new CrearCategoria();
            var editarCategoriaVM = editarCategoria.BindingContext as CrearCategoriaVM;
            editarCategoriaVM.CategoriaEditar = categoria;
            editarCategoriaVM.NombreCategoriaCampo = categoria.NombreCategoria;
            editarCategoriaVM.ColorCategoriaCampo = Color.FromHex(categoria.ColorCategoria);
            editarCategoriaVM.IconoCampo = categoria.IconoCategoria;
            editarCategoriaVM.TituloCrearCategoria = "Editar Categoría";
            editarCategoriaVM.ActualizadorCategorias += ActualizadorCategoriasExternoEventHandler;
            await Navigation.PushAsync(editarCategoria);
            ventanaAbriendo = false;
        }
        //------------------------------------------


        #region TAREAS

        public event EventHandler ActualizadorTareas;
        public async Task AbrirTareasPrincipalAsync(Categoria categoria)
        {
            if (ventanaAbriendo)
                return;
            ventanaAbriendo = true;

            var abrirTareas = new TareasPrincipal();
            var abrirTareasVM = abrirTareas.BindingContext as TareasPrincipalVM;
            abrirTareasVM.CategoriaTareas = categoria;
            Color col = Color.FromHex(categoria.ColorCategoria);
            abrirTareasVM.ColorOpacidad = Color.FromRgba(col.R, col.G, col.B, 0.5);
            ActualizadorTareas += abrirTareasVM.ActualizadorTareasExternoEventHandler;
            ActualizadorTareas?.Invoke(this, EventArgs.Empty);
            await Navigation.PushAsync(abrirTareas);
            ventanaAbriendo = false;
        }

        public ICommand AbrirTareasPrincipalAsyncCommand => new Command<Categoria>(async (categoria) => await AbrirTareasPrincipalAsync(categoria));
        #endregion
    }
}

