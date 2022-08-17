using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data
{
    public abstract class Reloadable
    {
        protected abstract void Reload(KintaiDbContext context);

        [NotMapped]
        public bool isReloaded { get; private set; }

        public Reloadable()
        {
            isReloaded = false;
        }

        public void run(KintaiDbContext context)
        {
            Reload(context);
            isReloaded = true;
        }
    }
}
