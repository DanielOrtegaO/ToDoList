using SQLite;
using System.Drawing;

namespace ToDoList_Start.Modelos.Datos
{
    public class Categoria
    {
        [PrimaryKey, AutoIncrement]
        public int IdCategoria { get; set; }

        [MaxLength(50)]
        public string NombreCategoria { get; set; }
        public string IconoCategoria { get; set; }
        public string ColorCategoria { get; set; }

        public int OrdenTareas { get; set; }
        public Categoria(string nombreCategoria, string iconoCategoria, string colorCategoria, int ordenTareas)
        {
            NombreCategoria = nombreCategoria;
            IconoCategoria = iconoCategoria;
            ColorCategoria = colorCategoria;
            OrdenTareas = ordenTareas;
        }

        public Categoria() { }
    }
}

