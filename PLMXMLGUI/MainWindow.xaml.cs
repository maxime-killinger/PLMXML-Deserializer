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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLMXMLGUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            DateTime starttime = DateTime.Now;
            textBlock.Text = "L'export de " + (textBox.Text.Split(new char[] { '\\' }).Last<string>() == "" ? textBox.Text : textBox.Text.Split(new char[] { '\\' }).Last<string>()) + " à démarré\n" + textBlock.Text;
            int i = PLMXMLParser.Exportation.Export(textBox.Text);
            if (i == 0)
                textBlock.Text = @"L'export à bien été fait dans C:\Export\" + (textBox.Text.Split(new char[] { '\\' }).Last<string>() == "" ? textBox.Text : textBox.Text.Split(new char[] { '\\' }).Last<string>()) + ".csv\n" + textBlock.Text;
            else if (i == -2)
                textBlock.Text = @"Le fichier " + (textBox.Text.Split(new char[] { '\\' }).Last<string>() == "" ? textBox.Text : textBox.Text.Split(new char[] { '\\' }).Last<string>()) + " n\'existe pas\n" + textBlock.Text;
            else
                textBlock.Text = @"Une erreur est survenur durant l'export\n" + (textBox.Text.Split(new char[] { '\\' }).Last<string>() == "" ? textBox.Text : textBox.Text.Split(new char[] { '\\' }).Last<string>()) + textBlock.Text;
            textBlock1.Text = "Temps d'éxécution : " + DateTime.Now.Subtract(starttime);
        }
    }
}
