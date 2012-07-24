using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Apollo.Model.Data;

namespace Apollo.Model
{
    public class Result : BaseDataRowAdapterObject<RawData.ResultRow, RawData.ResultDataTable, RawData>
    {
        public static Result MakeResult(RawData dataSet)
        {
            var row = dataSet.Result.NewResultRow();
            dataSet.Result.AddResultRow(row);
            return new Result(row);
        }

        internal Result(RawData.ResultRow resultRow) : base(resultRow)
        {}

        #region Public properties on RowData.ResultRow

        public string Bib
        {
            get { return _Item.Bib; }
            set { _Item.Bib = value; }
        }

        public DateTime ChipStart
        {
            get { return _Item.ChipStart; }
            set { _Item.ChipStart = value; }
        }

        public DateTime FinishTime
        {
            get { return _Item.FinishTime; }
            set { _Item.FinishTime = value; }
        }

        #endregion

        #region References to Other Tables
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
        #endregion

        #region Calculated Fields

        public TimeSpan ChipDuration
        {
            get { return FinishTime - ChipStart; }
        }

        public TimeSpan GunDuration
        {
            get { return FinishTime - Race.StartTime; }
        }

        public double ChipSpeedMph
        {
            get { return Race.DistanceMiles / ChipDuration.TotalHours; }
        }

        public double GunSpeedMph
        {
            get { return Race.DistanceMiles / GunDuration.TotalHours; }
        }

        public TimeSpan ChipPaceMinutesPerMile
        {
            get { return TimeSpan.FromMinutes(ChipDuration.TotalMinutes / Race.DistanceMiles); }
        }

        public TimeSpan GunPaceMinutesPerMile
        {
            get { return TimeSpan.FromMinutes(GunDuration.TotalMinutes / Race.DistanceMiles); }
        }

        public int RunnerAge
        {
            get
            {
                // The last term compensates if the runner has not had a birthday yet this year.
                return Race.RaceDate.Year - Runner.Birthdate.Year - (Runner.Birthdate.DayOfYear < Race.RaceDate.DayOfYear ? 0 : 1);
            }
        }

        public int CaloriesBurned
        {
            get { return CalculateCalories(); }
        }

        #endregion

        /// <summary>
        /// Calculate a rough count of calories burned based on the distance of the race and runner.
        /// </summary>
        /// <remarks>
        /// Found this calculation on Runner's World.  0.63 * weightLbs == net calories burned while running per mile
        /// (0.75 is used for total calories burned, including metabolism; net is just calories burned due to running).
        /// 
        /// Default body weights were found by googling "default body weights."
        /// </remarks>
        /// <returns>Estimate of calories burned for the runner and race associated with this result.</returns>
        int CalculateCalories()
        {
            int weightLbs = Runner.WeightLbs;
            if (0 == weightLbs)
            {
                switch (Runner.Gender)
                {
                    case Gender.Male:
                        weightLbs = 185;
                        break;
                    case Gender.Female:
                    default:
                        weightLbs = 155;
                        break;
                }
            }
            return (int)Math.Round(0.63 * weightLbs * Race.DistanceMiles);
        }

        #region Place Calculations
        /// <summary>
        /// Calculates the number of results associated with this race.
        /// </summary>
        /// <returns>The number of results associated with this race.</returns>
        int CalculateNumberOfResults()
        {
            return Race.Results.Count();
        }

        /// <summary>
        /// Calculates the overall place of the result.
        /// </summary>
        /// <param name="bChipDuration">TRUE to use chip duration; FALSE to use gun duration.</param>
        /// <returns>Overall place of the result.</returns>
        int CalculateOverallPlace(bool bChipDuration)
        {
            var results = from result in Race.Results
                          orderby (bChipDuration) ? result.ChipDuration : result.GunDuration ascending
                          select result;
            if (!results.Contains(this))
                throw new InvalidOperationException("This result was not found in the results for this race.");
            return results.ToList().IndexOf(this);
        }

        /// <summary>
        /// Calculates the gender group place.
        /// </summary>
        /// <param name="bChipDuration">TRUE to use chip duration; FALSE to use gun duration.</param>
        /// <returns>Gender place of the result.</returns>
        int CalculateGenderPlace(bool bChipDuration)
        {
            var results = from result in Race.Results
                          where result.Runner.Gender == this.Runner.Gender
                          orderby (bChipDuration) ? result.ChipDuration : result.GunDuration ascending
                          select result;
            if (!results.Contains(this))
                throw new InvalidOperationException("This result was not found in the gender group results for this race.");
            return results.ToList().IndexOf(this);
        }

        /// <summary>
        /// Calculates the age group place.
        /// </summary>
        /// <param name="bChipDuration">TRUE to use chip duration; FALSE to use gun duration.</param>
        /// <returns>Age group place of the result.</returns>
        int CalculateAgeGroupPlace(bool bChipDuration)
        {
            var results = from result in Race.Results
                          where result.Runner.Gender == this.Runner.Gender &&
                          result.RunnerAge / 10 == this.RunnerAge / 10  // Divide by 10 will create age groups of 10 years (e.g. 20/10=2, 29/10=2)
                          orderby (bChipDuration)? result.ChipDuration : result.GunDuration ascending
                          select result;
            if (!results.Contains(this))
                throw new InvalidOperationException("This result was not found in the age group results for this race.");
            return results.ToList().IndexOf(this);
        }
        #endregion
    }
}
