using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList_Start.Modelos.Datos;

namespace ToDoList_Start.Modelos.BaseDatos
{
    public class SQLiteHelper
    {
        readonly SQLiteAsyncConnection bd;

        public SQLiteHelper(string bdPath)
        {
            bd = new SQLiteAsyncConnection(bdPath);
            bd.CreateTableAsync<Tarea>().Wait();
            bd.CreateTableAsync<Categoria>().Wait();
            bd.CreateTableAsync<MicroTarea>().Wait();
        }

        #region CATEGORIAS

        public async Task<int> GuardarCategoriaAsync(Categoria categoria)
        {
            if (categoria.IdCategoria != 0)
            {
                return await bd.UpdateAsync(categoria);
            }
            else
            {
                return await bd.InsertAsync(categoria);
            }
        }

        public async Task<int> EliminarCategoriaAsync(Categoria categoria)
        {
            var tareaList = await App.SQLiteDB.GetListaTareasPorCategoriaAsync(categoria);
            foreach (Tarea tarea in tareaList)
            {
                await EliminarTareaAsync(tarea);
            }
            await bd.DeleteAsync(categoria);
            return tareaList.Count;
        }

        public async Task<int> EliminarCategoriaYTareasAsync(Categoria categoria, List<Tarea> tareaList)
        {
            foreach (Tarea tarea in tareaList)
            {
                await EliminarTareaAsync(tarea);
            }
            await bd.DeleteAsync(categoria);
            return tareaList.Count;
        }

        public Task<List<Categoria>> GetListaCategoriasAsync()
        {
            return bd.Table<Categoria>().ToListAsync();
        }

        public async Task<Categoria> GetCategoriaByIdAsync(int idCategoria)
        {
            return await bd.Table<Categoria>().Where(x => x.IdCategoria == idCategoria).FirstOrDefaultAsync();
        }
        #endregion

        #region TAREAS
        public async Task<int> GuardarTareaAsync(Tarea tarea)
        {
            if (tarea.IdTarea != 0)
            {
                return await bd.UpdateAsync(tarea);
            }
            else
            {
                return await bd.InsertAsync(tarea);
            }
        }

        public async Task<int> ActualizarTareaAsync(Tarea tarea)
        {
            return await bd.UpdateAsync(tarea);

        }

        public async Task<int> ActualizarTareasAsync(IEnumerable<Tarea> lista)
        {
            return await bd.UpdateAllAsync(lista,true);
        }

        //public Task<int> UpdateAllAsync(IEnumerable objects, bool runInTransaction = true)
        //{
        //    return WriteAsync((SQLiteConnectionWithLock conn) => conn.UpdateAll(objects, runInTransaction));
        //}

        public Task<List<Tarea>> GetListaTareasAsync()
        {
            return bd.Table<Tarea>().ToListAsync();
        }

        public Task<List<Tarea>> GetListaTareasPorCategoriaAsync(Categoria categoria)
        {
            return bd.Table<Tarea>().Where(x => x.IdCategoria == categoria.IdCategoria).ToListAsync();
        }

        public async Task<int> MarcarTodasTareasAsync(Categoria categoria)
        {
            var tareaList = await App.SQLiteDB.GetListaTareasPorCategoriaAsync(categoria);
            foreach (Tarea tarea in tareaList)
            {
                tarea.Realizada = true;
                await bd.UpdateAsync(tarea);
            }
            return tareaList.Count;
        }

        public async Task<int> DesmarcarTodasTareasAsync(Categoria categoria)
        {
            var tareaList = await App.SQLiteDB.GetListaTareasPorCategoriaAsync(categoria);
            foreach (Tarea tarea in tareaList)
            {
                tarea.Realizada = false;
                await bd.UpdateAsync(tarea);
            }
            return tareaList.Count;
        }

        public Task<List<Tarea>> GetListaTareasHechasPorCategoriaAsync(int idCategoria)
        {
            return bd.Table<Tarea>().Where(x => x.IdCategoria == idCategoria && x.Realizada == true).ToListAsync();
        }

        public async Task<int> EliminarTareasHechasAsync(Categoria categoria)
        {
            var tareaList = await App.SQLiteDB.GetListaTareasHechasPorCategoriaAsync(categoria.IdCategoria);
            foreach (Tarea tarea in tareaList)
            {
                await EliminarTareaAsync(tarea);
            }
            return tareaList.Count;
        }

        public async Task<int> EliminarTareaAsync(Tarea tarea)
        {
            var microTareaList = await App.SQLiteDB.GetListaMicroTareasPorTareaAsync(tarea);
            foreach (MicroTarea microTarea in microTareaList)
            {
                await bd.DeleteAsync(microTarea);
            }
            return await bd.DeleteAsync(tarea);
        }

        public async Task<int> EliminarTareaYMicroTareasAsync(Tarea tarea, List<MicroTarea> microTareaList)
        {
            //var microTareaList = await App.SQLiteDB.GetListaMicroTareasPorTareaAsync(tarea);
            foreach (MicroTarea microTarea in microTareaList)
            {
                await bd.DeleteAsync(microTarea);
            }
            return await bd.DeleteAsync(tarea);
        }

        public async Task<Tarea> GetTareaByIdAsync(int idTarea)
        {
            return await bd.Table<Tarea>().Where(x => x.IdTarea == idTarea).FirstOrDefaultAsync();
        }
        #endregion

        #region MICROTAREAS

        public Task<int> GuardarMicroTareaAsync(MicroTarea microTarea)
        {
            if (microTarea.IdMicroTarea != 0)
            {
                return bd.UpdateAsync(microTarea);
            }
            else
            {
                return bd.InsertAsync(microTarea);
            }
        }

        public async Task<int> ActualizarMicroTareaAsync(MicroTarea microTarea)
        {
            return await bd.UpdateAsync(microTarea);

        }

        public async Task<int> EliminarMicroTareaAsync(MicroTarea microTarea)
        {
            return await bd.DeleteAsync(microTarea);
        }

        public Task<List<MicroTarea>> GetListaMicroTareasAsync()
        {
            return bd.Table<MicroTarea>().ToListAsync();
        }

        public async Task<MicroTarea> GetMicroTareaByIdAsync(int idMicroTarea)
        {
            return await bd.Table<MicroTarea>().Where(x => x.IdMicroTarea == idMicroTarea).FirstOrDefaultAsync();
        }

        public Task<List<MicroTarea>> GetListaMicroTareasPorTareaAsync(Tarea tarea)
        {
            return bd.Table<MicroTarea>().Where(x => x.IdTarea == tarea.IdTarea).ToListAsync();
        }
        #endregion
    }
}
