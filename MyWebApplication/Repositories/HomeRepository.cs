using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApplication.Repositories
{
    public interface IHomeRepository
    {
        int Get();
    }
    public class HomeRepository: IHomeRepository
    {
        public int Get()
        {
            return 1;
        }
    }
}
