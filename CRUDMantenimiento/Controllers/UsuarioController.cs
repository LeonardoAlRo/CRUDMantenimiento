using Microsoft.AspNetCore.Mvc;
using CRUDMantenimiento.Datos;
using CRUDMantenimiento.Models;
using ClosedXML.Excel;

namespace CRUDMantenimiento.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioDatos _ContactoDatos = new UsuarioDatos();
        public IActionResult Listar(int? dni)
        {
            var oLista = _ContactoDatos.Listar();

            if (dni.HasValue)
            {
                // Si se proporciona un número de DNI, realiza la búsqueda por DNI
                var usuario = _ContactoDatos.ObtenerPorDni(dni.Value);

                // Crea una lista temporal con un solo usuario para mostrar en la vista
                oLista = new List<UsuarioModel> { usuario };
            }

            return View(oLista);
        }

        public IActionResult Guardar()
        {   

            return View();
        }

        [HttpPost]
        public IActionResult Guardar(UsuarioModel oUsuario)
        {   //Si es incorrecto el estado(Los datos del formulario)
            if (!ModelState.IsValid)
                return View();

            var respuesta = _ContactoDatos.Guardar(oUsuario);
            if(respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }

        public IActionResult Editar(int Id)
        {
            var ousuario = _ContactoDatos.Obtener(Id);
            return View(ousuario);
        }

        [HttpPost]
        public IActionResult Editar(UsuarioModel oUsuario)
        {   //Si es incorrecto el estado(Los datos del formulario)
            if (!ModelState.IsValid)
                return View();

            var respuesta = _ContactoDatos.Editar(oUsuario);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }

        public IActionResult Eliminar(int Id)
        {
            var ousuario = _ContactoDatos.Obtener(Id);
            return View(ousuario);
        }

        [HttpPost]
        public IActionResult Eliminar(UsuarioModel oUsuario)
        {   //Si es incorrecto el estado(Los datos del formulario)

            var respuesta = _ContactoDatos.Eliminar(oUsuario.Id);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }


        public IActionResult BuscarPorDni(int? dni)
        {
            List<UsuarioModel> usuarios;

            if (dni.HasValue)
            {
                var usuario = _ContactoDatos.ObtenerPorDni(dni.Value);

                if (usuario != null)
                {
                    // Si se encontró un usuario, crea una lista con un solo elemento (el resultado de la búsqueda por DNI)
                    usuarios = new List<UsuarioModel> { usuario };
                }
                else
                {
                    // Si no se encontró ningún usuario, muestra un mensaje de error
                    TempData["Mensaje"] = "No se encontró ningún usuario con el DNI proporcionado.";
                    usuarios = _ContactoDatos.Listar();
                }
            }
            else
            {
                // Si no se proporciona un número de DNI, carga todos los usuarios
                usuarios = _ContactoDatos.Listar();
            }

            // Pasa la lista de usuarios a la vista
            return View("Listar", usuarios);
        }

        public IActionResult ExportarExcel(int? dni)
        {
            List<UsuarioModel> usuarios;

            if (dni.HasValue)
            {
                // Si se proporciona un número de DNI, exporta solo ese usuario
                // ¡¡¡¡¡¡¡¡inhabilitado, no se logra capturar el dni para pasarlo!!!!!!!!
                var usuario = _ContactoDatos.ObtenerPorDni(dni.Value);
                usuarios = new List<UsuarioModel> { usuario };
            }
            else
            {
                // Si no se proporciona un número de DNI, exporta todos los usuarios
                usuarios = _ContactoDatos.Listar();
            }

            using (var workbook = new XLWorkbook())
            {   
                //titulo
                var worksheet = workbook.Worksheets.Add("Usuarios");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "DNI";
                worksheet.Cell(1, 3).Value = "Nombre";
                worksheet.Cell(1, 4).Value = "Fecha";
                //posicion del titulo
                worksheet.Range("A1:D1").Style.Font.Bold = true;
                // Agregar datos al libro de Excel
                int row = 2;
                foreach (var usuario in usuarios)
                {
                    worksheet.Cell(row, 1).Value = usuario.Id;
                    worksheet.Cell(row, 2).Value = usuario.Nombre;
                    worksheet.Cell(row, 3).Value = usuario.Dni;
                    worksheet.Cell(row, 4).Value = usuario.Fecha;
                    row++;
                }
                string fechaActual = DateTime.Now.ToString("yyyyMMddHHmmss");
                // Guardar el libro de Excel en un MemoryStream
                using (var stream = new System.IO.MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    // Devolver el archivo Excel como una descarga
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"usuarios_{fechaActual}.xlsx");
                }
            }
        }
    }
}
