using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SUBD89
{
    public partial class sign_up : Form
    {
        DataBase dataBase = new DataBase();
        public sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }


        private void button1_Click(object sender, EventArgs e)
        {


            var login = textBox_login2.Text;
            var password = textBox_password2.Text;

            string querystring = $"EXEC dbo.InsertUser @login = '{login}', @password = '{password}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            if (!checkuser() && command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успех");
                log_in frm_login = new log_in();
                this.Hide();
                frm_login.ShowDialog();
            }
            else

                dataBase.closeConnection();
        }

        private Boolean checkuser()
        {
            var loginUser = textBox_login2.Text;
            var passUser = textBox_password2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"SELECT * FROM dbo.GetUserByCredentials(@loginUser, @passUser)";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            command.Parameters.AddWithValue("@loginUser", loginUser);
            command.Parameters.AddWithValue("@passUser", passUser);

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь существует!");
                return true;
            }
            else
                return false;

        }



        private void sign_up_Load(object sender, EventArgs e)
        {
            textBox_password2.PasswordChar = '*';
            textBox_login2.MaxLength = 50;
            textBox_password2.MaxLength = 50;
        }
    }
}
