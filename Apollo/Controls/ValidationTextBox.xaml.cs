using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Apollo.View.Controls
{
    /// <summary>
    /// Interaction logic for ValidationTextBox.xaml
    /// </summary>
    public partial class ValidationTextBox : TextBox
    {
        public ValidationTextBox()
        {
            InitializeComponent();

            Style = new Style();

            Style.TargetType = typeof(TextBox);

            var trigger = new Trigger();
            trigger.Property = TextBox.VisibilityProperty;
            trigger.Value = Visibility.Hidden;


            var setter = new Setter();
            setter.Property = TextBox.ToolTipProperty;
            setter.Value = "Error detected!";

            trigger.Setters.Add(setter);
        }
    }
}
