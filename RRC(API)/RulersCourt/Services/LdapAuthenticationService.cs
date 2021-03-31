using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Novell.Directory.Ldap;
using RulersCourt.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace RulersCourt.Services
{
    public class LdapAuthenticationService : IAuthenticationService
    {
        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SAMAccountNameAttribute = "sAMAccountName";
        private const string NameAttribute = "name";

        private readonly LdapConfig _config;
        private readonly LdapConnection _connection;
        private readonly IOptions<ConnectionSettingsModel> _appSettings;
        private readonly string domain = string.Empty;

        public LdapAuthenticationService(IOptions<LdapConfig> config, IOptions<ConnectionSettingsModel> app)
        {
            _config = config.Value;
            _appSettings = app;
            _connection = new LdapConnection
            {
                SecureSocketLayer = false
            };
            domain = config.Value.Domain + "\\";
        }

        public User Authenticate(string username, string password, WrdUserLoginCredentialsModel wrdUser)
        {
            User user = new User();
            if (username.Equals(wrdUser.UserName) && password.Equals(wrdUser.Password))
            {
                user.Username = domain + username;
                user.IsWrdUser = true;
                user.DisplayName = username;
            }
            else
            {
                user = LdapAuthenticate(username, password);
                if (user == null)
                    return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Value.AuthSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("displayName", user.DisplayName),
                    new Claim("username", user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;

            return user;
        }

        public User LdapAuthenticate(string username, string password)
        {
            _connection.Connect(_config.Url, LdapConnection.DEFAULT_PORT);
            _connection.Bind(domain + _config.BindDn, _config.BindCredentials);
            var searchFilter = string.Format("(samAccountName={0})", username);
            var result = _connection.Search(_config.SearchBase, LdapConnection.SCOPE_SUB, searchFilter, new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute, NameAttribute }, false);

            try
            {
                var user = result.next();
                if (user != null)
                {
                    string domainUser = domain + username;
                    _connection.Bind(domainUser, password);
                    if (_connection.Bound)
                    {
                        return new User
                        {
                            DisplayName = user.getAttribute(DisplayNameAttribute).StringValue,
                            ADUserName = user.getAttribute(SAMAccountNameAttribute).StringValue,
                            Username = domainUser,
                            IsAdmin = user.getAttribute(MemberOfAttribute) == null ? false : CheckIsAdmin(user.getAttribute(MemberOfAttribute).StringValueArray)
                        };
                    }
                }
            }
            catch
            {
                _connection.Disconnect();
            }

            return null;
        }

        private bool CheckIsAdmin(string[] user)
        {
                foreach (var result in user)
                {
                    string[] cnSplit = result.Split(',');

                    foreach (var attr in cnSplit)
                    {
                        var attrVal = string.Empty;
                        attrVal = attr.ToLower().Replace("cn=", string.Empty);
                        attrVal = attrVal.ToLower().Replace("dc=", string.Empty);


                        if (attrVal.Equals(_config.AdminCn.ToLower()))
                        {
                            return true;
                        }
                    }
                }

                return false;
        }
    }
}