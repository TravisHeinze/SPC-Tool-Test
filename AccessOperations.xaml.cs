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

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for AccessOperations.xaml
    /// </summary>
    public partial class AccessOperations : Window
    {
        public AccessOperations()
        {
            InitializeComponent();
        }

        private void ChangePermissions_Click(object sender, RoutedEventArgs e)
        {
            ModifyPermissions modifyPermissions = new ModifyPermissions();
            modifyPermissions.Show();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NewUser newUser = new NewUser();
            newUser.Show();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            RemoveUser removeUser = new RemoveUser();
            removeUser.Show();
        }
    }
}
