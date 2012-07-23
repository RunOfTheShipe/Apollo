using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Apollo.Model.Data;

namespace Apollo.Model
{
    /// <summary>
    /// Is an adapter class around a DataSet, abstracts logic around opening/closing/saving, creating accessor objects
    /// to the dataset, and querying the dataset.
    /// </summary>
    public class ApolloModel : IDisposable
    {
        /// <summary>
        /// Constructs an instance of the ApolloModel for use.  If no file path is given, will construct and in-memory
        /// model only.
        /// </summary>
        /// <param name="fullFilePath">(Optional) Reads in an initial state of the ApolloModel.</param>
        /// <returns>ApolloModel for use.</returns>
        public static ApolloModel MakeApolloModel(string fullFilePath)
        {
            var model = new ApolloModel();
            model.Initialize(fullFilePath);
            return model;
        }

        private string _FullFilePath;
        private RawData _RawData;
        private bool _InMemoryOnly;
        internal ApolloModel()
        {
            _RawData = null;
            _FullFilePath = String.Empty;
            _InMemoryOnly = true;
        }
        
        /// <summary>
        /// Initializes the underlying data model by reading in the contents of the given XML file.
        /// </summary>
        /// <param name="fullFilePath">Full file path to the XML stored dataset.</param>
        internal void Initialize(string fullFilePath)
        {
            _RawData = new RawData();
            if (!String.IsNullOrWhiteSpace(fullFilePath))
            {
                if (!File.Exists(fullFilePath))
                {
                    throw new FileNotFoundException(String.Format("The file '{0}' does not exist.", fullFilePath));
                }
                _RawData.ReadXml(fullFilePath);
                _InMemoryOnly = false;
            }

            // bms_todo: Create a thread to save the dataset every <timeperiod> if not and "in memory" dataset.
        }

        public void Dispose()
        {
            if (null != _RawData)
                _RawData.Dispose();
        }
    }
}
