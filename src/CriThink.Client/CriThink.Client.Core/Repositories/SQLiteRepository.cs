using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.Entities;
using Microsoft.Extensions.Logging;
using SQLite;

namespace CriThink.Client.Core.Repositories
{
    public class SQLiteRepository : ISQLiteRepository
    {
        private readonly string _databasePath;
        private readonly ILogger<SQLiteRepository> _logger;

        public SQLiteRepository(ILogger<SQLiteRepository> logger)
        {
            _databasePath = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "crithink_db.db3");
            _logger = logger;
        }

        public async ValueTask<IEnumerable<LatestNewsCheck>> GetLatestNewsChecks()
        {
            try
            {
                var db = OpenSqLiteConnection();
                await db.CreateTableAsync<LatestNewsCheck>().ConfigureAwait(false);

                var query = $@"SELECT * 
                            FROM {nameof(LatestNewsCheck)}
                            ORDER BY {nameof(LatestNewsCheck.SearchDateTime)} DESC
                            LIMIT 5;";

                return await db.QueryAsync<LatestNewsCheck>(query)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting recent news checks from DB");
                return Array.Empty<LatestNewsCheck>();
            }
        }

        public async ValueTask AddLatestNewsCheck(LatestNewsCheck latesNewsCheck)
        {
            if (latesNewsCheck == null)
                throw new ArgumentNullException(nameof(latesNewsCheck));

            try
            {
                var db = OpenSqLiteConnection();
                await db.CreateTableAsync<LatestNewsCheck>().ConfigureAwait(false);

                var command = $@"insert into {nameof(LatestNewsCheck)}
                                ({nameof(LatestNewsCheck.NewsLink)}, {nameof(LatestNewsCheck.Classification)}, {nameof(LatestNewsCheck.SearchDateTime)}, {nameof(LatestNewsCheck.NewsImageLink)})
                                values('{latesNewsCheck.NewsLink}', '{latesNewsCheck.Classification}', '{latesNewsCheck.SearchDateTime}', '{latesNewsCheck.NewsImageLink}');";

                await db.ExecuteAsync(command).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error adding a recent news to DB", latesNewsCheck);
            }
        }

        private SQLiteAsyncConnection OpenSqLiteConnection()
        {
            return new SQLiteAsyncConnection(_databasePath);
        }

        public async ValueTask RemoveLatestNewsCheck(string link)
        {
            if (link == null)
                throw new ArgumentNullException(nameof(link));

            try
            {
                var db = OpenSqLiteConnection();
                await db.CreateTableAsync<LatestNewsCheck>().ConfigureAwait(false);

                var command = $@"delete from {nameof(LatestNewsCheck)}
                                 where {nameof(LatestNewsCheck.NewsLink)} = {link} ;";

                await db.ExecuteAsync(command).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error remove recent news to DB", link);
            }
        }
    }

    public interface ISQLiteRepository
    {
        ValueTask<IEnumerable<LatestNewsCheck>> GetLatestNewsChecks();

        ValueTask AddLatestNewsCheck(LatestNewsCheck latesNewsCheck);

        ValueTask RemoveLatestNewsCheck(string link);
    }
}
