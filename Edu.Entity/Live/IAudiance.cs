using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edu.Entity.Live
{
    public interface IAudiance
    {
        Task Show(string id);
        bool Authenticated();
        Task<IEnumerable<LiveHostShow>> GetList(int pgsz, out int ttl, string orderBy, int pg = 1);
    }
}