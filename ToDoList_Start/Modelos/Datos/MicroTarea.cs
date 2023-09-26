using SQLite;

namespace ToDoList_Start.Modelos.Datos
{
    public class MicroTarea
    {
        [PrimaryKey, AutoIncrement]
        public int IdMicroTarea { get; set; }
        [MaxLength(100)]
        public string NombreMicroTarea { get; set; }
        public int IdTarea { get; set; }
        public bool Realizada { get; set; }

        public MicroTarea(string nombreMicroTarea, int idTarea, bool realizada)
        {
            NombreMicroTarea = nombreMicroTarea;
            IdTarea = idTarea;
            Realizada = realizada;
        }

        public MicroTarea() { }

    }
}
