using ToDoList_Start.VistaModelo.MicroTareas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList_Start.Vistas.MicroTareas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MicroTareasPrincipal : ContentPage
	{
		public MicroTareasPrincipal ()
		{
			InitializeComponent ();
			BindingContext = new MicroTareasPrincipalVM(Navigation);
            MicroTareasPrincipalVM.NombreMicroTareaEntry = NombreMicroTareaCampo;
			//NombreMicroTareaCampo.Focus();
		}
	}
}