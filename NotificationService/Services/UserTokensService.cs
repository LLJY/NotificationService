using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using NotificationService.Models;
using UserService.Protos;

namespace NotificationService.Services
{
    /**
     * Service that communicates with mongodb
     * NOT a gRPC service
     */
    public class UserTokensService
    {
        private readonly IMongoCollection<UserToken> _userTokens;

        public UserTokensService(IUserTokensDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _userTokens = database.GetCollection<UserToken>(settings.CollectionName);
        }
        
        /**
         * Adds a new token
         */
        public async Task<UserToken> CreateAsync(UserToken token)
        {
            await _userTokens.InsertOneAsync(token);
            return token;
        }
        
        /**
         * Get all the user tokens regardless
         */
        public async Task<List<UserToken>> GetAllUserTokensAsync()
        {
            return await (await _userTokens.FindAsync(token=>true)).ToListAsync();
        }
        
        /**
         * Gets the user token by id
         */
        public async Task<UserToken> GetUserTokenByIdAsync(string id)
        {
            return await (await _userTokens.FindAsync(token => token.Id == id)).FirstOrDefaultAsync();
        }
        
        /**
         * Upserts item in database that matches the id with the object
         * *Upsert = insert if not exist, update if exists
         * <param name="token">The token key value pair to upsert</param>
         */
        public async Task UpsertAsync(UserToken token)
        {
            await _userTokens.ReplaceOneAsync(userToken => userToken.Id == token.Id, token, new ReplaceOptions
            {
                IsUpsert = true
            });
        }
    }
}