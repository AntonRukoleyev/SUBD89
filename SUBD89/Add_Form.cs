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
    public partial class Add_Form : Form
    {
        DataBase dataBase = new DataBase();
        public Add_Form()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            LoadJobtitles();
            LoadCurators();
            LoadDiplomas();
            LoadGroups();
            LoadStudents();
            comboBox_idjobtitle_curator.SelectedIndexChanged += combobox_jobtitle_curator_SelectedIndexChanged;
        }


        private void button_save_curator_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var fio = textBox_addfio.Text;
            int idjobtitle;
            int idvuz;

            // Проверка поля FIO на соответствие допустимому формату
            if (!Regex.IsMatch(fio, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле FIO можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            if (int.TryParse(textBox_addidjobtitle_curator.Text, out idjobtitle))
            {
                if (int.TryParse(textBox_addidvuz_curator.Text, out idvuz))
                {
                    var addQuery = $"EXEC dbo.InsertCurator @fio = '{fio}', @idjobtitle = '{idjobtitle}', @idvuz = '{idvuz}'";

                    var command = new SqlCommand(addQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();

                    MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("В IDVUZ должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("В IDJobtitle должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dataBase.closeConnection();
        }

        private void button_save_department_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();


            var name = textBox_adddepartname.Text;

            // Проверка поля FIO на соответствие допустимому формату
            if (!Regex.IsMatch(name, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле Name можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            var addQuery = $"EXEC dbo.InsertDepartment @name = '{name}'";

            var command = new SqlCommand(addQuery, dataBase.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataBase.closeConnection();

        }

        private void button_save_diploma_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var theme = textBox_addtheme.Text;
            int mark;
            int idstud;
            int idcurator;


            if (int.TryParse(textBox_addmark.Text, out mark))
            {
                if (int.TryParse(textBox_addidstud_diploma.Text, out idstud))
                {
                    if (int.TryParse(textBox_addidcurator_diploma.Text, out idcurator))
                    {
                        var addQuery = $"EXEC dbo.InsertDiploma @theme = '{theme}', @mark = '{mark}', @idstud = '{idstud}', @idcurator = '{idcurator}'";

                        var command = new SqlCommand(addQuery, dataBase.getConnection());
                        command.ExecuteNonQuery();

                        MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            dataBase.closeConnection();
        }

        private void button_save_groups_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var name = textBox_addgroupname.Text;
            int yearofadmission;
            int iddepart;

            if (int.TryParse(textBox_addyearofadmission.Text, out yearofadmission))
            {
                if (int.TryParse(textBox_addiddepart_groups.Text, out iddepart))
                {
                    var addQuery = $"EXEC dbo.InsertGroup @name = '{name}', @yearofadmission = '{yearofadmission}', @iddepart = '{iddepart}'";

                    var command = new SqlCommand(addQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();

                    MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("В IDDepart должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("В YearOfAdmission должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dataBase.closeConnection();
        }

        private void button_save_jobtitle_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var name = textBox_addjobtitlename.Text;

            // Проверка поля FIO на соответствие допустимому формату
            if (!Regex.IsMatch(name, @"^[а-яА-ЯёЁ\s.]+$"))
            {
                MessageBox.Show("В поле Name можно вводить только русские буквы, пробелы и точки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();
                return;
            }

            var addQuery = $"EXEC dbo.InsertJobtitle @name = '{name}'";

            var command = new SqlCommand(addQuery, dataBase.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataBase.closeConnection();
        }

        private void button_save_student_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var surname = textBox_addsurname.Text;
            var name = textBox_addstudname.Text;
            var patronymic = textBox_addpatronymic.Text;
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

            if (int.TryParse(textBox_addrecordbook.Text, out recordbook))
            {
                if (int.TryParse(textBox_addidgroup_student.Text, out idgroup))
                {
                    if (DateTime.TryParseExact(textBox_adddatebirth.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datebirth))
                    {
                        var addQuery = $"EXEC dbo.InsertStudent @surname = '{surname}', @name = '{name}', @patronymic = '{patronymic}', @recordbook = '{recordbook}', @idgroup = '{idgroup}', @datebirth = '{datebirth}'";

                        var command = new SqlCommand(addQuery, dataBase.getConnection());
                        command.ExecuteNonQuery();

                        MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


            dataBase.closeConnection();
        }

        private void button_save_vuz_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var name = textBox_addvuzname.Text;
            var location = textBox_addlocation.Text;
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

            if (int.TryParse(textBox_addinn.Text, out inn))
            {
                if (DateTime.TryParseExact(textBox_adddateoffoundation.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateoffoundation))
                {
                    var addQuery = $"EXEC dbo.InsertVUZ @name = '{name}', @location = '{location}', @dateoffoundation = '{dateoffoundation}', @inn = '{inn}'";

                    var command = new SqlCommand(addQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();

                    MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Поле DateOfFoundation должно иметь формат [yyyy-MM-dd]!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("В INN должны быть только целые числа!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dataBase.closeConnection();
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
                textBox_addidjobtitle_curator.Text = selectedItem.Value;
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
                UpdateTextboxWithComboboxValue(comboBox_idvuz_curator, textBox_addidvuz_curator);
            };
        }

        private void LoadDiplomas()
        {
            LoadComboboxWithQuery(comboBox_idstud_diploma, "SELECT IDStud, Surname + ' ' + Name + ' ' + Patronymic AS FullName FROM Student", "FullName", "IDStud");
            comboBox_idstud_diploma.SelectedIndexChanged += (sender, e) =>
            {
                UpdateTextboxWithComboboxValue(comboBox_idstud_diploma, textBox_addidstud_diploma);
            };

            LoadComboboxWithQuery(comboBox_idcurator_diploma, "SELECT IDCurator, FIO FROM Curator", "FIO", "IDCurator");
            comboBox_idcurator_diploma.SelectedIndexChanged += (sender, e) =>
            {
                UpdateTextboxWithComboboxValue(comboBox_idcurator_diploma, textBox_addidcurator_diploma);
            };
        }

        private void LoadGroups()
        {
            LoadComboboxWithQuery(comboBox_iddepart_groups, "SELECT IDDepart, Name FROM Department", "Name", "IDDepart");
            comboBox_iddepart_groups.SelectedIndexChanged += (sender, e) =>
            {
                UpdateTextboxWithComboboxValue(comboBox_iddepart_groups, textBox_addiddepart_groups);
            };
        }

        private void LoadStudents()
        {
            LoadComboboxWithQuery(comboBox_idgroup_student, "SELECT IDGroup, Name FROM Groups", "Name", "IDGroup");
            comboBox_idgroup_student.SelectedIndexChanged += (sender, e) =>
            {
                UpdateTextboxWithComboboxValue(comboBox_idgroup_student, textBox_addidgroup_student);
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

    }
}
