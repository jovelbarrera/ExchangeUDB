using Exchange.Dependencies.Facebook;
using Exchange.Models;
using Exchange.RealmServices;
using Exchange.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Exchange.Pages
{
    public partial class LoginPage : ContentPage
    {
        Label _testOutputLabel;
        Label _testExceptionsLabel;
        public LoginPage()
        {
            TestUI();
            RealmTest().ConfigureAwait(false);
            //InitializeComponents();
        }

        #region Realm Test
        async Task RealmTest()
        {
            try
            {
                AddTextToOutput("****REALM SERSICE TEST****\n\n");
                AddTextToOutput("Get all cached comments. The first time it has no objects\n");
                // Get all cached comments. The first time it has no objects
                List<Comment> cachedComments = await RealmService<Comment>.Instance.GetObjects();
                AddTextToOutput(cachedComments);

                AddTextToOutput("Insert 10 comments\n");
                // Insert 10 comments
                for (int i = 1; i <= 10; i++)
                    await RealmService<Comment>.Instance.InsertObject(new Comment { ObjectId = i.ToString(), Message = "Comentario " + i });

                AddTextToOutput("Retrive all comments againm now should be 10 comments.\n");
                // Retrive all comments againm now should be 10 comments.
                cachedComments = await RealmService<Comment>.Instance.GetObjects();
                AddTextToOutput(cachedComments);

                AddTextToOutput("Get the fourth Comment\n");
                // Get the fourth Comment
                var fourthComent = await RealmService<Comment>.Instance.GetObject("4");
                AddTextToOutput(fourthComent);
                AddTextToOutput("Use UpdateObjectTransaction for replace a part of an object, for instance lets update the Message for this Comment\n");
                // Use UpdateObjectTransaction for replace a part of an object, for instance lets update the Message for this Comment
                await RealmService<Comment>.Instance.UpdateObjectTransaction(() =>
                {
                    fourthComent.Message = "Mensaje actualizado";
                });

                AddTextToOutput("Get the fifthComent Comment\n");
                // Get the fifthComent Comment
                var fifthComent = await RealmService<Comment>.Instance.GetObject("5");
                AddTextToOutput("Use UpdateObject for replace an old object by a new object, it have to have the same objectId\n");
                // Use UpdateObject for replace an old object by a new object, it have to have the same objectId
                var randomComment = new Comment
                {
                    ObjectId = fifthComent.ObjectId,
                    Message = "Comentario random"
                };
                await RealmService<Comment>.Instance.UpdateObject(randomComment);

                // Retrive all comments again, now comments 4 and 5 were updated.
                AddTextToOutput("Retrive all comments again, now comments 4 and 5 were updated.\n");
                cachedComments = await RealmService<Comment>.Instance.GetObjects();
                AddTextToOutput(cachedComments);

                // Delete fifthComent Comment
                AddTextToOutput("Delete fifthComent Comment.\n");
                await RealmService<Comment>.Instance.RemoveObject(fifthComent.ObjectId);

                // Retrive all comments again, there's only 9 comments
                AddTextToOutput("Retrive all comments again, there's only 9 comments.\n");
                cachedComments = await RealmService<Comment>.Instance.GetObjects();
                AddTextToOutput(cachedComments);

                AddTextToOutput("RealmService throwns exceptions so you should handle, " +
                "For example if you run this example again without delete cache, an exception will rise because you will try to " +
                "add elements with the same objectId but this elements already exist.\n");
            }
            catch (Exception ex)
            {
                AddTextToExceptions("RealmService throwns exceptions so you should handle, " +
                "For example if you run this example again without delete cache, an exception will rise because you will try to " +
                "add elements with the same objectId but this elements already exist.\n\n");
                // RealmService throwns exceptions so you should handle,
                // For example if you run this example again without delete cache, an exception will rise because you will try to
                // add elements with the same objectId but this elements already exist
                var e = ex.Message;
                AddTextToExceptions(ex.Message);
            }
            return;
        }

        void AddTextToOutput(List<Comment> comments)
        {
            _testOutputLabel.Text += comments.Count + " Comentarios\n";
            foreach (var comment in comments)
                _testOutputLabel.Text += "ObjectId: " + comment.ObjectId + " Message: " + comment.Message + "\n";
        }

        void AddTextToOutput(Comment comment)
        {
            _testOutputLabel.Text += "ObjectId: " + comment.ObjectId + " Message: " + comment.Message + "\n";
        }

        void AddTextToOutput(string text)
        {
            _testOutputLabel.Text += text;
        }

        void AddTextToExceptions(string text)
        {
            _testExceptionsLabel.Text += text;
        }

        private void TestUI()
        {
            _testOutputLabel = new Label
            {
                TextColor = Color.Green,
            };
            _testExceptionsLabel = new Label
            {
                TextColor = Color.Red,
            };
            var nextButton = new Button
            {
                Text = "Seguir a la app"
            };
            nextButton.Clicked += NextButton_Clicked;
            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    BackgroundColor = Color.Black,
                    Children = {
                        _testOutputLabel,
                        _testExceptionsLabel,
                        nextButton
                    }
                }
            };
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            InitializeComponents();
        }
        #endregion
        private void FbButton_Clicked(object sender, EventArgs e)
        {
            //((Button)sender).IsEnabled = false;
            try
            {
                var facebook = DependencyService.Get<IFacebookButton>();
                facebook.LoginWithReadPermissions(new[] { "email" }, ReadPermissionsCallback);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private async void ReadPermissionsCallback(FacebookEvent e)
        {
            if (e == null)
            {
                await DisplayAlert("Error", "No se pudo iniciar sesión con facebook, intente de nuevo", "OK");
            }
            else
            {
                if (e.GrantedPermissions.Any(i => i == "email"))
                    await UserManager.Instance.SignInWithFacebook(e.AccessToken, FacebookLoginCallback);
                else
                    await DisplayAlert("Error", "Tu correo electrónico es necesario para completar el login con facebook, por favor acepta el permiso de correo electrónico", "OK");
            }

        }

        private void FacebookLoginCallback(bool isSuccessful)
        {
            if (isSuccessful)
                App.Current.MainPage = new MainPage();
            else
                DisplayAlert("Error", "No se pudo iniciar sesión con facebook, intente de nuevo", "OK");

        }
    }
}


