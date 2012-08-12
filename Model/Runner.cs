using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Apollo.Model.Data;

namespace Apollo.Model
{
    public enum Gender : int
    {
        Unknown = 'U',
        Male = 'M',
        Female = 'F'
    }

    public class Runner : DataRowAdapterObjectBase<RawData.RunnerRow, RawData.RunnerDataTable, RawData>
    {
        public static Runner MakeRunner(RawData dataSet)
        {
            var runnerRow = dataSet.Runner.NewRunnerRow();
            dataSet.Runner.AddRunnerRow(runnerRow);

            return new Runner(runnerRow);
        }

        internal Runner(RawData.RunnerRow runnerRow)
            : base(runnerRow)
        { }

        #region Public properites on RawData.RunnerRow
        public string Name
        {
            get { return _Item.Name; }
            set { _Item.Name = value; }
        }

        public DateTime Birthdate
        {
            get { return _Item.Birthdate; }
            set { _Item.Birthdate = value; }
        }

        public int WeightLbs
        {
            get { return _Item.WeightLbs; }
            set { _Item.WeightLbs = value; }
        }

        public Gender Gender
        {
            get { return (Gender)_Item.Gender; }
            set { _Item.Gender = (int)value; }
        }
        #endregion

        public IEnumerable<Result> Results
        {
            get
            {
                return from resultRow in DataSet.Result
                       where resultRow.RunnerRow.ID == _Item.ID
                       select new Result(resultRow);
            }
        }
    }
}
