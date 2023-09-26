using ToDoList_Start.VistaModelo.Categorias;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList_Start.Vistas.Categorias
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrearCategoria : ContentPage
    {
        public CrearCategoria()
        {
            InitializeComponent();
            BindingContext = new CrearCategoriaVM(Navigation);
        }
    }
}