using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NRLAdmin.Models
{
    public class PlayersMetadata
    {
        [DisplayFormat(DataFormatString = "{0:d}")]
        public string DOB;
    }
    public class GamesMetadata
    {
        [DisplayFormat(DataFormatString = "{0:d}")]
        public string GameDate;
    }
}