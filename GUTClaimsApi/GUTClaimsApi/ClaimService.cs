using GUTClaimsApi.Models;
using Microsoft.Data.SqlClient;

namespace GUTClaimsApi.Services
{
    public class ClaimService
    {
        private readonly DbConnectionHelper _db;
        public ClaimService(DbConnectionHelper db) => _db = db;

        public async Task<int> SubmitClaimAsync(Claim claim)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var cmd = new SqlCommand(@"INSERT INTO Claim(UserId, ProgrammeId, MonthYear, TotalAmount, Remarks)
                                       VALUES(@u,@p,@m,@t,@r);
                                       SELECT SCOPE_IDENTITY();", conn);
            cmd.Parameters.AddWithValue("@u", claim.UserId);
            cmd.Parameters.AddWithValue("@p", claim.ProgrammeId);
            cmd.Parameters.AddWithValue("@m", claim.MonthYear);
            cmd.Parameters.AddWithValue("@t", claim.TotalAmount);
            cmd.Parameters.AddWithValue("@r", claim.Remarks ?? (object)DBNull.Value);
            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return id;
        }

        public async Task<List<Claim>> GetClaimsByUserAsync(int userId)
        {
            var list = new List<Claim>();
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT * FROM Claim WHERE UserId=@id", conn);
            cmd.Parameters.AddWithValue("@id", userId);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Claim
                {
                    ClaimId = (int)reader["ClaimId"],
                    UserId = (int)reader["UserId"],
                    ProgrammeId = (int)reader["ProgrammeId"],
                    MonthYear = reader["MonthYear"].ToString(),
                    TotalAmount = (decimal)reader["TotalAmount"],
                    CurrentStatus = reader["CurrentStatus"].ToString(),
                    SubmittedAt = (DateTimeOffset)reader["SubmittedAt"],
                    Remarks = reader["Remarks"].ToString()
                });
            }
            return list;
        }

        public async Task UpdateStatusAsync(int claimId, string status, int changedBy, string? comment)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var tran = conn.BeginTransaction();

            var cmd1 = new SqlCommand("UPDATE Claim SET CurrentStatus=@s WHERE ClaimId=@id", conn, tran);
            cmd1.Parameters.AddWithValue("@s", status);
            cmd1.Parameters.AddWithValue("@id", claimId);
            await cmd1.ExecuteNonQueryAsync();

            var cmd2 = new SqlCommand(@"INSERT INTO ClaimStatusHistory(ClaimId, Status, ChangedBy, Comment)
                                        VALUES(@c,@s,@b,@cm)", conn, tran);
            cmd2.Parameters.AddWithValue("@c", claimId);
            cmd2.Parameters.AddWithValue("@s", status);
            cmd2.Parameters.AddWithValue("@b", changedBy);
            cmd2.Parameters.AddWithValue("@cm", comment ?? (object)DBNull.Value);
            await cmd2.ExecuteNonQueryAsync();

            await tran.CommitAsync();
        }
    }
}