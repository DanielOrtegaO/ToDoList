﻿using ToDoList_Start.VistaModelo.Categorias.Emergentes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList_Start.Vistas.Categorias.Emergentes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmergenteIconosCategorias : ContentPage
    {
        public EmergenteIconosCategorias()
        {
            InitializeComponent();
            BindingContext = new EmergenteIconosCategoriasVM(Navigation);
        }
    }
}