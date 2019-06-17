using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SqlInjection.Controllers
{
    public class ContaController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string senha)
        {
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProdutosDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //var consulta = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = '" + usuario + "' AND Senha = '" + senha + "'";
            var consulta = "SELECT Id FROM Usuarios WHERE Usuario = @usuario AND Senha = @senha;";

            try
            {
                using (var conexao = new SqlConnection(connectionString))
                {
                    conexao.Open();

                    using (SqlCommand comando = new SqlCommand(consulta, conexao))
                    {

                        comando.Parameters.Add(new SqlParameter("@usuario", usuario));
                        comando.Parameters.Add(new SqlParameter("@senha", senha));

                        var resultado = (int)comando.ExecuteScalar();
                        if (resultado > 0)
                        { 
                            var cookie = new HttpCookie("idUsuario", resultado.ToString());
                            Response.Cookies.Add(cookie);
                            ViewBag.Mensagem = "Login efetuado com sucesso";
                        }
                        else
                            ViewBag.Mensagem = "Falha no login";
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Mensagem = "Erro: " + e.Message;
            }

            return View();
        }
    }
}