using ToDoList_Start.VistaModelo.Tareas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList_Start.Vistas.Tareas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TareasPrincipal : ContentPage
	{
		public TareasPrincipal ()
		{
			InitializeComponent ();
			BindingContext = new TareasPrincipalVM(Navigation);
		}
	}
}