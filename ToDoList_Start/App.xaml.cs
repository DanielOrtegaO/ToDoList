using System;
using System.IO;
using ToDoList_Start.Modelos.BaseDatos;
using ToDoList_Start.Vistas.Categorias;
using Xamarin.Forms;

namespace ToDoList_Start
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new CategoriasPrincipal());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        //BASE DE DATOS

        static SQLiteHelper bd;

        public static SQLiteHelper SQLiteDB
        {
            get
            {
                if (bd == null)
                {
                    bd = new SQLiteHelper(Path
                        .Combine(Environment
                        .GetFolderPath(Environment
                        .SpecialFolder
                        .LocalApplicationData), 
                        "BDTareas.db3"));
                }
                return bd;
            }
        }
    }
}
