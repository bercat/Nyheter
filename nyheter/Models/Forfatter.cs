using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Forfatter : EntityBase
    {
        public Forfatter()
        {
            Nyhets = new List<Nyhet>();
        }
        [Required]
        [Display(Name = "Fornavn")]
        public string Fornavn { get; set; }
        [Required]
        [Display(Name = "Etternavn")]
        public string Etternavn { get; set; }
        [Required]
        [Display(Name = "Mobil")]
        public string Mobil { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual List<Nyhet> Nyhets { get; set; }
    }
}
