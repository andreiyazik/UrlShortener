using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entities
{
    [Table("Urls")]
    public class Url : EntityBase
    {
        [Key]
        public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }
    }
}