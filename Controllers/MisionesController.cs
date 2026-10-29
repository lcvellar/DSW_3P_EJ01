using DSW_3P_EJ01.Models;
using DSW_3P_EJ01.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_EJ01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MisionesController : ControllerBase
{
    private readonly IMisionService _servicio;

    public MisionesController(IMisionService servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public ActionResult<List<Mision>> ObtenerTodos() => Ok(_servicio.ObtenerTodos());

    [HttpGet("{id:int}")]
    public ActionResult<Mision> ObtenerPorId(int id)
    {
        var mision = _servicio.ObtenerPorId(id);
        return mision is null ? NotFound() : Ok(mision);
    }

    [HttpPost]
    public ActionResult<Mision> Agregar([FromBody] Mision mision)
    {
        try
        {
            var creada = _servicio.Agregar(mision);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Actualizar(int id, [FromBody] Mision mision)
    {
        try
        {
            return _servicio.Actualizar(id, mision) ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Eliminar(int id) =>
        _servicio.Eliminar(id) ? NoContent() : NotFound();
}
