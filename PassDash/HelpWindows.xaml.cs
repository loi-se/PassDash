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
            treeViewItemImport.MouseLeftButtonUp += treeViewItemImport_MouseLeftButtonUp;

            treeViewItemFunc.Items.Add(treeViewItemImport);

            this.treeViewHelp.Items.Add(treeViewItemAbout);
            this.treeViewHelp.Items.Add(treeViewItemFunc);
            
        }

        public void treeViewItemImport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem  = treeViewHelp.SelectedItem as TreeViewItem;
            txtBlockHelp1.Inlines.Clear();
            txtBlockHelp2.Inlines.Clear();
            if (treeViewItem.Name == "ImportExcell")
            {
                ImageSource imageSource = new BitmapImage(new Uri(@"Images/ImportHelp1.jpg", UriKind.Relative));
                imgHelp.Source = imageSource;
                imgHelp.Width = 600;
                imgHelp.Height = 400;


                txtBlockHelp1.FontSize = 14; // 24 points
                txtBlockHelp1.Inlines.Add(new Bold(new Run("Import passwords from an excell file:" + Environment.NewLine + Environment.NewLine)));
                txtBlockHelp1.Inlines.Add("Do the following in the Excell file you want to import: " + Environment.NewLine);

                txtBlockHelp1.Inlines.Add("-In the column that contains the passwords place the following text at the first row:");
                txtBlockHelp1.Inlines.Add(new Bold(new Run(" password" + Environment.NewLine)));

                txtBlockHelp1.Inlines.Add("-In the column that contains the usernames place the following text at the first row:");
                txtBlockHelp1.Inlines.Add(new Bold(new Run(" username" + Environment.NewLine)));

                txtBlockHelp1.Inlines.Add("-In the column that contains the names place the following text at the first row:");
                txtBlockHelp1.Inlines.Add(new Bold(new Run(" name" + Environment.NewLine)));

                txtBlockHelp1.Inlines.Add("-In the column that contains the categories place the following text at the first row:");
                txtBlockHelp1.Inlines.Add(new Bold(new Run(" category" + Environment.NewLine)));

                txtBlockHelp1.Inlines.Add("-In the column that contains the notes place the following text at the first row:");
                txtBlockHelp1.Inlines.Add(new Bold(new Run(" note" + Environment.NewLine + Environment.NewLine)));

                txtBlockHelp1.Inlines.Add("You can see an example in the image below. It is important to place the texts at the");
                txtBlockHelp1.Inlines.Add(new Bold(new Run(" first row")));
                txtBlockHelp1.Inlines.Add(" of the Excell file");

                txtBlockHelp2.FontSize = 14;
                txtBlockHelp2.Inlines.Add("At least the text: ");
                txtBlockHelp2.Inlines.Add(new Bold(new Run(" name ")));
                txtBlockHelp2.Inlines.Add(" or the text: ");
                txtBlockHelp2.Inlines.Add(new Bold(new Run(" username ")));
                txtBlockHelp2.Inlines.Add(" must be present.");
                txtBlockHelp2.Inlines.Add(" The other texts do not have to be present. ");

            }
        }
    }
}
