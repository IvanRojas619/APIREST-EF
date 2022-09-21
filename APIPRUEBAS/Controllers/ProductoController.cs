using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRUEBAS.Models;
using System;

namespace APIPRUEBAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly DBAPIContext _dbcontext;

        public ProductoController(DBAPIContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                lista = _dbcontext.Productos.Include(c => c.oCategoria).ToList();

                return StatusCode(200, new { mensaje = "ok", response = lista });

            } catch (Exception e)
            {
                return StatusCode(200, new { mensaje = e.Message, response = lista });


            }
        }


        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            Producto oProducto = _dbcontext.Productos.Find(idProducto);

            if (oProducto == null)
            {
                return BadRequest("No se encontro el producto");
            }

            try
            {
                oProducto = _dbcontext.Productos.Include(c => c.oCategoria).Where(a => a.IdProducto == idProducto).FirstOrDefault();

                return StatusCode(200, new { mensaje = "ok", response = oProducto });

            }
            catch (Exception e)
            {
                return StatusCode(200, new { mensaje = e.Message, response = oProducto });


            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {
                _dbcontext.Productos.Add(objeto);
                _dbcontext.SaveChanges();
                return StatusCode(200,new {mensaje="Ok guardado con exito!"});

            }catch(Exception e)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = e.Message });
            }
    
        }


        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            Producto oProducto =_dbcontext.Productos.Find(objeto.IdProducto);

            if (oProducto == null)
            {
                return BadRequest("No se encontro el producto");
            }
            try
            {
                oProducto.CodigoBarra = objeto.CodigoBarra is null ? oProducto.CodigoBarra : objeto.CodigoBarra;
                oProducto.Descripcion = objeto.Descripcion is null ? oProducto.Descripcion : objeto.Descripcion;
                oProducto.Marca = objeto.Marca is null ? oProducto.Marca : objeto.Marca;
                oProducto.IdCategoria = objeto.IdCategoria is null ? oProducto.IdCategoria : objeto.IdCategoria;
                oProducto.Precio = objeto.Precio is null ? oProducto.Precio : objeto.Precio;


                _dbcontext.Productos.Update(oProducto);
                _dbcontext.SaveChanges();
                return StatusCode(200, new { mensaje = "Ok actualizado con exito!" });

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = e.Message });
            }

        }

        [HttpDelete]
        [Route("Eliminar/{idProducto}")]
        public IActionResult Eliminar(int idProducto)
        {

            Producto oProducto = _dbcontext.Productos.Find(idProducto);

            if (oProducto == null)
            {
                return BadRequest("No se encontro el producto");
            }
            try
            {
  
                _dbcontext.Productos.Remove(oProducto);
                _dbcontext.SaveChanges();
                return StatusCode(200, new { mensaje = "Ok Eliminado con exito!" });

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = e.Message });
            }

        }

    }
}
