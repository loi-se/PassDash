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


            TreeViewItem treeViewItemFunc = new TreeViewItem();
            treeViewItemFunc.Name = "Functonalities";
            treeViewItemFunc.IsExpanded = true;
            treeViewItemFunc.Header = "Main functionalities";
            treeViewItemFunc.FontSize = 14;

            TreeViewItem treeViewItemImport = new TreeViewItem();
            treeViewItemImport.Name = "ImportExcell";
            treeViewItemImport.Header = "Import passwords from an excell file";
            treeViewItemImport.FontSize = 14;
            treeViewItemImport.IsExpanded = true;
            treeViewItemImport.MouseLeftButtonUp += treeViewItem_MouseLeftButtonUp;

            treeViewItemFunc.Items.Add(treeViewItemImport);

            this.treeViewHelp.Items.Add(treeViewItemAbout);
            this.treeViewHelp.Items.Add(treeViewItemFunc);
            
        }

        public void treeViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem  = treeViewHelp.SelectedItem as TreeViewItem;
            txtBlockHelp1.Inlines.Clear();
            txtBlockHelp2.Inlines.Clear();
            if (treeViewItem.Name == "ImportExcell")
            {

                importFunction();
            }
            else if (treeViewItem.Name == "About")
            {
                about();
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
