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

namespace Apollo.View.Views
{
    /// <summary>
    /// Non-class.  Since ApolloUserControl is a generic class, it can't be defined in XAML (maybe it can, but don't
    /// know how).  Define this wrapper class to allow us to create the derived type in XAML.
    /// </summary>
    public abstract class CreateRaceViewType : ApolloViewBase<CreateModifyRaceViewModel>
    { }

    /// <summary>
    /// Interaction logic for CreateRaceControl.xaml
    /// </summary>
    public partial class CreateRaceView : CreateRaceViewType
    {
        public CreateRaceView()
        {
            InitializeComponent();
        }
    }

    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return String.Format("{0:0.00}", (double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double number;
            if (double.TryParse((string)value, out number))
            {
                if (number <= 0.0)
                    return new ValidationResult(false, "Race distance cannot be negative");
                return number;
            }

            var result = new ValidationResult(false, "Error");
            return new ValidationResult(false, "Not a number!");
        }
    }

    #region Test Presentation

    internal class TestCreateRacePresentation
    {
        public string RaceName { get; set; }
        public DateTime RaceDate { get; set; }
        public double DistanceMiles { get; set; }
        public bool ChipTimed { get; set; }

        public Brush BackgroundColor { get { return Brushes.White; } }

        public TestCreateRacePresentation()
        {
            RaceName = "Random State Fair 5-K Race";
            RaceDate = new DateTime(2012, 8, 22);
            DistanceMiles = 3.20;
            ChipTimed = true;
        }
    }

    #endregion
}
