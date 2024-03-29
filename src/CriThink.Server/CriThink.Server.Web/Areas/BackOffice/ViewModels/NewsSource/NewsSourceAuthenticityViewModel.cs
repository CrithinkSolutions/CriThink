﻿using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public enum NewsSourceAuthenticityViewModel
    {
        Reliable,
        Satirical,
        Conspiracist,
        [Display(Name = "Fake News")]
        FakeNews,
        Suspicious,
        [Display(Name = "Social Media")]
        SocialMedia,
        Unknown,
    }
}
