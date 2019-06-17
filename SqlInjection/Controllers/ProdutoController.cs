using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SqlInjection.Controllers
{
    public class ProdutoController : Controller
    {
        [HttpGet]
        public ActionResult List(int? id)
        {
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProdutosDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var consulta = "SELECT * FROM Produtos WHERE idUsuario = @idUsuario";

            try
            {
                using (var conexao = new SqlConnection(connectionString))
                {
                    conexao.Open();

                    using (SqlCommand comando = new SqlCommand(consulta, conexao))
                    {
                        if (id.HasValue)
                        {
                            comando.CommandText = consulta + " AND Id = @id;";
                            comando.Parameters.Add(new SqlParameter("@id", id));
                        }

                        // Cookie
                        var cookie = Request.Cookies["idUsuario"];
                        if (cookie == null)
                        {
                            ViewBag.Mensagem = "Sessão expirada";
                            return View();

                        }
                        comando.Parameters.Add(new SqlParameter("@idUsuario", cookie.Value));

                        var leitor = comando.ExecuteReader();
                        if (leitor.HasRows)
                        {
                            var produtos = new List<string>();
                            while (leitor.Read())
                            {
                                produtos.Add(leitor["Descricao"].ToString());
                            }
                            ViewBag.Produtos = produtos;
                        }
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