using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exchange.Models;
using Exchange.Services;

namespace Exchange.Configs
{
    public class Dummy
    {
        const int delay = 1000;

        public async Task FirebaseCrudSample()
        {
            //// Create
            //Ask ask = new Ask { CreatedAt = DateTime.Now, User = Configs.Dummy.User(), Title = "¿Lorem ipsum dolor sit amet?", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..." };
            //await AskService.Instance.Create(ask);
            //await AskService.Instance.Create(ask);
            //// Take last one id
            //string response = await AskService.Instance.Create(ask);

            //// Read all
            //List<Ask> results = await AskService.Instance.Read();

            //// Read last one only
            //Ask result = await AskService.Instance.Read(response);

            //// Update last one
            //result.Description = "no more lorem ipsum";
            //result = await AskService.Instance.Update(result);

            //// Read last one only
            //result = await AskService.Instance.Read(response);

            //// Delete last one
            //bool success = await AskService.Instance.Delete(result);
        }

        public static User User()
        {
            User user = new User
            {
                ProfilePicture = "https://scontent.xx.fbcdn.net/v/t1.0-1/p100x100/12391013_1144527735592055_1550185947693024373_n.jpg?oh=a171d4d67bdd85984ce6e65be89bb8a4&oe=5879B9F4",
                DisplayName = "Roberto",
                Email = "roberto@email.com",
                FirstName = "Roberto Ernesto",
                LastName = "Jovel Barrera",
                ObjectId = "wgS36JgBi1N7PxYzYAG0SYo4mdh2",
                CreatedAt = DateTime.Parse("2016-09-11T16:35:35-06:00"),
                UpdatedAt = DateTime.Parse("2016-09-11T16:35:38-06:00"),
            };
            user.Data["University"] = "Universidad Don Bosco";
            user.Data["Career"] = "Ingeniería en Ciencias de la Computación";
            user.Data["About"] = "Love coding";
            return user;
        }

        public static async Task<List<Activity>> ActivityList()
        {
            await Task.Delay(delay);
            User user = User();
            return new List<Activity>
            {
                new Activity{Type=ActivityType.Ask ,Time=DateTime.Now, User=user, Detail="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
                new Activity{Type=ActivityType.Response ,Time=DateTime.Now, User=user, Detail="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
                new Activity{Type=ActivityType.Comment ,Time=DateTime.Now, User=user, Detail="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
            };
        }

        public static async Task<List<Notification>> NotificationList()
        {
            await Task.Delay(delay);
            User user = User();
            return new List<Notification>
            {
                new Notification{Type=NotificationType.AskActivity ,Time=DateTime.Now, User=user, Detail="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
                new Notification{Type=NotificationType.NewExchange ,Time=DateTime.Now, User=user, Detail="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
                new Notification{Type=NotificationType.Undefined ,Time=DateTime.Now, User=user, Detail="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
            };
        }

        public static async Task<List<Comment>> CommentList()
        {
            await Task.Delay(delay);
            User user = User();
            return new List<Comment>
            {
                new Comment{CreatedAt=DateTime.Now, User=user, Message="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
                new Comment{CreatedAt=DateTime.Now, User=user, Message="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
                new Comment{CreatedAt=DateTime.Now, User=user, Message="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
            };
        }

        public static async Task<List<Ask>> AskList()
        {
            await Task.Delay(delay);
            User user = User();
            return new List<Ask>
            {
                new Ask{CreatedAt=DateTime.Now, User=user, Title="¿Lorem ipsum dolor sit amet?", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
                new Ask{CreatedAt=DateTime.Now, User=user,Title="¿Lorem ipsum dolor sit amet?", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
                new Ask{CreatedAt=DateTime.Now, User=user,Title="¿Lorem ipsum dolor sit amet?", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
            };
        }

        public static async Task<List<Exchange.Models.Video>> ExchangeList()
        {
            await Task.Delay(delay);
            User user = User();
            return new List<Exchange.Models.Video>
            {
                new Exchange.Models.Video{Thumbnail="http://www.learnconline.com/wp-content/uploads/2015/08/why-learn-C-programming.jpg", CreatedAt=DateTime.Now, User=user, Title="¿Lorem ipsum dolor sit amet?", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
                new Exchange.Models.Video{Thumbnail="http://www.learnconline.com/wp-content/uploads/2015/08/why-learn-C-programming.jpg", CreatedAt=DateTime.Now, User=user,Title="¿Lorem ipsum dolor sit amet?", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
                new Exchange.Models.Video{Thumbnail="http://www.learnconline.com/wp-content/uploads/2015/08/why-learn-C-programming.jpg", CreatedAt=DateTime.Now, User=user,Title="¿Lorem ipsum dolor sit amet?", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation..."},
            };
        }
    }
}

