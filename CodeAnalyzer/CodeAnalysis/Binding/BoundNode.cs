using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static rs.CodeAnalysis.Binding.BoundBinaryExpression;

namespace rs.CodeAnalysis.Binding
{
    internal abstract class BoundNode
    {
        public abstract  BoundNodeType BoundNodeType { get;}
    }
}


