using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace DockerWebApp
{
    public partial class _Default : Page
    {
        private readonly ILogger<_Default> _logger;
        private string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        // Constructor to initialize logger
        public _Default()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole(); // Logs to Docker console
            });

            _logger = loggerFactory.CreateLogger<_Default>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _logger.LogInformation("Page_Load triggered.");
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(strcon))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT TOP 5 ID, FirstName, LastName, Username, Email FROM Users";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    GridViewUsers.DataSource = dt;
                    GridViewUsers.DataBind();
                }
                catch (Exception ex)
                {
                    // Handle the error (optional: log it or display a message)
                    Response.Write("<script>alert('Error loading users: " + ex.Message + "');</script>");
                }
            }
        }

        protected void btnHello_Click(object sender, EventArgs e)
        {
            // Display an alert in the browser
            // ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Hello, World!');", true);
            //string connectionString = WebConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
            string message = "No user found"; // Default message if no user is in the database
            string id = "";
            string given_name = "";
            string email = "";

            _logger.LogInformation($"LOGGER: SQL Connection String: {strcon}"); // Use Debug.WriteLine instead of Trace.WriteLine
            using (SqlConnection con = new SqlConnection(strcon))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 id, given_name, email FROM T_JP_User", con);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Check if a row exists
                        {
                            id = reader["id"].ToString();
                            given_name = reader["given_name"].ToString();
                            email = reader["email"].ToString();
                        }
                    }
                    message = "First User: " + id + " " + given_name + " " + email;
                }
                catch (Exception ex)
                {
                    message = "Error: " + ex.Message;
                    _logger.LogError($"LOGGER ERROR: {ex}"); // Use Debug.WriteLine instead of Trace.WriteLine
                }
            }

            // Show alert with the user's data
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + strcon + message + "');", true);
        }
    }
}