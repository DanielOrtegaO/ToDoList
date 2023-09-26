using ToDoList_Start.VistaModelo.Categorias;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList_Start.Vistas.Categorias
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriasPrincipal : ContentPage
    {
        public CategoriasPrincipal()
        {
            InitializeComponent();
            BindingContext = new CategoriasPrincipalVM(Navigation);
        }
    }
}