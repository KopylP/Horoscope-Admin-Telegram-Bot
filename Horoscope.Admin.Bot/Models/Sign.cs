using System.ComponentModel.DataAnnotations;

namespace Horoscope.Admin.Bot.Models;

public enum Sign
{
    None,
    
    [Display(Name = "Овен ♈")]
    Aries,

    [Display(Name = "Телець ♉")]
    Taurus,

    [Display(Name = "Близнюки ♊")]
    Gemini,

    [Display(Name = "Рак ♋")]
    Cancer,

    [Display(Name = "Лев ♌")]
    Leo,

    [Display(Name = "Діва ♍")]
    Virgo,

    [Display(Name = "Терези ♎")]
    Libra,

    [Display(Name = "Скорпіон ♏")]
    Scorpio,

    [Display(Name = "Стрілець ♐")]
    Sagittarius,

    [Display(Name = "Козеріг ♑")]
    Capricorn,

    [Display(Name = "Водолій ♒")]
    Aquarius,

    [Display(Name = "Риби ♓")]
    Pisces,
    
    [Display(Name = "Змієносець \u26ce")]
    Ophiuchus
}