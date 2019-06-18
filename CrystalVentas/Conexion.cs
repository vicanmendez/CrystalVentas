using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Traemos librería para BD
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CrystalVentas
{


    
        class Conexion
        {
            //Creamos el stringConnection (cambiar el nombre del servidor y la BD según corresponda)

            private SqlConnection conexion = new SqlConnection("server=DESKTOP-DI86SQL\\SQLEXPRESS ; database=ventas ; integrated security = true");
            private SqlCommand comando;
            //DataReader para las consultas
            private SqlDataReader retorno = null;
        //construct
        public Conexion()
            {

            }
            public void abrirConexion()
            {

                conexion.Open();
            }

            public void cerrarConexion()
            {
                conexion.Close();
            }

            //ejecuta una sentencia en la conexion establecida
            //Precondicion: conexion creada
            public void ejecutarSentencia(String query)
            {
                comando = new SqlCommand(query, conexion);
                comando.ExecuteNonQuery();

            }

            //ejecuta una sentencia en la conexion establecida
            //Precondicion: conexion creada
            public Boolean ingresarCliente(String nombre, String direccion, String contacto)
            {
            Boolean result = true;
            
            try
                {
            
            //limpiamos el dataReader
                 retorno.Close();
            /* como la ID no está auto incrementada en la BD (error del DBA que es tan vago que prefirió dejarlo así)
             debemos obtener primero la última ID registrada así le asignamos automáticamente el número siguiente */
                if (retorno != null)
                    retorno = this.ejecutarProcedimiento("verUltimoCliente");
                
                //El reader retorno ya tendrá almacenada la última ID pues así está programado el P.A 
                //List<int> elementos = new List<string>();
                int LastID = 0;
                List<int> elementos = new List<int>();
                while (retorno.Read())
                {
                    elementos.Add((int)retorno[0]);
                }
                LastID = elementos[0];
                //Ahora la ID del nuevo cliente será 1 más que la última regisrada
                LastID++;
                //preparamos el comando
                String query = "INSERT INTO cliente(idCliente, nombre, direccion, contacto) VALUES (" + LastID + ", '" + nombre + "', '" + direccion + "', '" + contacto + "');";
                //MessageBox.Show("Cadena a insertar: " + query);
                Console.WriteLine(query);
                SqlCommand comando = new SqlCommand(query, conexion);
                //Limpiamos el dataReader retorno si ya existe
                if (retorno != null)
                    retorno.Close();
                comando.ExecuteNonQuery();
                cerrarConexion();
            } catch(Exception ex)
                {
                
                result = false;
                } 
                
            return result;
 
            }


        //ejecuta una sentencia en la conexion establecida
        //Precondicion: conexion creada
        public Boolean ingresarVenta(int idCliente, String descripcion, int monto)
        {
            Boolean result = true;

            try
            {
                //preparamos el comando
                String query = "INSERT INTO venta(idCliente, monto, descripcion, momento) VALUES (" + idCliente + ", " + monto + ", '" + descripcion + "', GETDATE());";
                MessageBox.Show("Cadena a insertar: " + query);
                Console.WriteLine(query);
                SqlCommand comando = new SqlCommand(query, conexion);
                //Limpiamos el dataReader retorno si ya existe
                if (retorno != null)
                    retorno.Close();
                comando.ExecuteNonQuery();
                cerrarConexion();
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;

        }


        //ejecuta unprocedimiento almacenado
        //Precondicion: conexion creada
        public void llenarTablaConProcedimiento(String nombreProcedimiento, DataTable tabla)
            {
                comando = new SqlCommand(nombreProcedimiento, conexion);
                //asignar nombre del procedure al commandText
                comando.CommandText = nombreProcedimiento;
                //el commandtipe debe ser procedimiento almacenado
                comando.CommandType = CommandType.StoredProcedure;
                //adaptador para ejecutar el comando y traer los datos a una tabla
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                //llenamos el dataTable
                adaptador.Fill(tabla);
               

        }

            //ejecuta UNA CONSULTA COMÚN
            //Precondicion: conexion creada
            public void llenarTablaConConsulta(String tipoConsulta, DataTable tabla)
            {
                //Crear el SqlCommand con la consulta
                comando = new SqlCommand("SELECT * FROM turno WHERE tipoTurno='" + tipoConsulta + "';", conexion);
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                //llenamos el dataTable
                adaptador.Fill(tabla);
                

        }

        //ejecuta unprocedimiento almacenado 
        //Precondicion: conexion creada
        public SqlDataReader ejecutarProcedimiento(String nombreProcedimiento)
        {
            
            comando = new SqlCommand(nombreProcedimiento, conexion);
            //el commandtipe debe ser procedimiento almacenado
            comando.CommandType = CommandType.StoredProcedure;
            //Limpiamos el dataReader para no generar conflictos
            if(retorno != null)
                 retorno.Close();
            //Utilizamos ExecuteReader porque nos debe traer resultados
            retorno = comando.ExecuteReader();
            


            return retorno;
        }


        //ejecuta unprocedimiento almacenado CON UN PARÁMETRO 
        //Precondicion: conexion creada
        public void llenarTablaConProcedimientoParametro(String parametro, String valor, String nombreProcedimiento, DataTable tabla)
            {
                comando = new SqlCommand(nombreProcedimiento, conexion);
                //asignar nombre del procedure al commandText
                comando.CommandText = nombreProcedimiento;
                //el commandtipe debe ser procedimiento almacenado
                comando.CommandType = CommandType.StoredProcedure;
                //definimos el parámetro para agregarlo al procedimiento
                SqlParameter argumento = new SqlParameter();
                // SqlParameter argumento = new SqlParameter("@"+parametro, SqlDbType.VarChar);
                //Nombre y tipo del parámetro
                argumento.ParameterName = "@" + parametro;
                argumento.SqlDbType = SqlDbType.VarChar;
                //Valor del parámetro
                argumento.SqlValue = valor;
                //Agregar argumento al comando
                comando.Parameters.Add(argumento);
                //adaptador para ejecutar el comando y traer los datos a una tabla
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                //llenamos el dataTable
                adaptador.Fill(tabla);
                

        }

        


    

            public void ejecutarProcedimientoPorNombre(String nombreProcedimiento, String nombre, DataTable dt)
            {
                comando = new SqlCommand(nombreProcedimiento, conexion);
                //el commandtipe debe ser procedimiento almacenado
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(comando);

                //definimos el parámetro
                SqlParameter parametro = new SqlParameter("@nombre", SqlDbType.VarChar);
                parametro.Value = nombre;
                //Añadimos el parámetro al comando
                comando.Parameters.Add(parametro);
                comando.ExecuteNonQuery();
                adapter.Fill(dt);
                

        }


        }
 }


