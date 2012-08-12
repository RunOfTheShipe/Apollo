using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Apollo.Model.Data;

namespace Apollo.Model
{
    /// <summary>
    /// Is an adapter class around a DataSet, abstracts logic around opening/closing/saving, creating accessor objects
    /// to the dataset, and querying the dataset.
    /// </summary>
    public class ApolloModel : IDisposable
    {
        #region Private Variables

        private Timer _SaveTimer;
        private ManualResetEvent _SaveCompleteEvent;
        private object _DatasetLock;


        private string _FullFilePath;
        internal RawData _RawData;
        private bool _InMemoryOnly;

        #endregion

        #region Construction
        /// <summary>
        /// Constructs an instance of the ApolloModel for use.  If no file path is given, will construct and in-memory
        /// model only.
        /// </summary>
        /// <param name="fullFilePath">(Optional) Reads in an initial state of the ApolloModel.</param>
        /// <param name="inMemoryOnly"></param>
        /// <returns>ApolloModel for use.</returns>
        public static ApolloModel MakeApolloModel(string fullFilePath, bool inMemoryOnly)
        {
            var model = new ApolloModel();
            model.Initialize(fullFilePath, inMemoryOnly);
            return model;
        }

        internal ApolloModel()
        {
            _RawData = null;
            _SaveTimer = null;
            _DatasetLock = new object();
            _SaveCompleteEvent = null;

            _FullFilePath = String.Empty;
            _InMemoryOnly = true;
        }
        
        /// <summary>
        /// Initializes the underlying data model by reading in the contents of the given XML file.
        /// </summary>
        void Initialize(string fullFilePath, bool bInMemoryOnly)
        {
            _RawData = new RawData();
            _InMemoryOnly = bInMemoryOnly;
            _FullFilePath = fullFilePath;

            if (!String.IsNullOrWhiteSpace(_FullFilePath))
            {
                // May be the case that the given file doesn't exist right now if the user wants to create a new one.
                // Don't try to read if it doesn't exist, we'll just save the file path for later to save changes to.
                if (File.Exists(_FullFilePath))
                    _RawData.ReadXml(_FullFilePath);
            }
        }
        #endregion

        #region Transactions
        /// <summary>
        /// Commits the current transaction to the dataset.  Also triggers a timer to save the changes to disk.
        /// </summary>
        public void CommitTransaction()
        {
            lock (_DatasetLock)
            {
                _RawData.AcceptChanges();

                // If a timer has not already been started, start one to run in 60 seconds to commit the changes to 
                // the disk.
                if (null == _SaveTimer)
                {
                    _SaveTimer = new Timer(SaveDataset_Timer, null, 60*1000, Timeout.Infinite);
                }
            }
        }

        /// <summary>
        /// Aborts the current transaction.  Rolls changes back to the last time CommitTransaction() was called.
        /// </summary>
        public void AbortTransaction()
        {
            lock (_DatasetLock)
            {
                _RawData.RejectChanges();
            }
        }
        #endregion

        #region Save Model to Disk
        /// <summary>
        /// Callback from the save timer.  Will save the dataset to disk if not an in-memory only copy of the model.
        /// </summary>
        /// <param name="obj">Not used.</param>
        void SaveDataset_Timer(object obj)
        {
            // Each of the below functions have their own locks around _DatasetLock, but combine them into
            // one "transaction" so some other commit doesn't get in there and mess things up.
            lock (_DatasetLock)
            {
                KillSaveTimer();
                SaveDataset();
            }
        }

        /// <summary>
        /// Prevents the save timer from firing any more and then kills it.
        /// </summary>
        void KillSaveTimer()
        {
            lock (_DatasetLock)
            {
                if (null != _SaveTimer)
                {
                    _SaveTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _SaveTimer.Dispose();
                    _SaveTimer = null;
                }
            }
        }

        /// <summary>
        /// Saves the dataset to disk if not an in-memory only data model.  Triggers
        /// </summary>
        void SaveDataset()
        {
            lock (_DatasetLock)
            {
                if (!_InMemoryOnly)
                    _RawData.WriteXml(_FullFilePath);

                if (null != _SaveCompleteEvent)
                    _SaveCompleteEvent.Set();
            }
        }

        /// <summary>
        /// If there are pending chnages to save to disk, this forces the timer thread to trigger right now and save
        /// changes before the function returns.
        /// </summary>
        void ForceSaveToDiskRightNow()
        {
            lock (_DatasetLock)
            {
                if (null != _SaveTimer)
                {
                    using (_SaveCompleteEvent = new ManualResetEvent(false))
                    {
                        _SaveTimer.Change(0, Timeout.Infinite);
                        _SaveCompleteEvent.WaitOne();
                    }
                }
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            lock (_DatasetLock)
            {
                ForceSaveToDiskRightNow();
                if (null != _RawData)
                    _RawData.Dispose();
            }
        }
        #endregion
    }
}
