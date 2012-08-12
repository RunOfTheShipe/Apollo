using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Apollo.Model;

namespace Apollo.ViewModel
{
    public abstract class ApolloViewModelBase : INotifyPropertyChanged
    {
        protected ApolloModel _ApolloModel;
        internal ApolloViewModelBase(ApolloModel model)
        {
            _ApolloModel = model;
        }

        public void CommitTransaction()
        {
            _ApolloModel.CommitTransaction();
        }

        public void AbortTransaction()
        {
            _ApolloModel.AbortTransaction();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
