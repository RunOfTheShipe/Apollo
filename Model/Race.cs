using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Apollo.Model.Data;

namespace Apollo.Model
{
    public class Race : DataRowAdapterObjectBase<RawData.RaceRow, RawData.RaceDataTable, RawData>
    {
        internal static Race MakeRace(RawData dataSet)
        {
            var raceRow = dataSet.Race.NewRaceRow();
            dataSet.Race.AddRaceRow(raceRow);
            return new Race(raceRow);
        }

        public static Race MakeRace(ApolloModel model)
        {
            return MakeRace(model._RawData);
        }
        
        internal Race(RawData.RaceRow raceRow)
            : base(raceRow)
        { }

        #region Public Properties on RawData.RaceRow
        public DateTime RaceDate
        {
            get { return _Item.RaceDate; }
            set { _Item.RaceDate = value; }
        }

        public string RaceName
        {
            get { return _Item.RaceName; }
            set { _Item.RaceName = value; }
        }

        public double DistanceMiles
        {
            get { return _Item.DistanceMiles; }
            set { _Item.DistanceMiles = value; }
        }

        public DateTime StartTime
        {
            get { return _Item.StartTime; }
            set { _Item.StartTime = value; }
        }

        public bool ChipTimed
        {
            get { return _Item.ChipTimed; }
            set { _Item.ChipTimed = value; }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
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
}
