using CRUDMantenimiento.Models;
using System.Data.SqlClient;
using System.Data;
using System.Net;

namespace CRUDMantenimiento.Datos
{
    public class UsuarioDatos
    {
        public List<UsuarioModel> Listar()
        {
            var oLista = new List<UsuarioModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("listar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) {
                        oLista.Add(new UsuarioModel()
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Dni = Convert.ToInt32(dr["Dni"]),
                            Nombre = dr["Nombre"].ToString(),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            Sueldo = Convert.ToDecimal(dr["Sueldo"])
                        });
                    }
                }
            }
            return oLista;
        }


        public UsuarioModel Obtener(int Id)
        {
            var oUsuario = new UsuarioModel();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("obtener", conexion);
                cmd.Parameters.AddWithValue("Id", Id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oUsuario.Id = Convert.ToInt32(dr["Id"]);
                        oUsuario.Dni = Convert.ToInt32(dr["Dni"]);
                        oUsuario.Nombre = dr["Nombre"].ToString();
                        oUsuario.Fecha = Convert.ToDateTime(dr["Fecha"]);
                        oUsuario.Sueldo = Convert.ToDecimal(dr["Sueldo"]);
                    }
                }
            }
            return oUsuario;
        }


        public bool Guardar(UsuarioModel oUsuario)
        {
            bool rpta;

            try {

                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("guardar", conexion);
                    cmd.Parameters.AddWithValue("Dni", oUsuario.Dni);
                    cmd.Parameters.AddWithValue("Nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("Fecha", oUsuario.Fecha);
                    cmd.Parameters.AddWithValue("Sueldo", oUsuario.Sueldo);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                rpta = true;

            }
            catch (Exception e)
            {
                string error = e.Message;
                rpta=false;
            }

            return rpta;
        }


        public bool Editar(UsuarioModel oUsuario)
        {
            bool rpta;

            try
            {

                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("editar", conexion);
                    cmd.Parameters.AddWithValue("Id", oUsuario.Id);
                    cmd.Parameters.AddWithValue("Dni", oUsuario.Dni);
                    cmd.Parameters.AddWithValue("Nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("Fecha", oUsuario.Fecha);
                    cmd.Parameters.AddWithValue("Sueldo", oUsuario.Sueldo);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                rpta = true;

            }
            catch (Exception e)
            {
                string error = e.Message;
                rpta = false;
            }

            return rpta;
        }


        public bool Eliminar(int Id)
        {
            bool rpta;

            try
            {

                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("eliminar", conexion);
                    cmd.Parameters.AddWithValue("Id",Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                rpta = true;

            }
            catch (Exception e)
            {
                string error = e.Message;
                rpta = false;
            }

            return rpta;
        }

        public UsuarioModel ObtenerPorDni(int dni)
        {
            var oUsuario = new UsuarioModel();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("reporte_usuario", conexion);
                cmd.Parameters.AddWithValue("@DNI", dni);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        oUsuario.Id = Convert.ToInt32(dr["Id"]);
                        oUsuario.Dni = Convert.ToInt32(dr["DNI"]);
                        oUsuario.Nombre = dr["Nombre"].ToString();
                        oUsuario.Fecha = Convert.ToDateTime(dr["Fecha"]);
                        oUsuario.Sueldo = Convert.ToDecimal(dr["Sueldo"]);
                    }
                    else
                    {
                        // Si no se encuentra ningún usuario con el DNI proporcionado, devuelve null
                        oUsuario = null;
                    }
                }
            }

            return oUsuario;
        }
    }
}
