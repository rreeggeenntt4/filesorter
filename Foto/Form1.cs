using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Foto
{
    public partial class Form1 : Form
    {
        //private IEnumerable<string> filelist;

        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                this.textBox1.Text = "";
                this.label2.Text = "Выберите файл list.txt";

            }
            if (comboBox1.SelectedIndex == 1)
            {
                this.textBox1.Text = "";
                this.label2.Text = "Выберите директорию";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            { this.label3.Text = "Где искать, вкл. подпапки"; }
            else
            { this.label3.Text = "Где искать"; }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked)
            { this.button4.Text = "Копировать";this.label4.Text = "Куда копировать"; }
            else
            { this.button4.Text = "Перенести"; this.label4.Text = "Куда перенести"; }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (((comboBox1.SelectedIndex == 0) || (comboBox1.SelectedIndex == 1)) && (textBox1.Text != "") && (textBox2.Text != "") && (textBox3.Text != ""))
            {
                if (textBox2.Text == textBox3.Text) { MessageBox.Show("Каталог в котором осуществляется поиск не может одновременно быть еще и каталогом назначения. Поменяйте условия!"); }
                else if (textBox1.Text == textBox2.Text) { MessageBox.Show("Каталог исходник не должен быть каталогом в котором осуществляется поиск. Поменяйте условия!"); }
                else if (textBox1.Text == textBox3.Text) { MessageBox.Show("Каталог исходник не должен быть каталогом назначения. Поменяйте условия!"); }
                else
                {
                    AppDomain dom = AppDomain.CurrentDomain; // Расплоложение домашней категории
                    string t1 = textBox1.Text;     // Передаем в строковую переменную содержание поля textBox1 для ввода файла list.txt
                    string[] flist = { "" };
                    //СТАРТ: первого варианта обработки по списку из файла  
                    
                        if (comboBox1.SelectedIndex == 0) { flist = File.ReadAllLines(t1); };
                        if (comboBox1.SelectedIndex == 1) { flist = Directory.GetFiles(t1); };
                                                                            //Список наименований файлов                                           FLIST       
                        string infofile = dom.BaseDirectory + "/info.txt"; // Информационный файл о имеющихся каталогах и файлах                  INFO.TXT
                        System.IO.File.Delete(@infofile); // Удаляем предыдущий инф. файл, чтобы потом создать новый
                        string logfile = dom.BaseDirectory + "/log.txt";        // Файл логов.                                                    LOG.TXT
                        System.IO.File.AppendAllText(logfile, "\r\nЗапуск " + DateTime.Now + "\r\n");
                        string dirName2 = this.textBox2.Text;  // Директория в которой осуществляется поиск                                       dirName2                      
                        string dirName3 = this.textBox3.Text;  // Директория назначения                                                           dirName3                  
                        if (checkBox2.Checked) { System.IO.File.AppendAllText(logfile, "Копирование из " + dirName2 + " в " + dirName3 + "\r\n"); }
                        else { System.IO.File.AppendAllText(logfile, "Перенос из " + dirName2 + " в " + dirName3 + "\r\n"); }
                                          
                        if (Directory.Exists(dirName2))
                        {
                            //СТАРТ: только для вывода Файлов дериктории ГДЕ ИСКАТЬ
                            string[] files = { "" };
                            System.IO.File.AppendAllText(infofile, "Файлы дериктории ГДЕ ИСКАТЬ до обработки:\r\n");
                            if (this.checkBox1.Checked) { files = Directory.GetFiles(this.textBox2.Text, "*.*", SearchOption.AllDirectories); }
                            else { files = Directory.GetFiles(dirName2); } // Хранит файлы которые есть в директории в которой осуществляется поиск dirName2=>   FILES
                            foreach (string sf in files)
                            {
                                string filename = sf;
                                string delstr = this.textBox2.Text + "\\"; // Для удаления полного пути из файл пути, чтобы получить только наименование файла
                                filename = filename.Replace(delstr, "");
                                FileInfo nameFile = new FileInfo(sf);
                                string fName = nameFile.Name;
                                System.IO.File.AppendAllText(infofile, sf + "\r\n");
                            }
                            //ФИНИШ: только для вывода Файлов дериктории ГДЕ ИСКАТЬ

                            //СТАРТ: основной обработки
                            foreach (string sl in flist)
                            {
                                int count = 0;
                                string nFile = System.IO.Path.GetFileNameWithoutExtension(sl); //Итерационное имя           nFile
                                foreach (string sd in files)
                                {                                    
                                    string nDir = System.IO.Path.GetFileNameWithoutExtension(sd); //Итерационное имя           nDir
                                    if (nFile == nDir)
                                        {
                                            FileInfo curentFile = new FileInfo(sd);
                                            string fName = curentFile.Name;
                                            if (this.checkBox2.Checked)
                                            {
                                                try { File.Copy(sd, dirName3 + "\\" + fName); }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show("Ошибка! " + ex.Message + "\r\n Файл " + sd + " пропущен \r\n соответствующую запись вы найдете в log.txt");
                                                    System.IO.File.AppendAllText(logfile, "Ошибка! " + ex.Message + " Файл " + sd + " пропущен \r\n");
                                                }
                                            }
                                            else
                                            {
                                                try { File.Move(sd, dirName3 + "\\" + fName); }
                                                catch (Exception ex2)
                                                {
                                                    MessageBox.Show("Ошибка! " + ex2.Message + "\r\n Файл " + sd + " пропущен \r\n соответствующую запись вы найдете в log.txt");
                                                    System.IO.File.AppendAllText(logfile, "Ошибка! " + ex2.Message + " Файл " + sd + " пропущен \r\n");
                                                }
                                            }
                                            count = count + 1;
                                        }                                   
                                }
                                if (count < 1) { System.IO.File.AppendAllText(logfile, "Совпадений по " + nFile + " не найдено.\r\n"); }
                            }
                            //ФИНИШ: основной обработки

                            System.IO.File.AppendAllText(infofile, "\r\nИсходник для поиска (LIST.TXT):\r\n");
                            foreach (string ns in flist)
                            {
                                System.IO.File.AppendAllText(infofile, ns + "\r\n");
                            }
                            MessageBox.Show("Исполнено.");
                        }
                    
                    //ФИНИШ:  первого варианта обработки по списку из файла

                    
                }

            }
            else
            {
                MessageBox.Show("Не все поля заполнены!");
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == 0)
            {
                OpenFileDialog FD = new OpenFileDialog();
                if (FD.ShowDialog() == DialogResult.OK)
                { this.textBox1.Text = FD.FileName; }

            }
            else if (comboBox1.SelectedIndex == 1)
            {
                FolderBrowserDialog FBD = new FolderBrowserDialog();
                if (FBD.ShowDialog() == DialogResult.OK)
                {
                    this.textBox1.Text = FBD.SelectedPath;
                }
            }
            else
            { MessageBox.Show("Вы не выбрали вариант поиска!"); }
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = FBD.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                this.textBox3.Text = FBD.SelectedPath;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная программа выполняет перенос либо копирование файлов в зависимости от условий. \r\n\r\nФайлы выбираются либо по списку который содержится в файле list.txt, либо по директории, файлы которой и будут служить списком для дальнейших действий программы. Если в данной директории есть поддиректории то они не учитываются.\r\n\r\nПрограмма выполняет поиск по наименованию файлов. Соответственно надо понимать что все одинаковые наименования с любыми разрешениями будут перенесены либо скопированы.\r\n\r\nНе найденные файлы из папки источника или из списка list.txt можно найти в файле log.txt. \r\n\r\nФайл info.txt содержит информацию о последнем запуске программы. При каждом запуске перезаписывается. Для функциональности программы отсутствие всех этих файлов на работоспособности никак не сказывается - файлы создаются снова.\r\n\r\nАвтор: Иван Медведев, скайп live:ivan_medvedev87");
        }
      
    }
}
