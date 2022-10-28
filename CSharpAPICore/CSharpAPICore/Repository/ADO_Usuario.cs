using CSharpAPICore.Models;
using System.Data.SqlClient;

namespace CSharpAPICore.Repository
{
    public class ADO_Usuario
    {
        public static List<Usuario> DevolverUsuarios()
        {
            var listaUsuarios = new List<Usuario>();

            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                SqlCommand cmd2 = connection.CreateCommand();
                cmd2.CommandText = "SELECT * FROM USUARIO";
                var reader2 = cmd2.ExecuteReader();

                while (reader2.Read())
                {
                    var usuario = new Usuario();

                    usuario.Id = Convert.ToInt32(reader2.GetValue(0));
                    usuario.Nombre = reader2.GetValue(1).ToString();
                    usuario.Apellido = reader2.GetValue(2).ToString();
                    usuario.NombreUsuario = reader2.GetValue(3).ToString();
                    usuario.Contraseña = reader2.GetValue(4).ToString();
                    usuario.Mail = reader2.GetValue(5).ToString();

                    listaUsuarios.Add(usuario);
                }

                reader2.Close();

                connection.Close();
            }

            return listaUsuarios;
        }

        internal static Usuario DevolverUsuario(int id)
        {
            var usuario = new Usuario();

            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Usuario WHERE Id = @id";

                // Crear parametro
                var param = new SqlParameter();
                param.ParameterName = "id";
                param.Value = id;
                param.SqlDbType = System.Data.SqlDbType.BigInt;

                // Agregar parametro
                cmd.Parameters.Add(param);

                var reader2 = cmd.ExecuteReader();

                while (reader2.Read())
                {
                    usuario.Id = Convert.ToInt32(reader2.GetValue(0));
                    usuario.Nombre = reader2.GetValue(1).ToString();
                    usuario.Apellido = reader2.GetValue(2).ToString();
                    usuario.NombreUsuario = reader2.GetValue(3).ToString();
                    usuario.Contraseña = reader2.GetValue(4).ToString();
                    usuario.Mail = reader2.GetValue(5).ToString();
                }

                reader2.Close();

                connection.Close();
            }

