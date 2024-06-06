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
using System.Text.RegularExpressions;

namespace SUBD89
{
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }

    public partial class Form1 : Form
    {
        private readonly checkUser _user;

        DataBase dataBase = new DataBase();

        int selectedRow_curator;
        int selectedRow_department;
        int selectedRow_diploma;
        int selectedRow_groups;
        int selectedRow_jobtitle;
        int selectedRow_student;
        int selectedRow_vuz;
        public Form1(checkUser user)
        {
            _user = user;
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            LoadJobtitles();
            LoadCurators();
            LoadDiplomas();
            LoadGroups();
            LoadStudents();
            comboBox_idjobtitle_curator.SelectedIndexChanged += combobox_jobtitle_curator_SelectedIndexChanged;
        }


        // Разрешено только для админа
        private void isAdmin()
        {
            управлениеToolStripMenuItem.Enabled = _user.IsAdmin;
        }
        private void управлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_admin pA = new panel_admin();
            pA.Show();
        }
        // Создание колонок Curator
        private void CreateColumns()
        {
            // Таблица Curator
            dataGridView1.Columns.Add("IDCurator", "IDCurator");
            dataGridView1.Columns.Add("FIO", "FIO");
            dataGridView1.Columns.Add("IDJobtitle", "IDJobtitle");
            dataGridView1.Columns.Add("IDVUZ", "IDVUZ");
            dataGridView1.Columns.Add("IsNew", String.Empty);
            dataGridView1.Columns[4].Visible = false;
            // Таблица Department
            dataGridView2.Columns.Add("IDDepart", "IDDepart");
            dataGridView2.Columns.Add("Name", "Name");
            dataGridView2.Columns.Add("IsNew", String.Empty);
            dataGridView2.Columns[2].Visible = false;
            // Таблица Diploma
            dataGridView3.Columns.Add("IDDiploma", "IDDiploma");
            dataGridView3.Columns.Add("Theme", "Theme");
            dataGridView3.Columns.Add("Mark", "Mark");
            dataGridView3.Columns.Add("IDStud", "IDStud");
            dataGridView3.Columns.Add("IDCurator", "IDCurator");
            dataGridView3.Columns.Add("IsNew", String.Empty);
            dataGridView3.Columns[5].Visible = false;
            // Таблица Groups
            dataGridView4.Columns.Add("IDGroup", "IDGroup");
            dataGridView4.Columns.Add("Name", "Name");
            dataGridView4.Columns.Add("YearOfAdmission", "YearOfAdmission");
            dataGridView4.Columns.Add("IDDepart", "IDDepart");
            dataGridView4.Columns.Add("IsNew", String.Empty);
            dataGridView4.Columns[4].Visible = false;
            // Таблица Jobtitle
            dataGridView5.Columns.Add("IDJobtitle", "IDJobtitle");
            dataGridView5.Columns.Add("Name", "Name");
            dataGridView5.Columns.Add("IsNew", String.Empty);
            dataGridView5.Columns[2].Visible = false;
            // Таблица Student
            dataGridView6.Columns.Add("IDStud", "IDStud");
            dataGridView6.Columns.Add("Surname", "Surname");
            dataGridView6.Columns.Add("Name", "Name");
            dataGridView6.Columns.Add("Patronymic", "Patronymic");
            dataGridView6.Columns.Add("RecordBook", "RecordBook");
            dataGridView6.Columns.Add("IDGroup", "IDGroup");
            dataGridView6.Columns.Add("DateBirth", "DateBirth");
            dataGridView6.Columns.Add("IsNew", String.Empty);
            dataGridView6.Columns[7].Visible = false;
            // Таблица VUZ
            dataGridView7.Columns.Add("IDVUZ", "IDVUZ");
            dataGridView7.Columns.Add("Name", "Name");
            dataGridView7.Columns.Add("Location", "Location");
            dataGridView7.Columns.Add("DateOfFoundation", "DateOfFoundation");
            dataGridView7.Columns.Add("INN", "INN");
            dataGridView7.Columns.Add("IsNew", String.Empty);
            dataGridView7.Columns[5].Visible = false;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_UserStatus.Text = $"Имя: {_user.Login}. Роль: {_user.Status}";
            isAdmin(); // Панель админа
            CreateColumns(); // Создание колонок Curator
            RefreshDataGrid_curator(dataGridView1); // Обновление таблицы Curator
            RefreshDataGrid_department(dataGridView2); // Обновление таблицы Department
            RefreshDataGrid_diploma(dataGridView3); // Обновление таблицы Diploma
            RefreshDataGrid_groups(dataGridView4); // Обновление таблицы Groups
            RefreshDataGrid_jobtitle(dataGridView5); // Обновление таблицы Jobtitle
            RefreshDataGrid_student(dataGridView6); // Обновление таблицы Student
            RefreshDataGrid_vuz(dataGridView7); // Обновление таблицы VUZ
        }

        //
        // Таблица Curator
        //


        // Заполнение таблицы Curator в приложении
        private void ReadSingleRow_curator(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), RowState.ModifiedNew);
        }
        // Обновление таблицы Curator
        private void RefreshDataGrid_curator(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string querystring = $"SELECT * FROM dbo.GetCurators()";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_curator(dgw, reader);
            }
            reader.Close();

            dataBase.closeConnection();
        }
        // Отображение информации о Curator при клике
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow_curator = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow_curator];

                textBox_idcurator.Text = row.Cells[0].Value.ToString();
                textBox_fio.Text = row.Cells[1].Value.ToString();
                textBox_idjobtitle_curator.Text = row.Cells[2].Value.ToString();
                comboBox_idjobtitle_curator.Text = row.Cells[2].Value.ToString();
                textBox_idvuz_curator.Text = row.Cells[3].Value.ToString();
                comboBox_idvuz_curator.Text = row.Cells[3].Value.ToString();
            }
        }

        // Поиск в таблице Curator
        private void Search_curator(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"SELECT * FROM dbo.SearchCurators(@searchText = '%" + textBox_search_curator.Text + "%')";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow_curator(dgw, read);
            }

            read.Close();
        }

        // Удаление строки в Curator
        private void deleteRow_curator()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
        }
        // Обновление в Curator
        private void Update_curator()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[4].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var idcurator = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"EXEC dbo.DeleteCurator @idcurator = {idcurator}";

                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var idcurator = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var fio = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var idjobtitle = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var idvuz = dataGridView1.Rows[index].Cells[3].Value.ToString();

                    var changeQuery = $"EXEC dbo.UpdateCurator @idcurator = {idcurator}, @fio = '{fio}', @idjobtitle = '{idjobtitle}', @idvuz = '{idvuz}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }
        // Изменение таблицы Curator
        private void Change_curator()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var idcurator = textBox_idcurator.Text;
            var fio = textBox_fio.Text;
            int idjobtitle;
            int idvuz;

            // Проверка поля FIO на соответствие допустимому формату
            if (!Regex.IsMatch(fio, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле FIO можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(textBox_idjobtitle_curator.Text, out idjobtitle))
                {
                    if (int.TryParse(textBox_idvuz_curator.Text, out idvuz))
                    {
                        dataGridView1.Rows[selectedRowIndex].SetValues(idcurator, fio, idjobtitle, idvuz);
                        dataGridView1.Rows[selectedRowIndex].Cells[4].Value = RowState.Modified;
                    }
                    else
                        MessageBox.Show("В IDVUZ должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("В IDJobtitle должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        // Обработчик кнопки обновления Curator
        private void button_Refresh_curator_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_curator(dataGridView1);
        }
        // Обработчик кнопки добавления записи Curator
        private void button_new_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form();
            addfrm.Show();
        }
        // Обработчик поиска Curator
        private void textBox_search_curator_TextChanged(object sender, EventArgs e)
        {
            Search_curator(dataGridView1);
        }
        // Обработчик удаления Curator
        private void button_delete_curator_Click(object sender, EventArgs e)
        {
            deleteRow_curator();
        }
        // Обработчик обновления Curator
        private void button_save_curator_Click(object sender, EventArgs e)
        {
            Update_curator();
        }
        // Обработчик изменения Curator
        private void button_change_curator_Click(object sender, EventArgs e)
        {
            Change_curator();
        }

        private void LoadJobtitles()
        {
            dataBase.openConnection();
            string query = "SELECT IDJobtitle, Name FROM Jobtitle";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                comboBox_idjobtitle_curator.Items.Add(new ComboboxItem
                {
                    Text = reader["Name"].ToString(),
                    Value = reader["IDJobtitle"].ToString()
                });
            }

            dataBase.closeConnection();
        }
        private void combobox_jobtitle_curator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_idjobtitle_curator.SelectedItem != null)
            {
                ComboboxItem selectedItem = (ComboboxItem)comboBox_idjobtitle_curator.SelectedItem;
                textBox_idjobtitle_curator.Text = selectedItem.Value;
            }
        }
        public class ComboboxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void LoadCurators()
        {
            LoadComboboxWithQuery(comboBox_idvuz_curator, "SELECT IDVUZ, Name FROM VUZ", "Name", "IDVUZ");
            comboBox_idvuz_curator.SelectedIndexChanged += (sender, e) =>
            {
                UpdateTextboxWithComboboxValue(comboBox_idvuz_curator, textBox_idvuz_curator);
            };
        }

        private void LoadDiplomas()
        {
            LoadComboboxWithQuery(comboBox_idstud_diploma, "SELECT IDStud, Surname + ' ' + Name + ' ' + Patronymic AS FullName FROM Student", "FullName", "IDStud");
            comboBox_idstud_diploma.SelectedIndexChanged += (sender, e) =>
            {
                UpdateTextboxWithComboboxValue(comboBox_idstud_diploma, textBox_idstud_diploma);
            };

            LoadComboboxWithQuery(comboBox_idcurator_diploma, "SELECT IDCurator, FIO FROM Curator", "FIO", "IDCurator");
            comboBox_idcurator_diploma.SelectedIndexChanged += (sender, e) =>
            {
                UpdateTextboxWithComboboxValue(comboBox_idcurator_diploma, textBox_idcurator_diploma);
            };
        }

        private void LoadGroups()
        {
            LoadComboboxWithQuery(comboBox_iddepart_groups, "SELECT IDDepart, Name FROM Department", "Name", "IDDepart");
            comboBox_iddepart_groups.SelectedIndexChanged += (sender, e) =>
            {
                UpdateTextboxWithComboboxValue(comboBox_iddepart_groups, textBox_iddepart_groups);
            };
        }

        private void LoadStudents()
        {
            LoadComboboxWithQuery(comboBox_idgroup_student, "SELECT IDGroup, Name FROM Groups", "Name", "IDGroup");
            comboBox_idgroup_student.SelectedIndexChanged += (sender, e) =>
            {
                UpdateTextboxWithComboboxValue(comboBox_idgroup_student, textBox_idgroup_student);
            };
        }

        private void LoadComboboxWithQuery(ComboBox comboBox, string query, string displayMember, string valueMember)
        {

            dataBase.openConnection();

            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                comboBox.Items.Add(new ComboboxItem
                {
                    Text = reader[displayMember].ToString(),
                    Value = reader[valueMember].ToString()
                });
            }

            reader.Close();
        }

    

        private void UpdateTextboxWithComboboxValue(ComboBox comboBox, TextBox textBox)
        {
            if (comboBox.SelectedItem != null)
            {
                ComboboxItem selectedItem = (ComboboxItem)comboBox.SelectedItem;
                textBox.Text = selectedItem.Value;
            }
        }
    

    //
    // Таблица Department
    //

    // Заполнение таблицы Department в приложении
    private void ReadSingleRow_department(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), RowState.ModifiedNew);
        }
        // Обновление таблицы Department
        private void RefreshDataGrid_department(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string querystring = $"SELECT * FROM dbo.GetDepartments()";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_department(dgw, reader);
            }
            reader.Close();

            dataBase.closeConnection();
        }
        // Отображение информации о Department при клике
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow_department = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[selectedRow_department];

                textBox_iddepart.Text = row.Cells[0].Value.ToString();
                textBox_departname.Text = row.Cells[1].Value.ToString();
            }
        }

        // Поиск в таблице Department
        private void Search_department(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"SELECT * FROM dbo.SearchDepartments(@searchText = '%" + textBox_search_department.Text + "%')";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow_department(dgw, read);
            }

            read.Close();
        }

        // Удаление строки в Department
        private void deleteRow_department()
        {
            int index = dataGridView2.CurrentCell.RowIndex;

            dataGridView2.Rows[index].Visible = false;

            if (dataGridView2.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView2.Rows[index].Cells[2].Value = RowState.Deleted;
                return;
            }
            dataGridView3.Rows[index].Cells[2].Value = RowState.Deleted;
        }
        // Обновление в Department
        private void Update_department()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView2.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView2.Rows[index].Cells[2].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var iddepart = Convert.ToInt32(dataGridView2.Rows[index].Cells[0].Value);
                    var deleteQuery = $"EXEC dbo.DeleteDepartment @iddepart = {iddepart}";

                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var iddepart = dataGridView2.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView2.Rows[index].Cells[1].Value.ToString();

                    var changeQuery = $"EXEC dbo.UpdateDepartment @iddepart = {iddepart}, @name = '{name}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }
        // Изменение таблицы Department
        private void Change_department()
        {
            var selectedRowIndex = dataGridView2.CurrentCell.RowIndex;

            var iddepart = textBox_iddepart.Text;
            var name = textBox_departname.Text;

            // Проверка поля FIO на соответствие допустимому формату
            if (!Regex.IsMatch(name, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле Name можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            if (dataGridView2.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {

                dataGridView2.Rows[selectedRowIndex].SetValues(iddepart, name);
                dataGridView2.Rows[selectedRowIndex].Cells[2].Value = RowState.Modified;

            }
        }
        // Обработчик кнопки обновления Department
        private void button_Refresh_department_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_department(dataGridView2);
        }
        // Обработчик кнопки добавления записи Department
        private void button_new_department_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form();
            addfrm.Show();
        }
        // Обработчик поиска Department
        private void textBox_search_department_TextChanged(object sender, EventArgs e)
        {
            Search_department(dataGridView2);
        }
        // Обработчик удаления Department
        private void button_delete_department_Click(object sender, EventArgs e)
        {
            deleteRow_department();
        }
        // Обработчик обновления Department
        private void button_save_department_Click(object sender, EventArgs e)
        {
            Update_department();
        }
        // Обработчик изменения Department
        private void button_change_department_Click(object sender, EventArgs e)
        {
            Change_department();
        }


        //
        // Таблица Diploma
        //

        // Заполнение таблицы Diploma в приложении
        private void ReadSingleRow_diploma(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), record.GetString(3), record.GetString(4), RowState.ModifiedNew);
        }
        // Обновление таблицы Diploma
        private void RefreshDataGrid_diploma(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string querystring = $"SELECT * FROM dbo.GetDiplomas()";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_diploma(dgw, reader);
            }
            reader.Close();

            dataBase.closeConnection();
        }
        // Отображение информации о Diploma при клике
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow_diploma = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView3.Rows[selectedRow_diploma];

                textBox_iddiploma.Text = row.Cells[0].Value.ToString();
                textBox_theme.Text = row.Cells[1].Value.ToString();
                textBox_mark.Text = row.Cells[2].Value.ToString();
                textBox_idstud_diploma.Text = row.Cells[3].Value.ToString();
                comboBox_idstud_diploma.Text = row.Cells[3].Value.ToString();
                textBox_idcurator_diploma.Text = row.Cells[4].Value.ToString();
                comboBox_idcurator_diploma.Text = row.Cells[4].Value.ToString();
            }
        }

        // Поиск в таблице Diploma
        private void Search_diploma(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"SELECT * FROM dbo.SearchDiplomas(@searchText = '%" + textBox_search_diploma.Text + "%')";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow_diploma(dgw, read);
            }

            read.Close();
        }

        // Удаление строки в Diploma
        private void deleteRow_diploma()
        {
            int index = dataGridView3.CurrentCell.RowIndex;

            dataGridView3.Rows[index].Visible = false;

            if (dataGridView3.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView3.Rows[index].Cells[5].Value = RowState.Deleted;
                return;
            }
            dataGridView3.Rows[index].Cells[5].Value = RowState.Deleted;
        }
        // Обновление в Diploma
        private void Update_diploma()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView3.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView3.Rows[index].Cells[5].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var iddiploma = Convert.ToInt32(dataGridView3.Rows[index].Cells[0].Value);
                    var deleteQuery = $"EXEC dbo.DeleteDiploma @iddiploma = {iddiploma}";

                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var iddiploma = dataGridView3.Rows[index].Cells[0].Value.ToString();
                    var theme = dataGridView3.Rows[index].Cells[1].Value.ToString();
                    var mark = dataGridView3.Rows[index].Cells[2].Value.ToString();
                    var idstud = dataGridView3.Rows[index].Cells[3].Value.ToString();
                    var idcurator = dataGridView3.Rows[index].Cells[4].Value.ToString();

                    var changeQuery = $"EXEC dbo.UpdateDiploma @iddiploma = {iddiploma}, @theme = '{theme}', @mark = '{mark}', @idstud = '{idstud}', @idcurator = '{idcurator}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }
        // Изменение таблицы Diploma
        private void Change_diploma()
        {
            var selectedRowIndex = dataGridView3.CurrentCell.RowIndex;

            var iddiploma = textBox_iddiploma.Text;
            var theme = textBox_theme.Text;
            int mark;
            int idstud;
            int idcurator;

            if (dataGridView3.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(textBox_mark.Text, out mark))
                {
                    if (int.TryParse(textBox_idstud_diploma.Text, out idstud))
                    {
                        if (int.TryParse(textBox_idcurator_diploma.Text, out idcurator))
                        {
                            dataGridView3.Rows[selectedRowIndex].SetValues(iddiploma, theme, mark, idstud, idcurator);
                            dataGridView3.Rows[selectedRowIndex].Cells[5].Value = RowState.Modified;
                        }
                        else
                            MessageBox.Show("В IDCurator должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        MessageBox.Show("В IDStud должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("В Mark должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
        // Обработчик кнопки обновления Diploma
        private void button_Refresh_diploma_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_diploma(dataGridView3);
        }
        // Обработчик кнопки добавления записи Diploma
        private void button_new_diploma_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form();
            addfrm.Show();
        }
        // Обработчик поиска Diploma
        private void textBox_search_diploma_TextChanged(object sender, EventArgs e)
        {
            Search_diploma(dataGridView3);
        }
        // Обработчик удаления Diploma
        private void button_delete_diploma_Click(object sender, EventArgs e)
        {
            deleteRow_diploma();
        }
        // Обработчик обновления Diploma
        private void button_save_diploma_Click(object sender, EventArgs e)
        {
            Update_diploma();
        }
        // Обработчик изменения Diploma
        private void button_change_diploma_Click(object sender, EventArgs e)
        {
            Change_diploma();
        }


        //
        // Таблица Groups
        //

        
        // Заполнение таблицы Groups в приложении
        private void ReadSingleRow_groups(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), record.GetString(3), RowState.ModifiedNew);
        }
        // Обновление таблицы Groups
        private void RefreshDataGrid_groups(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string querystring = $"SELECT * FROM dbo.GetGroups()";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_groups(dgw, reader);
            }
            reader.Close();

            dataBase.closeConnection();
        }
        // Отображение информации о Groups при клике
        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow_groups = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView4.Rows[selectedRow_groups];

                textBox_idgroup.Text = row.Cells[0].Value.ToString();
                textBox_groupname.Text = row.Cells[1].Value.ToString();
                textBox_yearofadmission.Text = row.Cells[2].Value.ToString();
                textBox_iddepart_groups.Text = row.Cells[3].Value.ToString();
                comboBox_iddepart_groups.Text = row.Cells[3].Value.ToString();
            }
        }

        // Поиск в таблице Groups
        private void Search_groups(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"SELECT * FROM dbo.SearchGroups(@searchText = '%" + textBox_search_groups.Text + "%')";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow_groups(dgw, read);
            }

            read.Close();
        }

        // Удаление строки в Groups
        private void deleteRow_groups()
        {
            int index = dataGridView4.CurrentCell.RowIndex;

            dataGridView4.Rows[index].Visible = false;

            if (dataGridView4.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView4.Rows[index].Cells[4].Value = RowState.Deleted;
                return;
            }
            dataGridView4.Rows[index].Cells[4].Value = RowState.Deleted;
        }
        // Обновление в Groups
        private void Update_groups()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView4.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView4.Rows[index].Cells[4].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var idgroup = Convert.ToInt32(dataGridView4.Rows[index].Cells[0].Value);
                    var deleteQuery = $"EXEC dbo.DeleteGroup @idgroup = {idgroup}";

                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var idgroup = dataGridView4.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView4.Rows[index].Cells[1].Value.ToString();
                    var yearofadmission = dataGridView4.Rows[index].Cells[2].Value.ToString();
                    var iddepart = dataGridView4.Rows[index].Cells[3].Value.ToString();

                    var changeQuery = $"EXEC dbo.UpdateGroup @idgroup = {idgroup}, @name = '{name}', @yearofadmission = '{yearofadmission}', @iddepart = '{iddepart}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }
        // Изменение таблицы Groups
        private void Change_groups()
        {
            var selectedRowIndex = dataGridView4.CurrentCell.RowIndex;

            var idgroup = textBox_idgroup.Text;
            var name = textBox_groupname.Text;
            int yearofadmission;
            int iddepart;

            if (dataGridView4.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(textBox_yearofadmission.Text, out yearofadmission))
                {
                    if (int.TryParse(textBox_iddepart_groups.Text, out iddepart))
                    {
                        dataGridView4.Rows[selectedRowIndex].SetValues(idgroup, name, yearofadmission, iddepart);
                        dataGridView4.Rows[selectedRowIndex].Cells[4].Value = RowState.Modified;
                    }
                    else
                        MessageBox.Show("В IDDepart должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("В YearOfAdmission должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        // Обработчик кнопки обновления Groups
        private void button_Refresh_groups_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_groups(dataGridView4);
        }
        // Обработчик кнопки добавления записи Groups
        private void button_new_groups_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form();
            addfrm.Show();
        }
        // Обработчик поиска Groups
        private void textBox_search_groups_TextChanged(object sender, EventArgs e)
        {
            Search_groups(dataGridView4);
        }
        // Обработчик удаления Groups
        private void button_delete_groups_Click(object sender, EventArgs e)
        {
            deleteRow_groups();
        }
        // Обработчик обновления Groups
        private void button_save_groups_Click(object sender, EventArgs e)
        {
            Update_groups();
        }
        // Обработчик изменения Groups
        private void button_change_groups_Click(object sender, EventArgs e)
        {
            Change_groups();
        }

        //
        // Таблица Jobtitle
        //

        // Заполнение таблицы Jobtitle в приложении
        private void ReadSingleRow_jobtitle(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), RowState.ModifiedNew);
        }
        // Обновление таблицы Jobtitle
        private void RefreshDataGrid_jobtitle(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string querystring = $"SELECT * FROM dbo.GetJobtitles()";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_jobtitle(dgw, reader);
            }
            reader.Close();

            dataBase.closeConnection();
        }
        // Отображение информации о Jobtitle при клике
        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow_jobtitle = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView5.Rows[selectedRow_jobtitle];

                textBox_idjobtitle.Text = row.Cells[0].Value.ToString();
                textBox_jobtitlename.Text = row.Cells[1].Value.ToString();
            }
        }

        // Поиск в таблице Jobtitle
        private void Search_jobtitle(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"SELECT * FROM dbo.SearchJobtitles(@searchText = '%" + textBox_search_jobtitle.Text + "%')";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow_jobtitle(dgw, read);
            }

            read.Close();
        }

        // Удаление строки в Jobtitle
        private void deleteRow_jobtitle()
        {
            int index = dataGridView5.CurrentCell.RowIndex;

            dataGridView5.Rows[index].Visible = false;

            if (dataGridView5.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView5.Rows[index].Cells[2].Value = RowState.Deleted;
                return;
            }
            dataGridView5.Rows[index].Cells[2].Value = RowState.Deleted;
        }
        // Обновление в Jobtitle
        private void Update_jobtitle()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView5.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView5.Rows[index].Cells[2].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var idjobtitle = Convert.ToInt32(dataGridView5.Rows[index].Cells[0].Value);
                    var deleteQuery = $"EXEC dbo.DeleteJobtitle @idjobtitle = {idjobtitle}";

                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var idjobtitle = dataGridView5.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView5.Rows[index].Cells[1].Value.ToString();

                    var changeQuery = $"EXEC dbo.UpdateJobtitle @idjobtitle = {idjobtitle}, @name = '{name}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }
        // Изменение таблицы Jobtitle
        private void Change_jobtitle()
        {
            var selectedRowIndex = dataGridView5.CurrentCell.RowIndex;

            var idjobtitle = textBox_idjobtitle.Text;
            var name = textBox_jobtitlename.Text;

            // Проверка поля FIO на соответствие допустимому формату
            if (!Regex.IsMatch(name, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле Name можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            if (dataGridView5.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {

                dataGridView5.Rows[selectedRowIndex].SetValues(idjobtitle, name);
                dataGridView5.Rows[selectedRowIndex].Cells[2].Value = RowState.Modified;

            }
        }
        // Обработчик кнопки обновления Jobtitle
        private void button_Refresh_jobtitle_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_jobtitle(dataGridView5);
        }
        // Обработчик кнопки добавления записи Jobtitle
        private void button_new_jobtitle_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form();
            addfrm.Show();
        }
        // Обработчик поиска Jobtitle
        private void textBox_search_jobtitle_TextChanged(object sender, EventArgs e)
        {
            Search_jobtitle(dataGridView5);
        }
        // Обработчик удаления Jobtitle
        private void button_delete_jobtitle_Click(object sender, EventArgs e)
        {
            deleteRow_jobtitle();
        }
        // Обработчик обновления Jobtitle
        private void button_save_jobtitle_Click(object sender, EventArgs e)
        {
            Update_jobtitle();
        }
        // Обработчик изменения Jobtitle
        private void button_change_jobtitle_Click(object sender, EventArgs e)
        {
            Change_jobtitle();
        }


        //
        // Таблица Student
        //

        // Заполнение таблицы Student в приложении
        private void ReadSingleRow_student(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetInt32(4), record.GetString(5), record.GetDateTime(6).ToString("yyyy-MM-dd"), RowState.ModifiedNew);
        }
        // Обновление таблицы Student
        private void RefreshDataGrid_student(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string querystring = $"SELECT * FROM dbo.GetStudents()";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_student(dgw, reader);
            }
            reader.Close();

            dataBase.closeConnection();
        }
        // Отображение информации о Student при клике
        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow_student = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView6.Rows[selectedRow_student];

                textBox_idstud.Text = row.Cells[0].Value.ToString();
                textBox_surname.Text = row.Cells[1].Value.ToString();
                textBox_studname.Text = row.Cells[2].Value.ToString();
                textBox_patronymic.Text = row.Cells[3].Value.ToString();
                textBox_recordbook.Text = row.Cells[4].Value.ToString();
                textBox_idgroup_student.Text = row.Cells[5].Value.ToString();
                comboBox_idgroup_student.Text = row.Cells[5].Value.ToString();
                textBox_datebirth.Text = row.Cells[6].Value.ToString();
            }
        }

        // Поиск в таблице Student
        private void Search_student(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"SELECT * FROM dbo.SearchStudents(@searchText = '%" + textBox_search_student.Text + "%')";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow_student(dgw, read);
            }

            read.Close();
        }

        // Удаление строки в Student
        private void deleteRow_student()
        {
            int index = dataGridView6.CurrentCell.RowIndex;

            dataGridView6.Rows[index].Visible = false;

            if (dataGridView6.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView6.Rows[index].Cells[7].Value = RowState.Deleted;
                return;
            }
            dataGridView6.Rows[index].Cells[7].Value = RowState.Deleted;
        }
        // Обновление в Student
        private void Update_student()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView6.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView6.Rows[index].Cells[7].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var idstud = Convert.ToInt32(dataGridView6.Rows[index].Cells[0].Value);
                    var deleteQuery = $"EXEC dbo.DeleteStudent @idstud = {idstud}";

                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var idstud = dataGridView6.Rows[index].Cells[0].Value.ToString();
                    var surname = dataGridView6.Rows[index].Cells[1].Value.ToString();
                    var name = dataGridView6.Rows[index].Cells[2].Value.ToString();
                    var patronymic = dataGridView6.Rows[index].Cells[3].Value.ToString();
                    var recordbook = dataGridView6.Rows[index].Cells[4].Value.ToString();
                    var idgroup = dataGridView6.Rows[index].Cells[5].Value.ToString();
                    var datebirth = dataGridView6.Rows[index].Cells[6].Value.ToString();

                    var changeQuery = $"EXEC dbo.UpdateStudent @idstud = {idstud}, @surname = '{surname}', @name = '{name}', @patronymic = '{patronymic}', @recordbook = '{recordbook}', @idgroup = '{idgroup}', @datebirth = '{datebirth}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }
        // Изменение таблицы Student
        private void Change_student()
        {
            var selectedRowIndex = dataGridView6.CurrentCell.RowIndex;

            var idstud = textBox_idstud.Text;
            var surname = textBox_surname.Text;
            var name = textBox_studname.Text;
            var patronymic = textBox_patronymic.Text;
            int recordbook;
            int idgroup;
            DateTime datebirth;

            // Проверка полей Surname, Name и Patronymic на соответствие допустимому формату
            if (!Regex.IsMatch(surname, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле Фамилия можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            if (!Regex.IsMatch(name, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле Имя можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            if (!Regex.IsMatch(patronymic, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле Отчество можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            if (dataGridView6.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(textBox_recordbook.Text, out recordbook))
                {
                    if (int.TryParse(textBox_idgroup_student.Text, out idgroup))
                    {
                        if (DateTime.TryParseExact(textBox_datebirth.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datebirth))
                        {
                            dataGridView6.Rows[selectedRowIndex].SetValues(idstud, surname, name, patronymic, recordbook, idgroup, datebirth.ToString("yyyy-MM-dd"));
                            dataGridView6.Rows[selectedRowIndex].Cells[7].Value = RowState.Modified;
                        }
                        else
                            MessageBox.Show("Поле DateBirth должно быть в формате [yyyy-mm-dd]!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        MessageBox.Show("В IDGroup должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("В RecordBook должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
        // Обработчик кнопки обновления Student
        private void button_Refresh_student_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_student(dataGridView6);
        }
        // Обработчик кнопки добавления записи Student
        private void button_new_student_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form();
            addfrm.Show();
        }
        // Обработчик поиска Student
        private void textBox_search_student_TextChanged(object sender, EventArgs e)
        {
            Search_student(dataGridView6);
        }
        // Обработчик удаления Student
        private void button_delete_student_Click(object sender, EventArgs e)
        {
            deleteRow_student();
        }
        // Обработчик обновления Student
        private void button_save_student_Click(object sender, EventArgs e)
        {
            Update_student();
        }
        // Обработчик изменения Student
        private void button_change_student_Click(object sender, EventArgs e)
        {
            Change_student();
        }


        //
        // Таблица VUZ
        //

        // Заполнение таблицы VUZ в приложении
        private void ReadSingleRow_vuz(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetDateTime(3).ToString("yyyy-MM-dd"), record.GetInt32(4), RowState.ModifiedNew);
        }
        // Обновление таблицы VUZ
        private void RefreshDataGrid_vuz(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string querystring = $"SELECT * FROM dbo.GetVUZs()";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_vuz(dgw, reader);
            }
            reader.Close();

            dataBase.closeConnection();
        }
        // Отображение информации о VUZ при клике
        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow_vuz = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView7.Rows[selectedRow_vuz];

                textBox_idvuz.Text = row.Cells[0].Value.ToString();
                textBox_vuzname.Text = row.Cells[1].Value.ToString();
                textBox_location.Text = row.Cells[2].Value.ToString();

                textBox_dateoffoundation.Text = row.Cells[3].Value.ToString();

                textBox_inn.Text = row.Cells[4].Value.ToString();
            }
        }

        // Поиск в таблице VUZ
        private void Search_vuz(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"SELECT * FROM dbo.SearchVUZs(@searchText = '%" + textBox_search_vuz.Text + "%')";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow_vuz(dgw, read);
            }

            read.Close();
        }

        // Удаление строки в VUZ
        private void deleteRow_vuz()
        {
            int index = dataGridView7.CurrentCell.RowIndex;

            dataGridView7.Rows[index].Visible = false;

            if (dataGridView7.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView7.Rows[index].Cells[5].Value = RowState.Deleted;
                return;
            }
            dataGridView7.Rows[index].Cells[5].Value = RowState.Deleted;
        }
        // Обновление в VUZ
        private void Update_vuz()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView7.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView7.Rows[index].Cells[5].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var idvuz = Convert.ToInt32(dataGridView7.Rows[index].Cells[0].Value);
                    var deleteQuery = $"EXEC dbo.DeleteVUZ @idvuz = {idvuz}";

                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var idvuz = dataGridView7.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView7.Rows[index].Cells[1].Value.ToString();
                    var location = dataGridView7.Rows[index].Cells[2].Value.ToString();
                    var dateoffoundation = dataGridView7.Rows[index].Cells[3].Value.ToString();
                    var inn = dataGridView7.Rows[index].Cells[4].Value.ToString();

                    var changeQuery = $"EXEC dbo.UpdateVUZ @idvuz = {idvuz}, @name = '{name}', @location = '{location}', @dateoffoundation = '{dateoffoundation}', @inn = '{inn}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }
        // Изменение таблицы VUZ
        private void Change_vuz()
        {
            var selectedRowIndex = dataGridView7.CurrentCell.RowIndex;

            var idvuz = textBox_idvuz.Text;
            var name = textBox_vuzname.Text;
            var location = textBox_location.Text;
            DateTime dateoffoundation;
            int inn;

            // Проверка полей Name и Location на соответствие допустимому формату
            if (!Regex.IsMatch(name, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле Название можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            if (!Regex.IsMatch(location, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле Местоположение можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            if (dataGridView7.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(textBox_inn.Text, out inn))
                {
                    if (DateTime.TryParseExact(textBox_dateoffoundation.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateoffoundation))
                    {

                        dataGridView7.Rows[selectedRowIndex].SetValues(idvuz, name, location, dateoffoundation.ToString("yyyy-MM-dd"), inn);
                        dataGridView7.Rows[selectedRowIndex].Cells[5].Value = RowState.Modified;

                    }
                    else
                        MessageBox.Show("Поле DateOfFoundation должно иметь формат [yyyy-MM-dd]!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("В INN должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
        // Обработчик кнопки обновления VUZ
        private void button_Refresh_vuz_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_vuz(dataGridView7);
        }
        // Обработчик кнопки добавления записи VUZ
        private void button_new_vuz_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form();
            addfrm.Show();
        }
        // Обработчик поиска VUZ
        private void textBox_search_vuz_TextChanged(object sender, EventArgs e)
        {
            Search_vuz(dataGridView7);
        }
        // Обработчик удаления VUZ
        private void button_delete_vuz_Click(object sender, EventArgs e)
        {
            deleteRow_vuz();
        }
        // Обработчик обновления VUZ
        private void button_save_vuz_Click(object sender, EventArgs e)
        {
            Update_vuz();
        }
        // Обработчик изменения VUZ
        private void button_change_vuz_Click(object sender, EventArgs e)
        {
            Change_vuz();
        }
    }
}
