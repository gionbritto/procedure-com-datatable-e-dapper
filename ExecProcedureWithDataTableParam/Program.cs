using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecProcedureWithDataTableParam
{
    /// <summary>
    /// Autor: Giovanne Britto, github: gnbritto
    /// Execução utilizando Dapper. Também é possibel SQlCommand diretamente
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ExecutarProcedurePassandoDataTableTipadaComoParametro();
            Console.ReadLine();
        }

        private static void ExecutarProcedurePassandoDataTableTipadaComoParametro()
        {
            try
            {
                var conn = new SqlConnection(@"Data Source=******* ;Initial Catalog=********;Connect Timeout=15;user id=*******;password=********;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                conn.Open();
                DataTable cargos = new DataTable();
                DataTable unidade = new DataTable();

                int[] c = { 9, 10, 11 };
                cargos.Columns.Add("Cargo");
                foreach (var item in c)
                {
                    cargos.Rows.Add(item);
                }

                DateTime data = new DateTime();
                data = DateTime.Now;

                var queryParameter = new DynamicParameters();
                queryParameter.Add("@idcargos", cargos.AsTableValuedParameter("[dbo].[tpCargos]")); //adicionando parametros dinamicos do tipo Type criado no SQl Server

                int[] u = { 496 };
                unidade.Columns.Add("Id");
                foreach (var item in c)
                {
                    unidade.Rows.Add(item);
                }

                queryParameter.Add("@dataMovimentacao", data.Date);
                queryParameter.Add("@idunidade", unidade.AsTableValuedParameter("[dbo].[tpIdUnidade]"));

                var empregados = conn.Query<EmpregadoDto>("SP_A_SER_CHAMADA", queryParameter, commandType: System.Data.CommandType.StoredProcedure); //Executando a procedure

                foreach (var item in empregados)
                {
                    Console.WriteLine("{0} - {1}", item.IdEmpregado, item.Empregado);
                }
                conn.Close();
            }
            catch (SqlException ex)
            {

                throw ex;
            }

        }
    }

    //dto
    public class EmpregadoDto
    {
        public int IdEmpregado { get; set; }
        public String Empregado { get; set; }
        public String CPF { get; set; }        
        public string Empresa { get; set; }
        public string Operacao { get; set; }
        public string Atividade { get; set; }        
        public string OrgaoEmissor { get; set; }
        public string Cargo { get; set; }
        public string TipoCargo { get; set; }
        public DateTime DataNascimento { get; set; }
        public decimal Salario { get; set; }
        public int IdCargo { get; set; }
        public int CriadoPor { get; set; }
        public int? idTipoCargo { get; set; }
        public System.DateTime DataCadastro { get; set; }       
        public int IdSituacaoEmpregado { get; set; }        
        public int? IdLiderImediato { get; set; }
        public string LiderImediato { get; set; }
        public List<EmpregadoDto> Subordinados { get; set; }     
       
    }
}
