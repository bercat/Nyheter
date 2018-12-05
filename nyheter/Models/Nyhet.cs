using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Nyhet : EntityBase
    {
        [Required]
        [Display(Name = "Tittel")]
        public string Tittel { get; set; }
        [Required]
        [Display(Name = "Tekst")]
        public string Tekst { get; set; }
        public DateTime DatoPostet { get; set; }
        
        [Display(Name = "Bilde")]
        public byte[] BildeSrc { get; set; }
        public virtual Guid ForfatterId { get; set; }
    }
}