            return usuario;
        }

        internal static void AgregarUsuario(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = 
                    "INSERT INTO Usuario (Id, Nombre, Apellido, NombreUsuario, Contraseña, Mail) " +
                    "VALUES (@id, @nombre, @apellido, @nombreUsuario, @contraseña, @mail)";

                // Crear parametros
                var paramId = new SqlParameter("id", System.Data.SqlDbType.BigInt);
                paramId.Value = usuario.Id;

                var paramNombre = new SqlParameter("nombre", System.Data.SqlDbType.VarChar);
                paramNombre.Value = usuario.Nombre;

                var paramApellido = new SqlParameter("apellido", System.Data.SqlDbType.VarChar);
                paramApellido.Value = usuario.Apellido;

                var paramNombreUsuario = new SqlParameter("nombreUsuario", System.Data.SqlDbType.VarChar);
                paramNombreUsuario.Value = usuario.NombreUsuario;

                var paramContraseña = new SqlParameter("contraseña", System.Data.SqlDbType.VarChar);
                paramContraseña.Value = usuario.Contraseña;

                var paramEmail = new SqlParameter("mail", System.Data.SqlDbType.VarChar);
                paramEmail.Value = usuario.Mail;

                // Agregar parametro
                cmd.Parameters.Add(paramId);
                cmd.Parameters.Add(paramNombre);
                cmd.Parameters.Add(paramApellido);
                cmd.Parameters.Add(paramNombreUsuario);
                cmd.Parameters.Add(paramContraseña);
                cmd.Parameters.Add(paramEmail);

                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static void ModificarUsuario(Usuario usuario)
        {
            // Buscar usuario para ver si existe
            var usuarioBuscado = DevolverUsuario(usuario.Id);

            if (usuarioBuscado.Id == 0)
                return;

            // Si existe Reemplazar Usuario
            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText =
                    "UPDATE Usuario SET Nombre = @nombre, Apellido = @apellido, " +
                    "NombreUsuario = @nombreUsuario, Contraseña = @contraseña, Mail = @mail " +
                    "WHERE Id = @id";

                // Crear parametros
                var paramId =
                    new SqlParameter("id", System.Data.SqlDbType.BigInt);
                paramId.Value = usuario.Id;

                var paramNombre = 
                    new SqlParameter("nombre", System.Data.SqlDbType.VarChar);
                paramNombre.Value = usuario.Nombre;

                var paramApellido = 
                    new SqlParameter("apellido", System.Data.SqlDbType.VarChar);
                paramApellido.Value = usuario.Apellido;

                var paramNombreUsuario = 
                    new SqlParameter("nombreUsuario", System.Data.SqlDbType.VarChar);
                paramNombreUsuario.Value = usuario.NombreUsuario;

                var paramContraseña = 
                    new SqlParameter("contraseña", System.Data.SqlDbType.VarChar);
                paramContraseña.Value = usuario.Contraseña;

                var paramEmail = 
                    new SqlParameter("mail", System.Data.SqlDbType.VarChar);
                paramEmail.Value = usuario.Mail;

                // Agregar parametro
                cmd.Parameters.Add(paramId);
                cmd.Parameters.Add(paramNombre);
                cmd.Parameters.Add(paramApellido);
                cmd.Parameters.Add(paramNombreUsuario);
                cmd.Parameters.Add(paramContraseña);
                cmd.Parameters.Add(paramEmail);

                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static void EliminarUsuario(int id)
        {
            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Usuario WHERE Id = @id";

                // Crear parametro
                var param = new SqlParameter();
                param.ParameterName = "id";
                param.Value = id;
                param.SqlDbType = System.Data.SqlDbType.BigInt;

                // Agregar parametro
                cmd.Parameters.Add(param);
                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// Recibe como parámetro un nombre del usuario, buscarlo en la base de datos y 
        /// devolver el objeto con todos sus datos (Esto se hará para la página en la que 
        /// se mostrara los datos del usuario y en la página para modificar sus datos).
        /// </summary>
        internal static Usuario TraerUsuario(string nombreUsuario)
        {
            Console.WriteLine("\nTraerUsuario");

            Usuario usuario = new();

            using (SqlConnection connection = new(General.ConnectionToString()))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * " +
                    "FROM Usuario" +
                    " WHERE NombreUsuario = @nombreUsuario";

                // Crear parametro
                var param =
                    new SqlParameter("nombreUsuario", System.Data.SqlDbType.VarChar);
                param.Value = nombreUsuario;

                // Agregar el parametro
                cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader.GetValue(0));
                    usuario.Nombre = reader.GetValue(1).ToString();
                    usuario.Apellido = reader.GetValue(2).ToString();
                    usuario.NombreUsuario = reader.GetValue(3).ToString();
                    usuario.Contraseña = reader.GetValue(4).ToString();
                    usuario.Mail = reader.GetValue(5).ToString();
                }

                reader.Close();
                connection.Close();
            }

            return usuario;
        }

        /// <summary>
        /// Se le pase como parámetro el nombre del usuario y la contraseña, buscar 
        /// en la base de datos si el usuario existe y si coincide con la contraseña 
        /// lo devuelve (el objeto Usuario), caso contrario devuelve uno vacío (Con 
        /// sus datos vacíos y el id en 0).
        /// </summary>
        internal static Usuario InicioDeSesion(string nombreUsuario, string contraseña)
        {
            Console.WriteLine("\nInicioDeSesion");

            Usuario usuario = TraerUsuario(nombreUsuario);

            if (usuario.Contraseña == contraseña)
                return usuario;
            else
                return new Usuario();
        }
    }
}
