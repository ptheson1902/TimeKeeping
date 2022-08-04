using System.Data;

namespace UNN_Ki_001.Pages.Attebdabce
{
    internal class NpgsqlCommand
    {
        public NpgsqlConnection Connection { get; internal set; }
        public string CommandText { get; internal set; }
        public CommandType CommandType { get; internal set; }

        internal void Dispose()
        {
            throw new NotImplementedException();
        }

        internal void ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }
    }
}