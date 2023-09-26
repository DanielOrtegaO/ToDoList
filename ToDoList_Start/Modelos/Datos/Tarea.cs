using SQLite;
using System;

namespace ToDoList_Start.Modelos.Datos
{
    public class Tarea
    {
        [PrimaryKey, AutoIncrement]
        public int IdTarea { get; set; }
        [MaxLength(100)]
        public string NombreTarea { get; set; }
        public bool Realizada { get; set; }
        public int IdCategoria { get; set; }
        public int Prioridad { get; set; }
        public string IconoPrioridad { get; set; }
        public DateTime FechaFin { get; set; }
        bool Notificar { get; set; }

        public Tarea(string nombreTarea, bool realizada, int idCategoria,
            int prioridad, string iconoPrioridad,
            DateTime fechaFin, bool notificar)
        {
            NombreTarea = nombreTarea;
            Realizada = realizada;
            IdCategoria = idCategoria;
            Prioridad = prioridad;
            IconoPrioridad = iconoPrioridad;
            FechaFin = fechaFin;
            Notificar = notificar;
        }

        public Tarea() { }

        public override string ToString()
        {
            return $"N:{NombreTarea}, P:{Prioridad}, FF:{FechaFin}, R:{Realizada}";
        }
    }
}
