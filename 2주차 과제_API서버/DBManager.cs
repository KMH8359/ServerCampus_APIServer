using System.Security.Cryptography;
using System.Text;
using CloudStructures;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;

public class DBManager
{
    static string? HiveDBConnectString;
    
    
    public static void Init(IConfiguration configuration)
    {
        HiveDBConnectString = configuration.GetSection("DBConnection")["MySqlHive"];
    }

    
    public static async Task<QueryFactory> GetGameDBQuery()
    {
        var connection = new MySqlConnection(HiveDBConnectString);
        await connection.OpenAsync();
        
        var compiler = new MySqlCompiler();        // SQL 쿼리를 컴파일하는 컴파일러 생성
        var queryFactory = new QueryFactory(connection, compiler);
        
        return queryFactory;
    }
    

    
}