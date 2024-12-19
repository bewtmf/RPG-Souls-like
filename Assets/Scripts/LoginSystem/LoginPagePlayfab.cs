using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.SceneManagement;

namespace DS
{

    public class LoginPagePlayfab : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI TopText;
        [SerializeField] TextMeshProUGUI MessageText;

        [Header("Login")]
        [SerializeField] TMP_InputField UsernameOrEmailLoginInput;
        [SerializeField] TMP_InputField PasswordLoginInput;
        [SerializeField] GameObject LoginPage;

        [Header("Register")]
        [SerializeField] TMP_InputField UsernameRegisterInput;
        [SerializeField] TMP_InputField EmailRegisterInput;
        [SerializeField] TMP_InputField PasswordRegisterInput;
        [SerializeField] GameObject RegisterPage;

        [Header("Reset Password")]
        [SerializeField] TMP_InputField EmailRecoveryInput;
        [SerializeField] GameObject RecoveryPage;

        #region Button Functions

        public void RegisterUser()
        {
            //string password = PasswordRegisterInput.text;
            //if (!IsPasswordValid(password))
            //{
            //    MessageText.color = Color.red;
            //    MessageText.fontSize = 14;
            //    MessageText.text = "Mật khẩu phải có ít nhất 10 ký tự, bao gồm một chữ thường, một chữ viết hoa, một chữ số và một ký tự đặc biệt";
            //    return;
            //}

            var request = new RegisterPlayFabUserRequest
            {
                Username = UsernameRegisterInput.text,
                DisplayName = UsernameRegisterInput.text,
                Email = EmailRegisterInput.text,
                Password = PasswordRegisterInput.text,
                RequireBothUsernameAndEmail = true
            };
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucces, OnError);
        }

        public void Login()
        {
            string input = UsernameOrEmailLoginInput.text;
            if (input.Contains("@"))
            {
                var request = new LoginWithEmailAddressRequest
                {
                    Email = input,
                    Password = PasswordLoginInput.text,
                };
                PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
            }
            else
            {
                var usernameRequest = new LoginWithPlayFabRequest
                {
                    Username = input,
                    Password = PasswordLoginInput.text,
                };
                PlayFabClientAPI.LoginWithPlayFab(usernameRequest, OnLoginSuccess, OnError);
            }
        }

        public void Recovery()
        {
            var request = new SendAccountRecoveryEmailRequest
            {
                Email = EmailRecoveryInput.text,
                TitleId = "20A7D"
            };

            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySuccess, OnRecoveryError);
        }

        private void OnRecoveryError(PlayFabError error)
        {
            MessageText.color = Color.red;
            MessageText.text = "Không tìm thấy Email!";
        }

        private void OnRecoverySuccess(SendAccountRecoveryEmailResult result)
        {
            MessageText.color = Color.green;
            MessageText.text = "Đã gửi yêu cầu thay đổi mật khẩu!";
        }

        private void OnLoginSuccess(LoginResult result)
        {
            SceneManager.LoadScene(1);
        }

        private void OnError(PlayFabError error)
        {
            MessageText.color = Color.red;
            MessageText.text = error.ErrorMessage;
            Debug.Log(error.GenerateErrorReport());
        }

        private void OnRegisterSucces(RegisterPlayFabUserResult result)
        {
            MessageText.color = Color.green;
            MessageText.text = "Đăng ký thành công!";
            OpenLoginPage();
        }

        public void OpenLoginPage()
        {
            LoginPage.SetActive(true);
            RegisterPage.SetActive(false);
            RecoveryPage.SetActive(false);
            TopText.text = "Đăng nhập";

        }

        public void OpenRegisterPage()
        {
            LoginPage.SetActive(false);
            RegisterPage.SetActive(true);
            RecoveryPage.SetActive(false);
            TopText.text = "Đăng ký";
        }

        public void OpenRecoveryPage()
        {
            LoginPage.SetActive(false);
            RegisterPage.SetActive(false);
            RecoveryPage.SetActive(true);
            TopText.text = "Quên mật khẩu";
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length < 10)
                return false;
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]")) return false;
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]")) return false;
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"\d")) return false;
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[\W_]")) return false;

            return true;
        }

        #endregion


    }
}
