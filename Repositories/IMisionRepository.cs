using DSW_3P_EJ01.Models;

namespace DSW_3P_EJ01.Repositories;

// Este contrato se conserva al pasar de JSON a SQL Server.
public interface IMisionRepository
{
    List<Mision> ObtenerTodos();
    Mision? ObtenerPorId(int id);
    Mision Agregar(Mision mision);
    bool Actualizar(int id, Mision mision);
    bool Eliminar(int id);
}
