﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Data;
using LiveCharts.Wpf.Points;
using Microsoft.Office.Interop;
using System.Runtime.InteropServices;
using MahApps.Metro.Controls;

namespace PassDash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public List<Password> passWords;
        public string masterPassword;
        public SeriesCollection SeriesCollectionPass { get; set; }
        public SeriesCollection SeriesCollectionCat { get; set; }

        public string openedPasswordFile = "";

        private Password selPassword = null;

        public Stack<int> saveHistory = new Stack<int>();
        private Dictionary<string, Brush> colorsPasswordStrengthChart = new Dictionary<string, Brush>();

        PasswordAdvisor passWordAdvisor = new PasswordAdvisor();
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            passWords = new List<Password>();

            this.bDelPassword.Visibility = Visibility.Hidden;
            this.bOpenWebsite.Visibility = Visibility.Hidden;
            this.bShowAllPasswords.Visibility = Visibility.Hidden;

            tabControlMain.SelectedIndex = 0;
            tabItemPasswords.IsEnabled = false;
            tabItemPasswords.Visibility = Visibility.Hidden;
            menuItemNoFile();

            //testData();
            resetPassWordForm();
            showPassWords();
            showPassWordPieChart();
        }

        #region menu events
        private void save_Click(object sender, RoutedEventArgs e)
        {
            savePassword();
        }

        private void saveAs_Click(object sender, RoutedEventArgs e)
        {
            saveAsPassword(false);
        }

     
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void closeFile_Click(object sender, RoutedEventArgs e)
        {
            ucCategory.Items.Clear();
            passWords.Clear();
            this.bShowAllPasswords.Visibility = Visibility.Hidden;
            this.uMasterPassword.Password = "";
            this.uTxtMasterPassword.Text = "";
            this.tFreeSearch.Text = "";
            this.lerrSearch.Text = "";

            openedPasswordFile = "";
            masterPassword = "";
            resetPassWordForm();
            showPassWords();
            showPassWordPieChart();
            showCatPieChart();
            menuItemNoFile();
            tabControlMain.SelectedIndex = 0;
            tabItemPasswords.IsEnabled = false;
            tabItemPasswords.Visibility = Visibility.Hidden;

        }


        private void import_Excell_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Range range;

            string str;
            int rCnt;
            int cCnt;
            int rw = 0;
            int cl = 0;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            string fileName = "";

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    fileName = openFileDialog.FileName.ToString();

                    xlApp = new Microsoft.Office.Interop.Excel.Application();
                    xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    range = xlWorkSheet.UsedRange;
                    rw = range.Rows.Count;
                    cl = range.Columns.Count;

                    int rowCount = 0;
                    Dictionary<int, string> passWordColumns = new Dictionary<int, string>();

                    for (rCnt = 1; rCnt <= rw; rCnt++)
                    {

                        Password newPassword = new Password();
                        Boolean passwordValid = false;

                        for (cCnt = 1; cCnt <= cl; cCnt++)
                        {
                            str = Convert.ToString((range.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2);
                            if (rowCount == 0)
                            {
                                if (str != null)
                                {
                                    if (str.ToLower() == "name")
                                    {
                                        passWordColumns.Add(cCnt, "name");
                                    }
                                    else if (str.ToLower() == "username")
                                    {
                                        passWordColumns.Add(cCnt, "username");
                                    }
                                    else if (str.ToLower() == "password")
                                    {
                                        passWordColumns.Add(cCnt, "password");
                                    }
                                    else if (str.ToLower() == "note")
                                    {
                                        passWordColumns.Add(cCnt, "note");
                                    }
                                    else if (str.ToLower() == "category")
                                    {
                                        passWordColumns.Add(cCnt, "category");
                                    }
                                }
                            }
                            else
                            {
                                if (passWordColumns.ContainsKey(cCnt))
                                {
                                    string column = passWordColumns[cCnt];
                                    if (column == "name")
                                    {
                                        if (str == null)
                                        {
                                            newPassword.name = "";
                                        }
                                        else
                                        {
                                            newPassword.name = str;
                                        }
                                    }
                                    else if (column == "username")
                                    {
                                        if (str == null)
                                        {
                                            newPassword.userName = "";
                                        }
                                        else
                                        {
                                            newPassword.userName = str;
                                        }
                                    }
                                    else if (column == "password")
                                    {
                                        if (str == null)
                                        {
                                            newPassword.userPassword = "";
                                        }
                                        else
                                        {
                                            newPassword.userPassword = str;
                                        }
                                    }
                                    else if (column == "note")
                                    {
                                        if (str == null)
                                        {
                                            newPassword.note = "";
                                        }
                                        else
                                        {
                                            newPassword.note = str;
                                        }
                                    }
                                    else if (column == "category")
                                    {
                                        if (str == null)
                                        {
                                            newPassword.category = "";
                                        }
                                        else
                                        {
                                            newPassword.category = str;
                                        }
                                    }
                                }

                                if ((newPassword.name != "" && newPassword.name != null) || (newPassword.userName != "" && newPassword.userName != null))
                                {
                                    passwordValid = true;
                                }
                            }
                        }

                        if (passwordValid == true)
                        {

                            if (newPassword.name == null)
                            {
                                newPassword.name = "";
                            }
                            if (newPassword.userPassword == null)
                            {
                                newPassword.userPassword = "";
                            }
                            if (newPassword.userName == null)
                            {
                                newPassword.userName = "";
                            }
                            if (newPassword.note == null)
                            {
                                newPassword.note = "";
                            }
                            if (newPassword.category == null)
                            {
                                newPassword.category = "";
                            }


                            newPassword.id = Guid.NewGuid().ToString();
                            newPassword.dateTime = DateTime.Now.ToShortDateString();
                            //newPassword.category = "";
                            newPassword.nr = "";
                            newPassword.website = "";
                            passWords.Add(newPassword);
                        }
                        rowCount = rowCount + 1;
                    }

                    xlWorkBook.Close(true, null, null);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);

                    showPassWords();
                    showPassWordPieChart();
                    showCatPieChart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Importing from Excell failed: " + ex.ToString());
            }
        }


        private void export_Excell_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xlsx)|*.xlsx";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "Export Excel File To";

            try
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    app.Visible = true;
                    Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
                    Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];
                    int i = 1;
                    int i2 = 1;

                    ws.Cells[i2, 1] = "nr";
                    ws.Cells[i2, 2] = "name";
                    ws.Cells[i2, 3] = "category";
                    ws.Cells[i2, 4] = "username";
                    ws.Cells[i2, 5] = "password";
                    ws.Cells[i2, 6] = "note";
                    ws.Cells[i2, 7] = "website";

                    foreach (Password password in passWords)
                    {
                        if (password.id == "ump")
                        {
                            continue;
                        }

                        i2++;
                        ws.Cells[i2, 1] = password.nr;
                        ws.Cells[i2, 2] = password.name;
                        ws.Cells[i2, 3] = password.category;
                        ws.Cells[i2, 4] = password.userName;
                        ws.Cells[i2, 5] = password.userPassword;
                        ws.Cells[i2, 6] = password.note;
                        ws.Cells[i2, 7] = password.website;

                    }
                    //wb.Save();
                    wb.SaveAs(saveFileDialog.FileName.ToString(), Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing);
                    //wb.Close(false, Type.Missing, Type.Missing);
                    //app.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exporting to Excell failed: " + ex.ToString());
            }
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            HelpWindows winHelp = new HelpWindows();
            winHelp.Show();
        }

        #endregion

            #region button events

        private void bOpenFile_Click(object sender, RoutedEventArgs e)
        {
            Boolean validMasterPassword = checkMasterPassword();
            string enteredMasterPassword = getEnteredMasterPassword();

            if (enteredMasterPassword != "")
            {
                if (validMasterPassword == true)
                {

                    string fileName = "";
                    OpenFileDialog openFileDialog = new OpenFileDialog();

                    try
                    {


                        if (openFileDialog.ShowDialog() == true)
                        {
                            fileName = openFileDialog.FileName.ToString();
                            openedPasswordFile = fileName;
                            DataSet ds = DecryptAndDeserialize(fileName);
                            string xml = ds.GetXml();

                            using (TextReader reader = new StringReader(xml))
                            {
                                XmlSerializer deserializer = new XmlSerializer(typeof(List<Password>),
                                    new XmlRootAttribute("password_list"));
                                passWords = (List<Password>)deserializer.Deserialize(reader);
                            }

                            string fileMasterPassword = "";
                            foreach (Password password in passWords)
                            {
                                fileMasterPassword = password.masterPassword;
                                break;
                            }

                            if (fileMasterPassword != "" && masterPassword == fileMasterPassword)
                            {

                                string file = setSavedPasswordFileInfo(fileName);
                                // MessageBox.Show("Password file: " + file + " opened succesfully.", "Master password!", MessageBoxButton.OK, MessageBoxImage.Warning);

                                tabItemPasswords.IsEnabled = true;
                                tabItemPasswords.Visibility = Visibility.Visible;
                                tabControlMain.SelectedIndex = 1;
                                menuItemOpenFile();
                                ucCategory.Items.Clear();
                                saveHistory.Push(passWords.Count() - 1);
                                showPassWords();
                                showPassWordPieChart();
                                showCatPieChart();

                            }
                            else
                            {
                                MessageBox.Show("You have entered the wrong master password for this password file. This password file can't be opened.", "Master password!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("This is not a valid PassDash XML password file");
                    }
                }
            }
            else
            {
                lerrMasterPassword.Text = "";
                lerrMasterPassword.Text = "To open a password file please enter a master password.";
            }
            //else
            //{
            //    MessageBox.Show("To open a password file please enter the corresponding master password.", "Master password!", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    tabControlMain.SelectedIndex = 0;
            //}
        }

        private void bNewFile_Click(object sender, RoutedEventArgs e)
        {
            Boolean validMasterPassword = checkMasterPassword();
            string enteredMasterPassword = getEnteredMasterPassword();

            if (enteredMasterPassword != "")
            {
                if (validMasterPassword == true)
                {
                    passWords = new List<Password>();
                    Password mpPassword = new Password();
                    mpPassword.id = "ump";
                    mpPassword.masterPassword = masterPassword;
                    mpPassword.name = "ump";
                    mpPassword.category = "";
                    mpPassword.dateTime = "";
                    mpPassword.note = "";
                    mpPassword.userName = "";
                    mpPassword.userPassword = "";
                    mpPassword.website = "";

                    passWords.Add(mpPassword);


                    Boolean saved = saveAsPassword(true);

                    if (saved == true)
                    {
                        ucCategory.Items.Clear();
                        this.bShowAllPasswords.Visibility = Visibility.Hidden;
                        this.uMasterPassword.Password = "";
                        this.tFreeSearch.Text = "";
                        this.lerrSearch.Text = "";

                        //openedPasswordFile = "";
                        //this.lpasswordFileName.Content = "";

                        resetPassWordForm();
                        showPassWords();
                        showPassWordPieChart();
                        showCatPieChart();

                        tabControlMain.SelectedIndex = 1;
                        tabItemPasswords.IsEnabled = true;
                        tabItemPasswords.Visibility = Visibility.Visible;
                        menuItemOpenFile();
                    }

                }
            }
            else
            {
                lerrMasterPassword.Text = "";
                lerrMasterPassword.Text = "To create a new password file please enter a master password.";
            }

        }


        private void chkMasterPassword_Click(object sender, RoutedEventArgs e)
        {
            if (chkMasterPassword.IsChecked == true)
            {
                uMasterPassword.Visibility = Visibility.Hidden;
                uTxtMasterPassword.Visibility = Visibility.Visible;
                uTxtMasterPassword.Text = uMasterPassword.Password.ToString();
            }
            else if (chkMasterPassword.IsChecked == false)
            {
                uTxtMasterPassword.Visibility = Visibility.Hidden;
                uMasterPassword.Visibility = Visibility.Visible;
                uMasterPassword.Password = uTxtMasterPassword.Text;
            }


        }
        private void openWebsite_Click(object sender, RoutedEventArgs e)
        {
            string website = this.uWebsite.Text;
            if (website != "")
            {
                try
                {
                    System.Diagnostics.Process.Start(website);
                }
                catch
                {

                }
            }
        }

        private void delPassword_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete this password?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {

            }
            else
            {
                if (selPassword != null)
                {
                    foreach (Password mypassword in passWords)
                    {
                        if (mypassword.id == selPassword.id)
                        {
                            passWords.Remove(mypassword);
                            break;
                        }
                    }
                    resetPassWordForm();
                    showPassWords();
                    showPassWordPieChart();
                    showCatPieChart();
                    selPassword = null;
                }
            }
        }

        private void addPassword_Click(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            listViewPasswords.Focus();

            if (this.uName.Text.Length <= 2)
            {
                valid = false;
                this.lerrName.Text = "Name must have 3 characters";
            }

            if (this.bAddPassword.Content.ToString() == "Edit")
            {

                if (selPassword != null)
                {
                    if (valid == true)
                    {
                        //Password password = (Password)listViewPasswords.SelectedItem;
                        string id = selPassword.id;

                        foreach (Password mypassword in passWords)
                        {
                            if (mypassword.id == id)
                            {
                                mypassword.name = this.uName.Text;
                                mypassword.category = this.ucCategory.Text;
                                mypassword.userName = this.uUsername.Text;
                                mypassword.website = this.uWebsite.Text;
                                mypassword.userPassword = this.uPassword.Text;
                                mypassword.note = this.uNote.Text;
                                mypassword.dateTime = DateTime.Now.ToShortDateString();
                                break;
                            }
                        }
                        showPassWordPieChart();
                        showCatPieChart();
                        resetPassWordForm();
                        selPassword = null;
                    }
                   
                }

            }
            else if (this.bAddPassword.Content.ToString() == "Add")
            {
                Password password = new Password();
                Guid obj = Guid.NewGuid();

                if (valid == true)
                {
                    password.id = obj.ToString();
                    password.name = this.uName.Text;
                    password.category = this.ucCategory.Text;
                    password.userName = this.uUsername.Text;
                    password.website = this.uWebsite.Text;
                    password.userPassword = this.uPassword.Text;
                    password.note = this.uNote.Text;
                    password.dateTime = DateTime.Now.ToShortDateString();

                    passWords.Add(password);
                    showPassWordPieChart();
                    showCatPieChart();
                    resetPassWordForm();
                }
            }
            showPassWords();
        }

        private void addNewPassword_Click(object sender, RoutedEventArgs e)
        {
            selPassword = null;
            resetPassWordForm();
        }

        private void showAllPasswords_Click(object sender, RoutedEventArgs e)
        {
            showPassWords();
            this.bShowAllPasswords.Visibility = Visibility.Hidden;
            this.tFreeSearch.Text = "";
            this.lerrSearch.Text = "";
        }

        private void searchAllPasswords_Click(object sender, RoutedEventArgs e)
        {
            List<Password> foundPasswords = new List<Password>();
            string searchQuery = tFreeSearch.Text;
            lerrSearch.Text = "";

            if (searchQuery.Length > 2)
            {

                foreach (Password password in passWords)
                {
                    if (password.id == "ump")
                    {
                        continue;
                    }

                    Boolean foundPassword = false;

                    if (password.name != null && password.name.ToLower().Contains(searchQuery.ToLower()))
                    {
                        foundPassword = true;

                    }
                    else if (password.userName != null && password.userName.ToLower().Contains(searchQuery.ToLower()))
                    {
                        foundPassword = true;
                    }
                    else if (password.userPassword != null && password.userPassword.ToLower().Contains(searchQuery.ToLower()))
                    {
                        foundPassword = true;
                    }
                    else if (password.note != null && password.note.ToLower().Contains(searchQuery.ToLower()))
                    {

                        foundPassword = true;
                    }
                    else if (password.category != null && password.category.ToLower().Contains(searchQuery.ToLower()))
                    {
                        foundPassword = true;
                    }
                    else if (password.strength != null && password.strength.ToLower().Contains(searchQuery.ToLower()))
                    {
                        foundPassword = true;
                    }


                    if (foundPassword == true)
                    {

                        foundPasswords.Add(password);
                    }
                }

                if (foundPasswords.Count > 0)
                {
                    showFoundPasswords(foundPasswords);
                }
                else
                {
                    lerrSearch.Text = "No passwords found.";

                }
            }
            else
            {
                lerrSearch.Text = "Search query should have at least 3 characters.";
            }
        }

        #endregion

        #region general events
        private void listView_Click(object sender, RoutedEventArgs e)
        {

            if ((Password)listViewPasswords.SelectedItem != null)
            {
                PasswordAdvisor passwordAdvisor = new PasswordAdvisor();

                this.bAddPassword.Content = "Edit";
                this.lpassWordForm.Content = "Edit/view your password:";
                this.bDelPassword.Visibility = Visibility.Visible;
                this.bOpenWebsite.Visibility = Visibility.Visible;
                this.passwordStrength.Visibility = Visibility.Visible;
                clearPasswordForm();

                clearErrors();
                Password password = (Password)listViewPasswords.SelectedItem;
                selPassword = null;
                string id = password.id;

                foreach (Password mypassword in passWords)
                {
                    if (mypassword.id == id)
                    {
                        selPassword = mypassword;
                        break;
                    }
                }

                if (selPassword.name != null)
                {
                    this.uName.Text = selPassword.name.ToString();
                }
                if (selPassword.userName != null)
                {
                    this.uUsername.Text = selPassword.userName.ToString();
                }
                if (selPassword.website != null)
                {
                    this.uWebsite.Text = selPassword.website.ToString();
                }
                if (selPassword.category != null)
                {
                    this.ucCategory.Text = selPassword.category.ToString();
                }

                if (selPassword.userPassword != null)
                {
                    this.uPassword.Text = selPassword.userPassword.ToString();
                    int passWordStrengthScore = passwordAdvisor.CheckStrength(selPassword.userPassword.ToString());

                    PasswordScore passScore = (PasswordScore)passWordStrengthScore;
                    string passWordStrengthText = getPassWordScoreText(passWordStrengthScore);
                    this.passwordStrength.Text = passWordStrengthText;

                    if (colorsPasswordStrengthChart.ContainsKey(passWordStrengthText))
                    {
                        this.passwordStrength.Background = colorsPasswordStrengthChart[passWordStrengthText];
                    }
                }
                if (selPassword.note != null)
                {
                    this.uNote.Text = selPassword.note.ToString();
                }
            }

        }
        #endregion

        #region encryption methods
        public DataSet DecryptAndDeserialize(string filename)
        {
            DataSet ds = new DataSet();
            FileStream aFileStream = new FileStream(filename, FileMode.Open);
            StreamReader aStreamReader = new StreamReader(aFileStream);
            UnicodeEncoding aUE = new UnicodeEncoding();
            byte[] key = aUE.GetBytes("password");
            RijndaelManaged RMCrypto = new RijndaelManaged();
            CryptoStream aCryptoStream = new CryptoStream(aFileStream, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

            //Restore the data set to memory.
            ds.ReadXml(aCryptoStream);
            aStreamReader.Close();
            aFileStream.Close();
            return ds;
        }
        #endregion

        #region save methods
        private void savePassword()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (passWords.Count() > 0)
            {
                if (masterPassword != null && masterPassword.Length > 5)
                {
                    foreach (Password password in passWords)
                    {
                        password.masterPassword = masterPassword;
                    }

                    if (openedPasswordFile == "")
                    {
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string fileName = saveFileDialog.FileName.ToString();
                            createXMLPasswords(fileName);
                            string file = setSavedPasswordFileInfo(fileName);
                            MessageBox.Show("Your password file: " + file + " is saved.", "Saved!", MessageBoxButton.OK, MessageBoxImage.Information);
                            saveHistory.Push(passWords.Count() - 1);
                            showPassWords();
                        }
                    }
                    else if (openedPasswordFile != "")
                    {
                        createXMLPasswords(openedPasswordFile);
                        string file = setSavedPasswordFileInfo(openedPasswordFile);
                        MessageBox.Show("Your password file: " + file + " is saved.", "Saved!", MessageBoxButton.OK, MessageBoxImage.Information);
                        saveHistory.Push(passWords.Count() - 1);
                        showPassWords();
                    }
                }
                else
                {
                    MessageBox.Show("To save a password file please enter a master password for the file.", "Master password!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tabControlMain.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("To save a password file please enter at least 1 password.", "Password", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private Boolean saveAsPassword(Boolean newFile)
        {
            Boolean saved = false;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (passWords.Count() > 0)
            {
                if (masterPassword != null && masterPassword.Length > 5)
                {
                    foreach (Password password in passWords)
                    {
                        password.masterPassword = masterPassword;
                    }

                    if (saveFileDialog.ShowDialog() == true)
                    {

                       string newFileText = "";
                       if (newFile == true)
                        {
                            newFileText = " is created.";

                        }
                       else if (newFile == false)
                        {
                            newFileText = " is saved.";
                        }

                        string fileName = saveFileDialog.FileName.ToString();
                        createXMLPasswords(fileName);
                        string file = setSavedPasswordFileInfo(fileName);
                        MessageBox.Show("Your password file: " + file + newFileText, "Saved!", MessageBoxButton.OK, MessageBoxImage.Information);
                        saved = true;
                        saveHistory.Push(passWords.Count() - 1);
                        showPassWords();
                    }
                }
                else
                {
                    MessageBox.Show("To save a password file please enter a master password for the file.", "Master password!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tabControlMain.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("To save a password file please enter at least 1 password.", "Password", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return saved;
        }


        private void createXMLPasswords(string fileName)
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(PasswordList));
            PasswordList list = new PasswordList();

            foreach (Password password in passWords)
            {
                list.Items.Add(password);
            }

            UnicodeEncoding aUE = new UnicodeEncoding();
            byte[] key = aUE.GetBytes("password");
            RijndaelManaged RMCrypto = new RijndaelManaged();

            using (FileStream fs = File.Open(fileName.Replace(".xml", "") + ".xml", FileMode.Create))
            {
                using (CryptoStream cs = new CryptoStream(fs, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write))
                {
                    XmlSerializer xmlser = new XmlSerializer(typeof(PasswordList));
                    xmlser.Serialize(cs, list);
                }
                fs.Close();
            }
        }

        private string setSavedPasswordFileInfo(string filePath)
        {
            openedPasswordFile = filePath;
            string file = System.IO.Path.GetFileName(filePath);
            file = file.Replace(".xml", "") + ".xml";
            lpasswordFileName.Text = file;
            return file;
        }

        #endregion

        #region general methods

        public void menuItemOpenFile()
        {
            menuItemCloseFile.IsEnabled = true;
            menuItemSave.IsEnabled = true;
            menuItemSaveAs.IsEnabled = true;
            menuItemExport.IsEnabled = true;
            menuItemExportExcell.IsEnabled = true;
            menuItemImport.IsEnabled = true;
            menuItemImportExcell.IsEnabled = true;
        }

        public void menuItemNoFile()
        {
            menuItemCloseFile.IsEnabled = false;
            menuItemSave.IsEnabled = false;
            menuItemSaveAs.IsEnabled = false;
            menuItemImport.IsEnabled = false;
            menuItemExportExcell.IsEnabled = false;
            menuItemImportExcell.IsEnabled = false;
            menuItemExport.IsEnabled = false;
        }

        private Boolean checkMasterPassword()
        {
            Boolean validMasterPassword = false;
            if (chkMasterPassword.IsChecked == true)
            {
                masterPassword = this.uTxtMasterPassword.Text.ToString();
            }
            else if (chkMasterPassword.IsChecked == false)
            {

                masterPassword = this.uMasterPassword.Password.ToString();
            }

            Match password = Regex.Match(masterPassword, @"
                                      ^              # Match the start of the string
                                       (?=.*\p{Lu})  # Positive lookahead assertion, is true when there is an uppercase letter
                                       (?=.*\P{L})   # Positive lookahead assertion, is true when there is a non-letter
                                       \S{8,}        # At least 8 non whitespace characters
                                      $              # Match the end of the string
                                     ", RegexOptions.IgnorePatternWhitespace);

            if (password.Success)
            {
                validMasterPassword = true;
                //MessageBox.Show("Master password saved. You can now save or open a password file.", "Master password!", MessageBoxButton.OK, MessageBoxImage.Information);
                //tabControlMain.SelectedIndex = 0;
            }
            else
            {
                lerrMasterPassword.Text = "";
                lerrMasterPassword.Text = "Invalid master password. The master password should have at least 8 characters, one uppercase letter, and one non-letter.";
            }
            return validMasterPassword;
        }

        private string getEnteredMasterPassword()
        {
            string enteredMasterPassword = "";

            if (chkMasterPassword.IsChecked == false)
            {
                enteredMasterPassword = uMasterPassword.Password.ToString();
            }
            else if (chkMasterPassword.IsChecked == true)
            {
                enteredMasterPassword = this.uTxtMasterPassword.Text.ToString();
            }
            return enteredMasterPassword;
        }

        private void showPassWords()
        {
            listViewPasswords.Items.Clear();

            int i = 1;
            foreach (Password password in passWords)
            {
                if (password.id == "ump")
                {
                    continue;
                }

                int score = passWordAdvisor.CheckStrength(password.userPassword);
                string passWordStrength = getPassWordScoreText(score);

                listViewPasswords.Items.Add(new Password { nr = i.ToString(), category = password.category, name = password.name, website = password.website, userName = password.userName, userPassword = password.userPassword, dateTime = password.dateTime, id = password.id, strength = passWordStrength });
                i = i + 1;

                if (!ucCategory.Items.Contains(password.category))
                {
                    ucCategory.Items.Add(password.category);
                }

            }

            lpasswordListView.Content = "My Passwords" + " (" + (passWords.Count -1).ToString() + ")" + ":";
            int lastSavePasswordCount = 0;
            if (saveHistory.Count > 0)
            {
                lastSavePasswordCount = saveHistory.Peek();
            }

            if (lastSavePasswordCount != passWords.Count() - 1)
            {
                lpasswordFileName.Foreground = Brushes.Red;
            }
            else if (lastSavePasswordCount == passWords.Count() - 1)
            {
                lpasswordFileName.Foreground = Brushes.Yellow;
            }

        }

        private void resetPassWordForm()
        {
            clearPasswordForm();
            this.bDelPassword.Visibility = Visibility.Hidden;
            this.bOpenWebsite.Visibility = Visibility.Hidden;
            this.passwordStrength.Visibility = Visibility.Hidden;
            this.bAddPassword.Content = "Add";
            this.lpassWordForm.Content = "Create a new password:";
        }


        private void clearPasswordForm()
        {
            this.uName.Text = "";
            this.uUsername.Text = "";
            this.uWebsite.Text = "";
            this.uPassword.Text = "";
            this.ucCategory.Text = "";
            this.uNote.Text = "";

            this.lerrName.Text = "";
            this.lerrPassword.Text = "";
            this.lerrUserName.Text = "";
            this.lerrWebsite.Text = "";
        }

        private void clearErrors()
        {
            this.lerrName.Text = "";
            this.lerrPassword.Text = "";
            this.lerrUserName.Text = "";
            this.lerrWebsite.Text = "";
        }


        public void showFoundPasswords(List<Password> foundPasswords)
        {
            listViewPasswords.Items.Clear();

            int i = 1;
            foreach (Password password in foundPasswords)
            {
                int score = passWordAdvisor.CheckStrength(password.userPassword);
                string passWordStrength = getPassWordScoreText(score);

                listViewPasswords.Items.Add(new Password { nr = i.ToString(), category = password.category, name = password.name, website = password.website, userName = password.userName, userPassword = password.userPassword, dateTime = password.dateTime, id = password.id, strength = passWordStrength });
                i = i + 1;
            }

            if (foundPasswords.Count < passWords.Count() - 1)
            {
                this.bShowAllPasswords.Visibility = Visibility.Visible;
            }

            lerrSearch.Text = "";
            lpasswordListView.Content = "My Passwords" + " (" + foundPasswords.Count + "/" + (passWords.Count() - 1).ToString() + ")" + ":";
        }

        #endregion

        #region chart methods
        private void showCatPieChart()
        {

            Dictionary<string, int> categories = new Dictionary<string, int>();

            foreach (Password password in passWords)
            {
                if (password.id == "ump")
                { 
                    continue;
                }

                int count = 1;
                if (password.category != null && password.category != "")
                {
                    if (categories.ContainsKey(password.category))
                    {
                        count = categories[password.category];
                        count = count + 1;
                        categories[password.category] = count;
                    }
                    else
                    {
                        categories.Add(password.category, count);
                    }
                }
            }

            SeriesCollectionCat = new SeriesCollection();
            foreach (KeyValuePair<string, int> entry in categories)
            {

                PieSeries pieSeries = new PieSeries();
                pieSeries.Title = entry.Key.ToString();
                pieSeries.ToolTip = null;
                pieSeries.FontSize = 11;
                pieSeries.Foreground  = new SolidColorBrush(Colors.Black);
                pieSeries.Values = new ChartValues<ObservableValue> { new ObservableValue(entry.Value) };
                pieSeries.LabelPoint = chartPoint =>
                string.Format(entry.Key.ToString() + "(" + entry.Value.ToString() + ")");

                pieSeries.LabelPosition = PieLabelPosition.OutsideSlice;

                //pieSeries.Values.Add(n9ew ChartValues<ObservableValue> { new ObservableValue(entry.Value) });
                pieSeries.DataLabels = true;

                SeriesCollectionCat.Add(pieSeries);
            }

            ChartCat.DataTooltip = null;
            ChartCat.Series = SeriesCollectionCat;
            //ChartCat.Content = "Categories:";
            ChartCat.DataContext = this;


        }

        private void showPassWordPieChart()
        {
            PasswordAdvisor passwordAdvisor = new PasswordAdvisor();

            Dictionary<string, int> dictPassScores = new Dictionary<string, int>();

            List<int> passScores = new List<int>();
            dictPassScores.Add("Blank", 0);
            dictPassScores.Add("Very Weak", 0);
            dictPassScores.Add("Weak", 0);
            dictPassScores.Add("Medium", 0);
            dictPassScores.Add("Strong", 0);
            dictPassScores.Add("Very Strong", 0);

            foreach (Password password in passWords)
            {
                if (password.id == "ump")
                {
                    continue;
                }

                int score = passwordAdvisor.CheckStrength(password.userPassword);
                passScores.Add(score);

                int count = 1;
                string passWordStrength = getPassWordScoreText(score);

                if (dictPassScores.ContainsKey(passWordStrength))
                {
                    count = dictPassScores[passWordStrength];
                    count = count + 1;
                    dictPassScores[passWordStrength] = count;
                }
                else
                {
                    dictPassScores.Add(passWordStrength, count);
                }
            }

            SeriesCollectionPass = new SeriesCollection();
            if (passWords.Count() - 1 > 0)
            {
                foreach (KeyValuePair<string, int> entry in dictPassScores)
                {
                    PieSeries pieSeries = new PieSeries();
                    if (entry.Key.ToString() == "Blank")
                    {
                        pieSeries.Fill = Brushes.LightBlue;
                    }
                    else if (entry.Key.ToString() == "Very Weak")
                    {
                        pieSeries.Fill = Brushes.Red;
                    }
                    else if (entry.Key.ToString() == "Weak")
                    {
                        pieSeries.Fill = Brushes.Orange;
                    }
                    else if (entry.Key.ToString() == "Medium")
                    {
                        pieSeries.Fill = Brushes.YellowGreen;
                    }
                    else if (entry.Key.ToString() == "Strong")
                    {
                        pieSeries.Fill = Brushes.LimeGreen;
                    }
                    else if (entry.Key.ToString() == "Very Strong")
                    {
                        pieSeries.Fill = Brushes.DarkGreen;
                    }

                    pieSeries.Title = entry.Key.ToString();
                    pieSeries.ToolTip = null;
                    pieSeries.FontSize = 11;
                    pieSeries.Foreground = new SolidColorBrush(Colors.Black);
                    pieSeries.Values = new ChartValues<ObservableValue> { new ObservableValue(entry.Value) };
                    if (entry.Value > 0)
                    {
                        pieSeries.LabelPoint = chartPoint =>
                        string.Format(entry.Key.ToString() + "(" + entry.Value.ToString() + ")");
                    }
                    else
                    {
                        pieSeries.LabelPoint = chartPoint =>
                        string.Format("");
                    }

                    pieSeries.LabelPosition = PieLabelPosition.OutsideSlice;
                    pieSeries.DataLabels = true;
                    SeriesCollectionPass.Add(pieSeries);
                }


                foreach (PieSeries PieSerie in SeriesCollectionPass)
                {
                    if (!colorsPasswordStrengthChart.ContainsKey(PieSerie.Title))
                    {
                        colorsPasswordStrengthChart.Add(PieSerie.Title, PieSerie.Fill);
                    }
                }

                ChartPass.DataTooltip = null;
                ChartPass.Series = SeriesCollectionPass;
                ChartPass.DataContext = this;
            }
            else
            {
                ChartPass.DataTooltip = null;
                ChartPass.Series = null;
                ChartPass.DataContext = this;
            }
        }
        #endregion

        #region chart event

        private void ChartCatOnDataClick(object sender, ChartPoint p)
        {
            this.tFreeSearch.Text = "";
            this.lerrSearch.Text = "";
            PieSeries pieSeries = new PieSeries();
            pieSeries.LabelPoint = p.SeriesView.LabelPoint;   
            string labelName = pieSeries.LabelPoint(p);

            string[] labelNames = labelName.Split('(');

            string category = "";
            if (labelNames[0] != null)
            {
                category = labelNames[0];
            }

            List<Password> foundPasswords = new List<Password>();

            foreach (Password password in passWords)
            {
                if (password.id == "ump")
                {
                    continue;
                }

                if (password.category == category)
                {
                    foundPasswords.Add(password);
                }
            }

            showFoundPasswords(foundPasswords);
        }


        private void ChartPassStrengthOnDataClick(object sender, ChartPoint p)
        {

            PasswordAdvisor passWordAdvisor = new PasswordAdvisor();
            this.tFreeSearch.Text = "";
            this.lerrSearch.Text = "";
            PieSeries pieSeries = new PieSeries();
            pieSeries.LabelPoint = p.SeriesView.LabelPoint;
            string labelName = pieSeries.LabelPoint(p);

            string[] labelNames = labelName.Split('(');

            string passWordStrength = "";
            if (labelNames[0] != null)
            {
                passWordStrength = labelNames[0];
            }

            List<Password> foundPasswords = new List<Password>();

            foreach (Password password in passWords)
            {
                if (password.id == "ump")
                {
                    continue;
                }

                string _password = password.userPassword;
                int passWordScore = passWordAdvisor.CheckStrength(_password);

                string _passWordStrenght = getPassWordScoreText(passWordScore);
                if (_passWordStrenght == passWordStrength)
                {
                    foundPasswords.Add(password);
                }
            }
            showFoundPasswords(foundPasswords);
        }

        #endregion

            #region enums
        public enum PasswordScore
        {
            Blank = 0,
            Veryweak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        private string getPassWordScoreText(int score)
        {
            string passWordScoreText = "";

            if (score == 0)
            {
                passWordScoreText = "Blank";
            }
            else if (score == 1)
            {
                passWordScoreText = "Very Weak";
            }
            else if (score == 2)
            {
                passWordScoreText = "Weak";
            }
            else if (score == 3)
            {
                passWordScoreText = "Medium";
            }
            else if (score == 4)
            {
                passWordScoreText = "Strong";
            }
            else if (score == 5)
            {
                passWordScoreText = "Very Strong";
            }
            return passWordScoreText;

        }

        #endregion

        #region test

        private void testData()
        {
            //passWords.Add(new Password { name = "111", category= "new", website = "www.nu.nl", userName = "Wi", userPassword = "1234567", note = "note1", dateTime = DateTime.Now.ToShortDateString().ToString(), id = Guid.NewGuid().ToString() });
            //passWords.Add(new Password { name = "111", category = "new", website = "www.nu.nl", userName = "Ro", userPassword = "1234567", note = "note2", dateTime = DateTime.Now.ToShortDateString().ToString(), id = Guid.NewGuid().ToString() });
            // passWords.Add(new Password { name = "111", category = "new", website = "www.nu.nl", userName = "Eg", userPassword = "1234567", note = "note3", dateTime = DateTime.Now.ToShortDateString().ToString(), id = Guid.NewGuid().ToString() });
            // passWords.Add(new Password { name = "111", category = "new", website = "www.nu.nl", userName = "Wi", userPassword = "1234567", note = "note4", dateTime = DateTime.Now.ToShortDateString().ToString(), id = Guid.NewGuid().ToString() });
        }
        #endregion

    }
}

