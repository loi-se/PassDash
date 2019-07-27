using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PassDash
{
    /// <summary>
    /// Interaction logic for HelpWindows.xaml
    /// </summary>
    public partial class HelpWindows : Window
    {
        public HelpWindows()
        {
            InitializeComponent();
       
            setTreeView();
            about();
        }


        private void exit_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Application.Current.Shutdown();
            this.Close();
        }

        public void setTreeView()
        {

            //TreeNode rootNode = new TreeNode();
            //rootNode.Text = "CustomerList";

            TreeViewItem treeViewItemAbout = new TreeViewItem();
            treeViewItemAbout.Name = "About";
            treeViewItemAbout.IsExpanded = true;
            treeViewItemAbout.Header = "About";
            treeViewItemAbout.FontSize = 14;
            treeViewItemAbout.MouseLeftButtonUp += treeViewItem_MouseLeftButtonUp;

            TreeViewItem treeViewItemSecurity = new TreeViewItem();
            treeViewItemSecurity.Name = "Security";
            treeViewItemSecurity.IsExpanded = true;
            treeViewItemSecurity.Header = "Security";
            treeViewItemSecurity.FontSize = 14;
            treeViewItemSecurity.MouseLeftButtonUp += treeViewItem_MouseLeftButtonUp;


            TreeViewItem treeViewItemFunc = new TreeViewItem();
            treeViewItemFunc.Name = "Functonalities";
            treeViewItemFunc.IsExpanded = true;
            treeViewItemFunc.Header = "Main functionalities";
            treeViewItemFunc.FontSize = 14;

            TreeViewItem treeViewItemCreate = new TreeViewItem();
            treeViewItemCreate.Name = "CreatePasswordFile";
            treeViewItemCreate.Header = "Create a new password file";
            treeViewItemCreate.FontSize = 14;
            treeViewItemCreate.IsExpanded = true;
            treeViewItemCreate.MouseLeftButtonUp += treeViewItem_MouseLeftButtonUp;

            TreeViewItem treeViewItemOpen = new TreeViewItem();
            treeViewItemOpen.Name = "OpenPasswordFile";
            treeViewItemOpen.Header = "Open a password file";
            treeViewItemOpen.FontSize = 14;
            treeViewItemOpen.IsExpanded = true;
            treeViewItemOpen.MouseLeftButtonUp += treeViewItem_MouseLeftButtonUp;


            TreeViewItem treeViewItemImport = new TreeViewItem();
            treeViewItemImport.Name = "ImportExcell";
            treeViewItemImport.Header = "Import passwords from an excell file";
            treeViewItemImport.FontSize = 14;
            treeViewItemImport.IsExpanded = true;
            treeViewItemImport.MouseLeftButtonUp += treeViewItem_MouseLeftButtonUp;

            treeViewItemFunc.Items.Add(treeViewItemCreate);
            treeViewItemFunc.Items.Add(treeViewItemOpen);
            treeViewItemFunc.Items.Add(treeViewItemImport);

         
            this.treeViewHelp.Items.Add(treeViewItemAbout);
            this.treeViewHelp.Items.Add(treeViewItemSecurity);
            this.treeViewHelp.Items.Add(treeViewItemFunc);
            
        }

        public void treeViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {


            TreeViewItem treeViewItem = treeViewHelp.SelectedItem as TreeViewItem;
            txtBlockHelp1.Inlines.Clear();
            txtBlockHelp2.Inlines.Clear();

            if (treeViewItem.Name == "CreatePasswordFile")
            {
                CreatePasswordFile();
            }
            else if (treeViewItem.Name == "OpenPasswordFile")
            {
                OpenPasswordFile();
            }
            else if (treeViewItem.Name == "ImportExcell")
            {

                importFunction();
            }
            else if (treeViewItem.Name == "About")
            {
                about();
            }

            else if (treeViewItem.Name == "Security")
            {
                security();
            }

        }



        private void about()
        {
            ImageSource imageSource = new BitmapImage(new Uri(@"Images/ScreenShot1.jpg", UriKind.Relative));
            imgHelp.Source = imageSource;
            imgHelp.Width = 900;
            imgHelp.Height = 550;

            txtBlockHelp1.FontSize = 14; // 24 points
            txtBlockHelp1.Inlines.Add(new Bold(new Run("About PassDash." + Environment.NewLine + Environment.NewLine)));
            txtBlockHelp1.Inlines.Add("PassDash is a windows desktop program to store all your passwords in a secure and simple way." + Environment.NewLine +
                                       "PassDash offers a clear overview over all your password data through a clear dashboard screen: hence the name PassDash.");

            txtBlockHelp2.FontSize = 14; // 24 points
            txtBlockHelp2.Inlines.Add("All passwords are stored in an encrypted XML file on your computer harddisk. A master password is linked to each file. To save a " + Environment.NewLine +
                                       "password file the user has to enter a master password for the file." + Environment.NewLine +
                                       "When opening a password file the user needs to enter the corresponding master password again. " + Environment.NewLine +
                                       "After filling in the right master password the user has access to all password data entered and saved previously in this password file. " + Environment.NewLine +
                                       "In this way a user can have multiple password files with each it's unique master password coupled to it. " + Environment.NewLine +
                                       "This offers our users a flexible, clear, and safe way to store passwords.");

        }

        private void security()
        {
            ImageSource imageSource = new BitmapImage(new Uri(@"Images/Security diagram.png", UriKind.Relative));
            imgHelp.Source = imageSource;
            imgHelp.Width = 800;
            imgHelp.Height = 550;

            txtBlockHelp1.FontSize = 14; // 24 points
            txtBlockHelp1.Inlines.Add(new Bold(new Run("Security." + Environment.NewLine + Environment.NewLine)));
            txtBlockHelp1.Inlines.Add("PassDash ensures the security of your password files in the following way:" + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- Password files are encrypted with a modern encyption algorithm." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- Password files can be stored on your own PC." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- PassDash is an offline windows desktop application." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- Every password file has it's own unique master password. It is important that the user picks a hard-to-guess and an easy to remember master password." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add(" There is no mechanism within PassDash to retrieve lost or forgotten master passwords. The user is responsible for storing his master password." + Environment.NewLine);

        }
        private void CreatePasswordFile()
        {


            ImageSource imageSource = new BitmapImage(new Uri(@"Images/CreateNewHelp1.jpg", UriKind.Relative));
            imgHelp.Source = imageSource;
            imgHelp.Width = 1100;
            imgHelp.Height = 700;


            txtBlockHelp1.FontSize = 14; // 24 points
            txtBlockHelp1.Inlines.Add(new Bold(new Run("Create a new password file." + Environment.NewLine + Environment.NewLine)));
            txtBlockHelp1.Inlines.Add("To create a new password file do the following:" + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- Enter a master password for the file at the master password tabpage." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- Press the button: create new file." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- Enter the filename and press save." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- The password file is created and the dashboard tabpage opens." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add(" Here you can add new passwords to your new password file, edit and delete passwords, and get good insight in all your password data through graphs." + Environment.NewLine);

        }

        private void OpenPasswordFile()
        {
            ImageSource imageSource = new BitmapImage(new Uri(@"Images/CreateNewHelp1.jpg", UriKind.Relative));
            imgHelp.Source = imageSource;
            imgHelp.Width = 1100;
            imgHelp.Height = 700;

            txtBlockHelp1.FontSize = 14; // 24 points
            txtBlockHelp1.Inlines.Add(new Bold(new Run("Open a password file." + Environment.NewLine + Environment.NewLine)));
            txtBlockHelp1.Inlines.Add("To open a password file do the following:" + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- Enter the master password of the file at the master password tabpage." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- Press the button: open file." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- Select the file and press open." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("- The password file is opened and the dashboard tabpage opens." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add(" Here you can add new passwords to your password file, edit and delete passwords, and get good insight in all your password data through graphs." + Environment.NewLine);

        }

        private void importFunction()
        {
            ImageSource imageSource = new BitmapImage(new Uri(@"Images/ImportHelp1.jpg", UriKind.Relative));
            imgHelp.Source = imageSource;
            imgHelp.Width = 600;
            imgHelp.Height = 400;


            txtBlockHelp1.FontSize = 14; // 24 points
            txtBlockHelp1.Inlines.Add(new Bold(new Run("Import passwords from an excell file." + Environment.NewLine + Environment.NewLine)));
            txtBlockHelp1.Inlines.Add("With this functionality it is possible to import passwords from an excell file." + Environment.NewLine);
            txtBlockHelp1.Inlines.Add("To do this do the following in the Excell file you want to import: " + Environment.NewLine);

            txtBlockHelp1.Inlines.Add("-In the column that contains the passwords (if present) place the following text at the first row:");
            txtBlockHelp1.Inlines.Add(new Bold(new Run(" password" + Environment.NewLine)));

            txtBlockHelp1.Inlines.Add("-In the column that contains the usernames (if present) place the following text at the first row:");
            txtBlockHelp1.Inlines.Add(new Bold(new Run(" username" + Environment.NewLine)));

            txtBlockHelp1.Inlines.Add("-In the column that contains the names (if present) place the following text at the first row:");
            txtBlockHelp1.Inlines.Add(new Bold(new Run(" name" + Environment.NewLine)));

            txtBlockHelp1.Inlines.Add("-In the column that contains the categories (if present) place the following text at the first row:");
            txtBlockHelp1.Inlines.Add(new Bold(new Run(" category" + Environment.NewLine)));

            txtBlockHelp1.Inlines.Add("-In the column that contains the notes (if present) place the following text at the first row:");
            txtBlockHelp1.Inlines.Add(new Bold(new Run(" note" + Environment.NewLine + Environment.NewLine)));

            txtBlockHelp1.Inlines.Add("You can see an example in the image below:");

            txtBlockHelp2.Inlines.Add("It is important to place the texts at the ");
            txtBlockHelp2.Inlines.Add(new Bold(new Run(" first row")));
            txtBlockHelp2.Inlines.Add(" of the Excell file. The texts need to be lowercase." + Environment.NewLine);

            txtBlockHelp2.FontSize = 14;
            txtBlockHelp2.Inlines.Add("At least the column: ");
            txtBlockHelp2.Inlines.Add(new Bold(new Run(" name ")));
            txtBlockHelp2.Inlines.Add(" or the column: ");
            txtBlockHelp2.Inlines.Add(new Bold(new Run(" username ")));
            txtBlockHelp2.Inlines.Add(" must be present.");
            txtBlockHelp2.Inlines.Add(" The other columns do not have to be present. ");
        }
    }
}
