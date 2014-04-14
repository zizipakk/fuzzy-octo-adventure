using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JQGrid.Helpers
{
    /// <summary>
    /// The supported operations in where-extension
    /// </summary>
    public enum WhereOperation
    {
        [StringValue("eq")]
        Equal,
        [StringValue("ne")]
        NotEqual,
        [StringValue("cn")]
        Contains,
        [StringValue("lt")]
        LessThan,
        [StringValue("le")]
        LessThanOrEqual,
        [StringValue("gt")]
        GreaterThan,
        [StringValue("ge")]
        GreaterThanOrEqual,
        [StringValue("bw")]
        BeginsWith,
        [StringValue("bn")]
        DoesNotBeginsWith,
        [StringValue("in")]
        In,
        [StringValue("ni")]
        NotIn,
        [StringValue("ew")]
        EndsWith,
        [StringValue("bn")]
        DoesNotEndsWith,
        [StringValue("nc")]
        DoesNotContain,
    }
}