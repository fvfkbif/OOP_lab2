using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace ЛР2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            comboBox1.SelectedItem = "1";
            comboBox2.SelectedItem = "1";
            label5.Text = "Возраст: " + trackBar1.Value;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label5.Text = "Возраст: " + trackBar1.Value;
        }

        private void textBox_KeyPress_Letter(object sender, KeyPressEventArgs e)
        {
            char letter = e.KeyChar;
            if (!Char.IsLetter(letter) && letter != 8 && letter != 45 && letter != 32) // буквы и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void textBox_KeyPress_Number(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsNumber(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void AvarageMark_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsNumber(number) && number != 8 && number != 44 && number != 46) // цифры, клавиша BackSpace и точка
            {
                e.Handled = true;
            }
        }

        private void textBox_Adress_KeyPress(object sender, KeyPressEventArgs e)
        {
            char symbol = e.KeyChar;
            if (!Char.IsNumber(symbol) && !Char.IsLetter(symbol) && symbol != 8 && symbol != 32 && symbol != 45 && symbol != 47) // буквы, цифры, клавиша BackSpace, дефис, пробел и слеш
            {
                e.Handled = true;
            }
        }

        private void textBox_KeyPress_Numbers_And_Letters(object sender, KeyPressEventArgs e)
        {
            char symbol = e.KeyChar;
            if (!Char.IsNumber(symbol) && !Char.IsLetter(symbol) && symbol != 8 && symbol != 32) // буквы, цифры, клавиша BackSpace и пробел
            {
                e.Handled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                groupBox2.Enabled = true;
            }
            else
            {
                groupBox2.Enabled = false;
            }
        }

        private void Save(object sender, EventArgs e)
        {
            textBox12.Text = "";
            if (IsFilled() && CorrectMark() && CorrectIndex())
            {
                string surname = textBox1.Text;
                string name = textBox2.Text;
                string patronymic = textBox3.Text;
                string sex = "";
                foreach (RadioButton r in panel2.Controls)
                {
                    if (r.Checked)
                        sex = r.Text;
                }
                int age = trackBar1.Value;
                string course = "";
                foreach (RadioButton r in panel1.Controls)
                {
                    if (r.Checked)
                        course = r.Text;
                }
                int group = Convert.ToInt32(comboBox1.SelectedItem);
                double mark = double.Parse(textBox4.Text.Replace('.', ','));
                string dateOfBirth = monthCalendar1.SelectionRange.Start.ToShortDateString();

                string town = textBox5.Text;
                string street = textBox7.Text;
                string house = textBox8.Text;
                int index = Convert.ToInt32(textBox6.Text);
                int flat = Convert.ToInt32(numericUpDown2.Value);
                
                
                Adress adress = new Adress(town, street, house, index, flat);
                Student student;

                if (checkBox1.Checked)
                {
                    string company = textBox9.Text;
                    string position = textBox10.Text;
                    string experience = textBox11.Text;
                    string rate = comboBox2.SelectedItem.ToString();

                    PlaceOfWork placeOfWork = new PlaceOfWork(company, position, experience, rate);

                    student = new Student(surname, name, patronymic, sex, age, course, group, mark, dateOfBirth, adress, placeOfWork);
                }
                else
                {
                    student = new Student(surname, name, patronymic, sex, age, course, group, mark, dateOfBirth, adress);
                }

                XmlSerializeWrapper.Serialize(student, "student.xml");

                textBox12.Text = "Данные сохранены!";
            }          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var deserializeStudent = XmlSerializeWrapper.Deserialize<Student>("student.xml");
            textBox12.Text = deserializeStudent.PrintString();
        }

        bool IsFilled()
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "")
            {
                textBox12.Text += "Не все поля заполнены!" + Environment.NewLine;
                return false;
            }
            else if(checkBox1.Checked && (textBox9.Text == "" || textBox10.Text == "" || textBox11.Text == ""))
            {
                textBox12.Text += "Не все поля заполнены!" + Environment.NewLine;
                return false;
            }
            else return true;
        }

        bool CorrectMark()
        {
            double mark = double.Parse(textBox4.Text.Replace('.', ','));
            if(mark >= 0 && mark <= 10)
            {
                return true;
            }
            else
            {
                textBox12.Text += "Неверное значение среднего балла!" + Environment.NewLine;
                return false;
            }
        }

        bool CorrectIndex()
        {
            if (textBox6.Text.Length < 6)
            {
                textBox12.Text += "Неверное значение индекса города!" + Environment.NewLine;
                return false;
            }
            else return true;
        }
    }


    [Serializable]
    public class Student
    {
        public string surname, name, patronymic, sex, course;
        public int age, group;
        public double mark;
        public string dateOfBirth;
        public Adress adress;
        public PlaceOfWork placeOfWork;

        public Student(string surname, string name, string patronymic, string sex, int age, string course, int group, double mark, string dateOfBirth, Adress adress)
        {
            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.sex = sex;
            this.age = age;
            this.course = course;
            this.group = group;
            this.mark = mark;
            this.dateOfBirth = dateOfBirth;
            this.adress = adress;
        }

        public Student(string surname, string name, string patronymic, string sex, int age, string course, int group, double mark, string dateOfBirth, Adress adress, PlaceOfWork placeOfWork)
        {
            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.sex = sex;
            this.age = age;
            this.course = course;
            this.group = group;
            this.mark = mark;
            this.dateOfBirth = dateOfBirth;
            this.adress = adress;
            this.placeOfWork = placeOfWork;
        }

        public Student() { }

        public string PrintString()
        {
            if (placeOfWork != null)
            {
                return $"Фамилия: {surname}, имя: {name}, отчество: {patronymic}, пол: {sex}, возраст: {age}, дата рождения: {dateOfBirth}, курс: {course}, группа: {group}, средний балл: {mark}, город: {adress.town}, индекс: {adress.index}, улица: {adress.street}, дом: {adress.house}, квартира: {adress.flat}, компания: {placeOfWork.company}, должность: {placeOfWork.position}, стаж: {placeOfWork.experience}, ставка: {placeOfWork.rate}";
            }
            else return $"Фамилия: {surname}, имя: {name}, отчество: {patronymic}, пол: {sex}, возраст: {age}, дата рождения: {dateOfBirth}, курс: {course}, группа: {group}, средний балл: {mark}, город: {adress.town}, индекс: {adress.index}, улица: {adress.street}, дом: {adress.house}, квартира: {adress.flat}";
        }
    }


    [Serializable]
    public class Adress
    {
        public string town, street, house;
        public int index, flat;

        public Adress(string town, string street, string house, int index, int flat)
        {
            this.town = town;
            this.street = street;
            this.house = house;
            this.index = index;
            this.flat = flat;
        }

        public Adress() { }
    }


    [Serializable]
    public class PlaceOfWork
    {
        public string company, position, experience, rate;

        public PlaceOfWork(string company, string position, string experience, string rate)
        {
            this.company = company;
            this.position = position;
            this.experience = experience;
            this.rate = rate;
        }

        public PlaceOfWork() { }
    }


    public static class XmlSerializeWrapper
    {
        public static void Serialize<T>(T obj, string filename)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));

            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, obj);
            }
        }

        public static T Deserialize<T>(string filename)
        {
            T obj;
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                obj = (T)formatter.Deserialize(fs);
            }

            return obj;
        }
    }

}
