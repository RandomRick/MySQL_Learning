using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient; // RJH


namespace MySQL_Learning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}

// all my stuff here
namespace MySQL_Learning
{
    public partial class Form1 : Form
    {
        private string myConnectionString;
        private MySql.Data.MySqlClient.MySqlConnection conn;


        private void button1_Click(object sender, EventArgs e)
        {
            // preliminaries
            (sender as Button).Enabled = false;
            textBox1.AppendText("Connecting" + Environment.NewLine);

            // build the connection string
            myConnectionString =
                string.Format("server={0};uid={1};pwd={2};database={3}",
                tbHostname.Text,
                tbUser.Text,
                tbPassword.Text,
                tbDatabase.Text
                );
#if DEBUG
            textBox1.AppendText("DEBUG: ");
            textBox1.AppendText("connection string : \""
                + myConnectionString
                + "\"" + Environment.NewLine);
#endif

            // attempt connection
            try
            {
                conn = new MySqlConnection(myConnectionString);
                conn.Open();
#if DEBUG
                textBox1.AppendText("DEBUG: successful connection" + Environment.NewLine);
#endif
                btnInsert.Enabled = true;
                btnRead.Enabled = true;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        textBox1.AppendText("ERROR: ");
                        textBox1.AppendText("Cannot connect to server.  Contact administrator" + Environment.NewLine);
                        break;
                    case 1045:
                        textBox1.AppendText("ERROR: ");
                        textBox1.AppendText("Invalid username/password, please try again" + Environment.NewLine);
                        break;
                    default:
                        textBox1.AppendText("ERROR: ");
                        textBox1.AppendText(ex.Message);
                        break;
                }
            }

            // conclusion
            (sender as Button).Enabled = true;
        }





        private void btnRead_Click(object sender, EventArgs e)
        {
            // https://dev.mysql.com/doc/connector-net/en/connector-net-tutorials-sql-command.html
            //string sql = @"SELECT * FROM et_pump_500hr.channel_4_flowrate;";
            string sql = @"SELECT * FROM channel_4_flowrate;";

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string report = String.Format("{0} -- {1} -- {2} -- {3}",
                        rdr[0], rdr[1], rdr[2], rdr[3]
                        ) + Environment.NewLine;
                    textBox1.AppendText(report);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                textBox1.AppendText("ERROR: ");
                textBox1.AppendText(ex.Message);
            }
        }




        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"INSERT INTO channel_4_flowrate (`ID`, `dt`, `cycles`, `flow`) VALUES ('3', '2020-10-03', '99', '2.236067')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                textBox1.AppendText("ERROR: ");
                textBox1.AppendText(ex.Message);
            }

        }

    }
}
