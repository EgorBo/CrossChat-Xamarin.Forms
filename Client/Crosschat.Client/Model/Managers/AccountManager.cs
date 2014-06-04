using System.Threading.Tasks;
using Crosschat.Client.Model.Contracts;
using Crosschat.Client.Model.Proxies;
using Crosschat.Server.Application.DataTransferObjects.Enums;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Requests;

namespace Crosschat.Client.Model.Managers
{
    public class AccountManager : ManagerBase
    {
        private readonly IDeviceInfo _deviceInfo;
        private readonly ProfileServiceProxy _profileServiceProxy;
        private readonly AuthenticationServiceProxy _authenticationServiceProxy;
        private readonly RegistrationServiceProxy _registrationServiceProxy;
        private readonly IStorage _storage;
        private UserDto _currentUser = null;
        private bool _deviceInfoInitialized = false;

        public AccountManager(IStorage storage,
            IDeviceInfo deviceInfo,
            ConnectionManager connectionManager,
            ProfileServiceProxy profileServiceProxy,
            RegistrationServiceProxy registrationServiceProxy,
            AuthenticationServiceProxy authenticationServiceProxy)
            : base(connectionManager)
        {
            _deviceInfo = deviceInfo;
            _profileServiceProxy = profileServiceProxy;
            _authenticationServiceProxy = authenticationServiceProxy;
            _registrationServiceProxy = registrationServiceProxy;
            _storage = storage;
        }

        //yeah, I didn't create an entity representing User and let managers expose DTO instead. I'm lazy :(
        public UserDto CurrentUser
        {
            get { return _currentUser ?? (_currentUser = _storage.Get<UserDto>()); }
            private set
            {
                _currentUser = value;
                _storage.Set(value);
            }
        }

        public bool IsRegistered
        {
            get { return _storage.Get<bool>(); }
            private set { _storage.Set(value); }
        }

        public string AccountName
        {
            get { return _storage.Get<string>(); }
            private set { _storage.Set(value); }
        }

        public string AccountPassword
        {
            get { return _storage.Get<string>(); }
            private set { _storage.Set(value); }
        }

        public async Task<AuthenticationResponseType> ValidateAccount(string name, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                return AuthenticationResponseType.InvalidNameOrPassword;

            await InitDeviceInfo();
            await ConnectionManager.ConnectAsync();
            var authResult = await _authenticationServiceProxy.Authenticate(
                new AuthenticationRequest { Huid = _deviceInfo.Huid, Name = name, Password = password });

            if (authResult.Result == AuthenticationResponseType.Success)
            {
                AccountName = name;
                AccountPassword = password;
                CurrentUser = authResult.User;
                IsRegistered = true;
            }

            return authResult.Result;
        }

        public async Task<AuthenticationResponseType> ValidateAccount()
        {
            if (!IsRegistered)
                return AuthenticationResponseType.InvalidNameOrPassword;
            return await ValidateAccount(AccountName, AccountPassword);
        }

        public async Task<RegistrationResponseType> Register(string name, string password, int age, bool sex, string country, string platform)
        {
            await InitDeviceInfo();
            await ConnectionManager.ConnectAsync();
            var result = await _registrationServiceProxy.RegisterNewPlayer(
                new RegistrationRequest
                {
                    Age = age,
                    Name = name,
                    Password = password,
                    Platform = platform,
                    Sex = sex,
                    Country = country,
                    Huid = _deviceInfo.Huid,
                    PushUri = _deviceInfo.PushUri,
                });

            if (result.Result == RegistrationResponseType.Success)
            {
                IsRegistered = true;
                CurrentUser = result.User;
                AccountName = name;
                AccountPassword = password;
            }

            return result.Result;
        }

        public async Task<bool> ChangePhoto(byte[] photoData)
        {
            var response = await _profileServiceProxy.ChangePhoto(new ChangePhotoRequest { PhotoData = photoData });
            if (response.Success)
            {
                CurrentUser.PhotoId = response.PhotoId;
            }
            return response.Success;
        }

        public async Task Deactivate()
        {
            var response = await _registrationServiceProxy.Deactivate(new DeactivationRequest());
            IsRegistered = false;
            AccountName = "";
            AccountPassword = "";
            CurrentUser = null;
        }

        private async Task InitDeviceInfo()
        {
            if (_deviceInfoInitialized)
                return;
            await _deviceInfo.InitAsync();
            _deviceInfoInitialized = true;
        }
    }
}
