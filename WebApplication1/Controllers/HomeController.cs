using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            SqlConnection con = new SqlConnection("Server=localhost;Database=master;Trusted_Connection=True;");
            SqlCommand cmd = new SqlCommand("select * from Person1", con);
            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();

            Task<String> c = Callasync();
            if (!c.IsCompleted)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Waiting for HTTP Back");
            }
            Console.WriteLine("Response Back");

            ViewBag.Title = "Home Page";

            Person1 person = new Person1();

            System.Diagnostics.Debug.WriteLine(person.first_name);

            return View();
        }
        public async Task<String>  Callasync()
        {
        using (var client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromSeconds(3);
            String ins = Environment.GetEnvironmentVariable("OTEL_DOTNET_AUTO_TRACES_ENABLED_INSTRUMENTATIONS") ;
            System.Diagnostics.Debug.WriteLine("ENV "+ins);

                System.Diagnostics.Debug.WriteLine("Calling");
                try
                {
                    Console.WriteLine("Calling HTTP");
                    HttpResponseMessage response = await client.GetAsync("https://www.google.com/");
                    Console.WriteLine("Success");
                    return "success";

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
                System.Diagnostics.Debug.WriteLine("Calling complete");

            }
            return "fail";
        }
    }
}
