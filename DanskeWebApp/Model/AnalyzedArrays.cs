using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DanskeWebApp.Model
{
    [Table("AnalyzedArrays")]
    public class AnalyzedArrays
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName="varchar(MAX)")]
        public string ArrayComposition { get; set; }

        public int IsReachable { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string OptimalPath { get; set; }
    }
}
