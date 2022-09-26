using System;
using System.Collections.Generic;

namespace ChallengeBP.Entities.Models
{
    public partial class Funding
    {
        public Funding()
        {
            Fundingcompositions = new HashSet<Fundingcomposition>();
            Fundingsharevalues = new HashSet<Fundingsharevalue>();
            Goaltransactionfundings = new HashSet<Goaltransactionfunding>();
            Goaltransactions = new HashSet<Goaltransaction>();
            Portfoliofundings = new HashSet<Portfoliofunding>();
        }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Uuid { get; set; }
        public int Id { get; set; }
        public int Financialentityid { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string? Mnemonic { get; set; }
        public bool Isbox { get; set; }
        public string? Yahoomnemonic { get; set; }
        public string? Cmfurl { get; set; }
        public int? Currencyid { get; set; }
        public bool Hassharevalue { get; set; }

        public virtual Currency? Currency { get; set; }
        public virtual Financialentity Financialentity { get; set; } = null!;
        public virtual ICollection<Fundingcomposition> Fundingcompositions { get; set; }
        public virtual ICollection<Fundingsharevalue> Fundingsharevalues { get; set; }
        public virtual ICollection<Goaltransactionfunding> Goaltransactionfundings { get; set; }
        public virtual ICollection<Goaltransaction> Goaltransactions { get; set; }
        public virtual ICollection<Portfoliofunding> Portfoliofundings { get; set; }
    }
}
