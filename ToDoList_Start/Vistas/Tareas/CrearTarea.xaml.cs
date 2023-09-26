using System;
using ToDoList_Start.VistaModelo.Tareas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList_Start.Vistas.Tareas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CrearTarea : ContentPage
	{
		public CrearTarea ()
		{
			InitializeComponent ();
            BindingContext = new CrearTareaVM(Navigation);
        }
    }
}