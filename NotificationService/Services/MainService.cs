using System;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Grpc.Core;
using NotificationService.Models;
using NotificationService.Protos;
using Notification = NotificationService.Protos.Notification;

namespace NotificationService.Services
{
    public class MainService : Notification.NotificationBase
    {
        private readonly UserTokensService _userTokensService;

        public MainService(UserTokensService userTokensService)
        {
            _userTokensService = userTokensService;
        }
        /**
         * Useful to send notifications to users, chats, calls, etc
         */
        public override async Task<UserIdNotificationResponse> SendNotificationByUserId(
            UserIdNotificationRequest request, ServerCallContext context)
        {
            // send a notification with the settings
            await FirebaseMessaging.DefaultInstance.SendAsync(new Message
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Body = request.Message,
                    Title = request.Title,
                },
                // get the token from the database
                Token = (await _userTokensService.GetUserTokenByIdAsync(request.Userid)).Token,
            });
            return new UserIdNotificationResponse
            {
                ErrorMessage = "",
                IsSuccessful = true
            };
        }
        
        /**
         * Firebase has the tendency to change user notification tokens every once in a while, this unary call is critical to update it
         * on our backend.
         */
        public override async Task<UpdateUserTokenResponse> UpdateUserToken(UpdateUserTokenRequest request,
            ServerCallContext context)
        {
            await _userTokensService.UpsertAsync(new UserToken
            {
                Id = request.Userid,
                Token = request.Token
            });
            return new UpdateUserTokenResponse
            {
                ErrorMessage = "",
                IsSuccessful = true
            };
        }
        /**
         * Good for announcement notifications
         */
        public override async Task<TopicNotificationResponse> SendNotificationByTopic(TopicNotificationRequest request,
            ServerCallContext context)
        {
            // send a notification with the settings
            await FirebaseMessaging.DefaultInstance.SendAsync(new Message
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Body = request.Message,
                    Title = request.Title,
                },
                Topic = request.Topic
            });
            return new TopicNotificationResponse
            {
                ErrorMessage = "",
                IsSuccessful = true
            };
        }
        
        /**
         * In case someone forgot to take their meds and wants to send a token request using our service as a middleman
         */
        public override async Task<TokenNotificationResponse> SendNotificationByToken(TokenNotificationRequest request,
            ServerCallContext context)
        {
            // send a notification with the settings
            await FirebaseMessaging.DefaultInstance.SendAsync(new Message
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Body = request.Message,
                    Title = request.Title,
                },
                // get the token from the database
                Token = request.Token
            });
            return new TokenNotificationResponse
            {
                ErrorMessage = "",
                IsSuccessful = true
            };
        }
    }
}