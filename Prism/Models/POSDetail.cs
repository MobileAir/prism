using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Prism.Models
{
    public class POSDetail
    {
        [Key, ForeignKey("Cart")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CartID { get; set; }

        [Required]
        public string POScode { get; set; }

        [Required]
        public virtual Cart Cart { get; set; }
    }
}