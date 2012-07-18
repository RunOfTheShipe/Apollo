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

    public class Runner : BaseDataRowAdapterObject<RawData.RunnerRow, RawData.RunnerDataTable, RawData>
    {
        public Runner(RawData.RunnerRow runnerRow) : base(runnerRow)
        {}

        #region Properites on DataRow
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

        public static Runner MakeRunner(RawData dataSet)
        {
            var runnerRow = dataSet.Runner.NewRunnerRow();
            dataSet.Runner.AddRunnerRow(runnerRow);
            dataSet.Runner.AcceptChanges();

            return new Runner(runnerRow);
        }

        public static Runner MakeRunner(RawData dataSet, string name, DateTime birthdate, int? weightLbs, Gender gender)
        {
            var runnerRow = dataSet.Runner.NewRunnerRow();
            runnerRow.Name = name;
            runnerRow.Birthdate = birthdate;
            runnerRow.WeightLbs = (weightLbs.HasValue) ? weightLbs.Value : 0;
            runnerRow.Gender = (int)gender;

            dataSet.Runner.AddRunnerRow(runnerRow);
            dataSet.Runner.AcceptChanges();

            return new Runner(runnerRow);
        }

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

    public class Race : BaseDataRowAdapterObject<RawData.RaceRow, RawData.RaceDataTable, RawData>
    {
        public Race(RawData.RaceRow raceRow) : base(raceRow)
        {}

        public IEnumerable<Result> Results
        {
            get
            {
                return from resultRow in DataSet.Result
                       where resultRow.RaceRow.ID == _Item.ID
                       select new Result(resultRow);
            }
        }
    }

    public class Result : BaseDataRowAdapterObject<RawData.ResultRow, RawData.ResultDataTable, RawData>
    {
        public Result(RawData.ResultRow resultRow) : base(resultRow)
        {}

        private Race _Race = null;
        public Race Race
        {
            get { return _Race ?? new Race(_Item.RaceRow); }
        }

        private Runner _Runner = null;
        public Runner Runner
        {
            get { return _Runner ?? new Runner(_Item.RunnerRow); }
        }
    }
}
