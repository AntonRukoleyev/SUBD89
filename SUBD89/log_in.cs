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
    public partial class log_in : Form
    {
        DataBase dataBase = new DataBase();
        public log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void log_in_Load(object sender, EventArgs e)
        {
            textBox_password.PasswordChar = '*';
            textBox_login.MaxLength = 50;
            textBox_password.MaxLength = 50;
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            var loginUser = textBox_login.Text;
            var passUser = textBox_password.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"SELECT * FROM dbo.GetUserByCredentials(@loginUser, @passUser)";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            command.Parameters.AddWithValue("@loginUser", loginUser);
            command.Parameters.AddWithValue("@passUser", passUser);

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if(table.Rows.Count == 1)
            {
                var user = new checkUser(table.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(table.Rows[0].ItemArray[3]));

                int userID = Convert.ToInt32(table.Rows[0]["id_user"]);
                SetCurrentUserContext(userID);

                MessageBox.Show("Вы успешно вошли!" + userID.ToString(), "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 frm1 = new Form1(user);
                this.Hide();
                frm1.ShowDialog();
                this.Show();
            }
            else
                MessageBox.Show("Такого аккаунте не существует!", "Аккаунта не существует!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sign_up frm_sign = new sign_up();
            frm_sign.Show();
            this.Hide();
        }
        private void SetCurrentUserContext(int userId)
        {
            dataBase.openConnection();

            string query = $"update CurrentUser set id_user = @UserID";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());


            command.Parameters.AddWithValue("@UserID", userId);
            command.ExecuteNonQuery();
            dataBase.closeConnection();

        }
    }
}
