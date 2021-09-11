using System;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Application.Administration.ViewModels
{
    public class UnknownNewsSourceGetViewModel
    {
        public Guid Id { get; set; }

        public string Uri { get; set; }

        public string RequestedAt { get; set; }

        public int RequestCount { get; set; }

        public string Authenticity { get; set; }

        public string IdentifiedAt { get; set; }
    }
}