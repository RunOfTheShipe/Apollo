using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Apollo.Model;

namespace Apollo.ViewModel
{
    public class CreateModifyRaceViewModel : ApolloViewModelBase
    {
        Race _Race;
        public CreateModifyRaceViewModel(ApolloModel model, Race race) : base(model)
        {
            if (null != race)
                _Race = race;
            else
                _Race = Race.MakeRace(model);
        }

        #region Notify-Update Properties
        public DateTime RaceDate
        {
            get { return _Race.RaceDate; }
            set { _Race.RaceDate = value; OnPropertyChanged("RaceDate"); }
        }

        public double DistanceMiles
        {
            get { return _Race.DistanceMiles; }
            set { _Race.DistanceMiles = value; OnPropertyChanged("DistanceMiles"); }
        }

        public bool ChipTimed
        {
            get { return _Race.ChipTimed; }
            set { _Race.ChipTimed = value; OnPropertyChanged("ChipTimed"); }
        }

        public string RaceName
        {
            get { return _Race.RaceName; }
            set { _Race.RaceName = value; OnPropertyChanged("RaceName"); }
        }
        #endregion
    }
}
