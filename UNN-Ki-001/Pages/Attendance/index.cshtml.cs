using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace UNN_Ki_001.Pages.Attendance
{
    [Authorize(Policy = "Rookie")]
    public class IndexModel : PageModel
    {
        public string? Date { get; private set; }

        public void OnGet()
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=Kintai;User Id=postgres;Password=123");
                conn.Open(); 
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select * from m_user";
                NpgsqlDataAdapter nda = new NpgsqlDataAdapter();
                DataTable dt = new DataTable();
                nda.Fill(dt);
                comm.Dispose();
                conn.Close();
                Date = "Successfully";
            }
            catch (Exception)
            {
                Date = "fail";
            }
        }
    }
}
