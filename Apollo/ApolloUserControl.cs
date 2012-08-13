using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using Apollo.ViewModel;

namespace Apollo.View
{
    public class ApolloUserControl<T> : UserControl
        where T : ApolloViewModelBase
    {
        public T Presentation
        {
            get { return (T)DataContext; }
            set { DataContext = value; }
        }
    }
}
