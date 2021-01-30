using System;
using SQLite;

namespace CriThink.Client.Core.Models.Entities
{
    public class LatestNewsCheck : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NewsLink { get; set; }

        public string Classification { get; set; }

        public DateTime SearchDateTime { get; set; }

        public string NewsImageLink { get; set; }
    }
}