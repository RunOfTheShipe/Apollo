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

using Apollo.ViewModel;

namespace Apollo.View.Controls
{
    /// <summary>
    /// Non-class.  Since ApolloUserControl is a generic class, it can't be defined in XAML (maybe it can, but don't
    /// know how).  Define this wrapper class to allow us to create the derived type in XAML.
    /// </summary>
    public abstract class CreateRaceControlType : ApolloUserControl<CreateModifyRaceViewModel>
    { }

    /// <summary>
    /// Interaction logic for CreateRaceControl.xaml
    /// </summary>
    public partial class CreateRaceControl : CreateRaceControlType
    {
        public CreateRaceControl()
        {
            //? How to get the Apollo.Model in here?
            Presentation = new CreateModifyRaceViewModel(null, null);
            InitializeComponent();
        }
    }
}
