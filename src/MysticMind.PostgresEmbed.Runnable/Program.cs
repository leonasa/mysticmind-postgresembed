using System;

namespace MysticMind.PostgresEmbed.Runnable
{
    class Program
    {
        static void Main(string[] args)
        {
            const string PG_USER = "postgres";
            const string CONN_STR = "Server=localhost;Port={0};User Id={1};Password=test;Database=postgres;Pooling=false";

            using (var server = new MysticMind.PostgresEmbed.PgServer(
                "9.5.5.1",
                PG_USER,
                addLocalUserAccessPermission: true,
                clearInstanceDirOnStop: true))
            {
                server.Start();

                // Note: set pooling to false to prevent connecting issues
                // https://github.com/npgsql/npgsql/issues/939
                string connStr = string.Format(CONN_STR, server.PgPort, PG_USER);
                var conn = new Npgsql.NpgsqlConnection(connStr);
                var cmd =
                    new Npgsql.NpgsqlCommand(
                        "CREATE TABLE table1(ID CHAR(256) CONSTRAINT id PRIMARY KEY, Title CHAR)",
                        conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
