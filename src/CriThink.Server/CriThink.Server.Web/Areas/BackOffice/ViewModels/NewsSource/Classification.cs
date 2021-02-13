using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public enum Classification
    {
        Reliable,
        Satirical,
        Conspiracist,
        [Display(Name = "Fake News")]
        FakeNews,
        Unknown,
    }
}
