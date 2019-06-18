using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrystalVentas
{
    public partial class Form1 : Form
    {
        //Conexion para la BD
        private Conexion c = new Conexion();
        //Lista para nombre de clientes
        private List<string> nombres = new List<string>();
        //Lista para ID de clientes
        List<int> idClientes = new List<int>();
        //Cliente Seleccionado por defecto
        private int defaultIdClient = 0;

        public Form1()
        {
            InitializeComponent();
            cargarComboClientes();
        }

        //Método para cargar el comboBox de los clientes desde la BD
        public void cargarComboClientes()
        {
            //En la BD, hay un procedimiento almacenado (P.A) con el nombre "verListaNombreClientes"
            c.abrirConexion();
            //consulta almacenará los resultados de la consulta ejecutada en el P.A
            SqlDataReader consulta = c.ejecutarProcedimiento("verListaClientes");
            //Guardamos los clientes en su respectiva lista
            //Mientras existan resultados de la consulta
            while (consulta.Read())
            {
                /*
                 * COMO EN LA BD primero aparece la idCliente, luego el nombre, el índice 0 del array del dataSource
                 * será la ID, y el 1 será el nombre
                 */
                //A CADA RESULTADO LO AGREGAMOS A LA LISTA
                nombres.Add(consulta[1].ToString());
                //También actualizamos la lista de ids
                idClientes.Add((int)consulta[0]);
            }
            //Por último, cargamos el comboBox con la lista de nombres
            comboCliente.DataSource = nombres;
        }


        //Al cliquear en registrar nuevo cliente
        private void button1_Click(object sender, EventArgs e)
        {
            String nombre = txtNombre.Text;
            String direccion = txtDireccion.Text;
            String contacto = txtContacto.Text;
            //MessageBox.Show("Se van a ingresar: " + nombre + " " + direccion + " " + contacto);
            //enviamos el nuevo registro a la Bd
            
            try
            {
            
                //Debe estar prebiamente abierta la conexión c
                if(c.ingresarCliente(nombre, direccion, contacto))
                {
                    //Si la conexión fue exitosa, debemos volver a recargar el comboBox de los clientes
                    MessageBox.Show("Cliente ingresado satisfactoriamente");
                    cargarComboClientes();
                }
         
        }catch(Exception ex)
            {
                MessageBox.Show("Error al ingresar datos: " + ex.Message);
            }
            
        }

        //Cuando se selecciona otro cliente en el combo de cliente debemos apuntar a su respectiva ID para hacer el ingreso
        private void comboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Obtenemos la ID de la Lista de Ids, en el índice marcado en el combo
            defaultIdClient = idClientes[comboCliente.SelectedIndex];
            //MessageBox.Show("Seleccionado Id: " + defaultIdClient);
        }


        //Se desea ingresar una nueva venta
        private void button2_Click(object sender, EventArgs e)
        {
            //La ID del cliente seleccionado ya la capturamos en otro evento con la variable defaultIdCLient
            String descripcion = txtDescripcion.Text;
            int monto =Int32.Parse(txtMonto.Text);
            
            try
            {
            
                //Debe estar prebiamente abierta la conexión c
                if (c.ingresarVenta(defaultIdClient, descripcion, monto))
                {
                    //insert exitoso
                    MessageBox.Show("Venta ingresada satisfactoriamente");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ingresar datos: " + ex.Message);
            }
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Lanzamos el form que tiene el CrystalReportViewer, cuyo Source es el CrystalReport que creamos a parte "ResumenVentas"
            Reportes r = new Reportes();
            r.Visible = true;
        }
    }
 }

