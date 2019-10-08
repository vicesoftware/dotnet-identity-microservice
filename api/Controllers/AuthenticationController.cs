﻿using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private const string _clientId = "11h81m4rsvbo8vo0ih2mdnom1";
        private readonly RegionEndpoint _region = RegionEndpoint.APSouth1;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public class User
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }


        [HttpPost]
        [Route("api/register")]
        public async Task<ActionResult<string>> Register(User user)
        {
            var cognito = new AmazonCognitoIdentityProviderClient(_region);

            var request = new SignUpRequest
            {
                ClientId = _clientId,
                Password = user.Password,
                Username = user.Username
            };

            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = user.Email
            };
            request.UserAttributes.Add(emailAttribute);

            var response = await cognito.SignUpAsync(request);

            return Ok();    
        }

        [HttpPost]
        [Route("api/signin")]
        public async Task<ActionResult<string>> SignIn(User user)
        {
            var cognito = new AmazonCognitoIdentityProviderClient( _region);

            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = "ap-south-1_QHqLZl116",
                ClientId = _clientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            };

            request.AuthParameters.Add("USERNAME", user.Username);
            request.AuthParameters.Add("PASSWORD", user.Password);

            var response = await cognito.AdminInitiateAuthAsync(request);

            return Ok(response.AuthenticationResult.IdToken);
        }
    }
}