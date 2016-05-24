using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlusUltra.MefResolver
{
    public interface IComponent
    {
        void Setup(IRegisterComponent registerComponent);
    }
}
