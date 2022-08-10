using Microsoft.EntityFrameworkCore;

namespace UNN_Ki_001.Data
{
    public abstract class Reloadable
    {
        public abstract void reload(KintaiDbContext context);
    }
}
