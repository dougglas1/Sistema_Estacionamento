using IBM.Data.DB2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaEstacionamento.Dao.Connections
{
    public class Conexao
    {
        const string conn = @"Server=apolo15.karsten.com.br:50000;Database=DB2DSV;UID=db2appl;PWD=password";

        public static DB2Connection Conn()
        {
            return new DB2Connection(conn);
        }
    }
}
