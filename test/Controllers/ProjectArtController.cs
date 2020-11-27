using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using test.Models;

namespace test.Controllers
{
    public class ProjectArtController : Controller
    {
        private AuthService _authService;

        //Получаем сервис аунтификации через Dependency Injection
        public ProjectArtController(AuthService authService)
        {
            _authService = authService;
        }

        //Основная страница
        [Route("")]
        public IActionResult Main()
        {
            var model = new MainViewModel();
            //В TopFilms Берем N лучших по рейтингу фильмов
            model.TopFilms = TestDataProvider.Films.Values.OrderByDescending(film => film.Rating).Take(6);
            //В TodayFilms берем фильмы, у которых есть хотя бы 1 сеанс сегодня
            model.TodayFilms = TestDataProvider.Films.Values.Where(film =>
                film.Schedule.Any(schedule => schedule.Time.Date == DateTime.Now.Date));
            return View(model);
        }
        
        //Страница фильма
        //id - Id фильма
        [Route("film/{id}")]
        public IActionResult Film(int id)
        {
            //Это нужно, чтобы смотреть расписание фильма на разные дни
            //Информация о выбранном дне сохраняется в HttpContext.Items["CurrentDay"]
            //По умолчанию равен дате сегодня
            if (Request.Query.ContainsKey("schedule_date"))
                HttpContext.Items["CurrentDay"] = DateTime.ParseExact(
                    Request.Query["schedule_date"],
                    "dd.MM.yyyy", CultureInfo.InvariantCulture);
            else HttpContext.Items["CurrentDay"] = DateTime.Now;
            
            return View(TestDataProvider.Films[id]);
        }
        
        //Страница фильмов, page - это номер страницы начиная с одного
        //Тут еще будут приниматься параметры поиска
        [Route("films")]
        public IActionResult Films(int page = 1)
        {
            var films = TestDataProvider.Films.Values;
            var filmsPerPage = 10;
            var totalPages = films.Count / filmsPerPage + (films.Count % filmsPerPage > 0 ? 1 : 0);
            var viewModel = new FilmsViewModel()
            {
                Films = films.Skip(filmsPerPage * (page - 1)).Take(filmsPerPage),
                CurrentPage = page,
                NumberOfPages = totalPages
            };
            return View(viewModel);
        }
        
        //Страница расписания
        [Route("schedule")]
        public IActionResult Schedule()
        {
            //Тоже самое что и на странице фильмов с расписанием
            var date = DateTime.Now;
            if (Request.Query.ContainsKey("schedule_date"))
                date = DateTime.ParseExact(
                    Request.Query["schedule_date"],
                    "dd.MM.yyyy", CultureInfo.InvariantCulture);
            HttpContext.Items["CurrentDay"] = date;
            
            //Передаем во View только те фильмы, которые имеют хотя бы один сеанс на выбранную дату date
            return View(TestDataProvider.Films.Values.Where(film =>
                film.Schedule.Any(schedule => schedule.Time.Date == date)));
        }
        
        //Страница бронирования мест
        //Id - это id сеанса(Schedule)
        [Route("booking/{id}")]
        public IActionResult Booking(int id)
        {
            //Не авторизован - на главную станицу
            if (!_authService.IsAuthenticated)
                return Redirect(Url.Content("~/"));
            
            var model = new BookingPageViewModel();
            //Передаем сеанс с указаным id
            model.Schedule = TestDataProvider.Schedules[id];
            //Передаем все бронирования на этот сеанс
            model.BookingsInSchedule = TestDataProvider.Bookings.Values
                .Where(booking => booking.Schedule.Id == id);
            return View(model);
        }
        
        //Action на ajax запрос бронирования мест
        //id - Id сеанса
        [Route("booking/{id}/book_seats")]
        public async Task<IActionResult> BookSeats(int id)
        {
            if (!_authService.IsAuthenticated)
                return BadRequest(); //Status code 400

            //Данные приходят как массив индексов мест в формате json
            var json = "";
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                json = await reader.ReadToEndAsync();
            }

            dynamic result = JsonConvert.DeserializeObject(json);

            var schedule = TestDataProvider.Schedules[id];
            //Создаем хэш-таблицу(словарь) уже забронированных мест на этот сеанс
            var unavailableSeats = TestDataProvider.Bookings.Values
                    .Where(booking => booking.Schedule.Id == id)
                    .ToDictionary(booking => booking.Seat);

            var bookedSeats = new List<int>();
            
            //Кэшируем места, которые хочет забронировать пользователь и проверяем
            //Не занято ли оно уже
            foreach (int index in result)
            {
                if (unavailableSeats.ContainsKey(index))
                    return BadRequest(); //Status code 400

                bookedSeats.Add(index);
            }

            var userId = TestDataProvider.GetUserId(_authService.Name);

            //Если места не заняты, добавляем информацию о бронировании и привязывем ее к текущему пользователю
            foreach (var seat in bookedSeats)
            {
                var booking = new BookingViewModel();
                booking.Schedule = schedule;
                booking.Seat = seat;
                booking.BookingCode = "123412341234";
                TestDataProvider.AddBooking(userId, booking);
            }

            return Ok(); //Status code 200
        }
        
        //Информация о пользователе
        [Route("user/{id?}")]
        public IActionResult User(int? id)
        {
            //Если id пользователя не указан, то есть страница была вызвана по адресу ~/user(без /{id})
            //То нужно отобразить страницу текущего пользователя, если он авторизован
            if(id == null)
                if (_authService.IsAuthenticated)
                    return View(TestDataProvider.Users.Values.Single(user =>
                        user.NickName == _authService.Name));
                else //Если id не указано и пользователь еще не вошел в систему, кидаем на главную страницу
                    return Redirect(Url.Content("~/"));
            
            //Если id указан, показываем страницу пользователя с данным id
            return View(TestDataProvider.Users[id.Value]);
        }
        
        //Редактирование профиля
        [HttpGet]
        [Route("user/{id}/edit")]
        public IActionResult UserEdit(int id)
        {
            var user = TestDataProvider.Users[id];
            if (_authService.IsAuthenticated
                && _authService.Name == user.NickName)
            {
                ViewData["login_unavailable"] = false;
                ViewData["email_unavailable"] = false;
                ViewData["validation_errors"] = false;
                
                return View(user);
            }

            //Если пользователь не авторизован или id принадлежит не ему,
            //то кидаем на страницу о пользователе
            return Redirect(Url.Content($"~/user/{id}"));
        }
        
        //Запрос на редактирование данных с формы, пришедшей с той же страницы
        [HttpPost]
        [Route("user/{id}/edit")]
        public IActionResult UserEdit(
            int id, 
            string nickname, 
            string name, 
            string surname,
            string email)
        {
            var user = TestDataProvider.Users[id];
            if(_authService.IsAuthenticated
               && _authService.Name == user.NickName)
            {
                var loginUnavailable = false; //Логин(никнейм) занят
                var emailUnavailable = false; //Email занят
                var isErrors = false; //Ошибки в данных(например не тот формат email и т.д)
                
                if (email != user.Email
                    && TestDataProvider.Users.Values.Any(user => user.Email == email))
                    emailUnavailable = true;
                if (nickname != user.NickName 
                    && TestDataProvider.Users.Values.Any(user => user.NickName == nickname))
                    loginUnavailable = true;
                //Validate input, if errors - set isErrors = true

                ViewData["login_unavailable"] = loginUnavailable;
                ViewData["email_unavailable"] = emailUnavailable;
                ViewData["validation_errors"] = isErrors;

                //Если есть ошибки, возвращаем на страницу редактирования
                if (loginUnavailable || emailUnavailable || isErrors)
                {
                    return View(user);
                }
                
                user.Name = name;
                user.Surname = surname;
                user.Email = email;

                user.NickName = nickname;

                //Это чтобы обновить аунтификационные cookie
                _authService.Logout();
                _authService.AuthenticateUser(nickname);
            }

            //Если все прошло хорошо или 
            //пользователь не авторизован или id принадлежит не ему,
            //то кидаем на страницу о пользователе
            return Redirect(Url.Content($"~/user/{id}"));
        }
        
        //Страница бронирований пользователя
        [Route("user/{id}/booking")]
        public IActionResult UserBooking(int id)
        {
            return View(TestDataProvider.Users[id]);
        }

        //Action на ajax запрос входа
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string password, string login, bool remember)
        {
            if (TestDataProvider.Users.Values.Any(user => user.NickName == login))
            {
                _authService.AuthenticateUser(login);
                return Ok(); //Status code 200
            }

            return BadRequest(); //Status code 400
        }

        //Запрос на добавление отзыва
        //review - текст отзыва
        //id - id фильма
        [HttpPost]
        [Route("addreview")]
        public IActionResult AddReview(int id, string review, int rating)
        {
            if (_authService.IsAuthenticated)
            {
                var user = TestDataProvider.Users.Values
                    .Single(u => u.NickName == _authService.Name);
                TestDataProvider.Films[id].Reviews = TestDataProvider.Films[id].Reviews
                    .Concat(new ReviewViewModel[]
                {
                    new ReviewViewModel()
                    {
                        Rating = rating,
                        ReviewText = review,
                        TimeOfReview = DateTime.Now,
                        User = user
                    }
                });
            }
            
            return Redirect(Url.Content($"~/film/{id}"));
        }
        
        //Страница пользовательского соглашения
        [Route("terms")]
        public IActionResult TermsOfUse()
        {
            return View();
        }
        
        //Выход из аккаунта
        [Route("exit")]
        public IActionResult Exit()
        {
            //Выходить только если авторизован
            if(_authService.IsAuthenticated)
                _authService.Logout();
            return Redirect(Url.Content("~/"));
        }
        
        //Страница регистрации
        [HttpGet]
        [Route("register")]
        public IActionResult Registration()
        {
            //Если уже авторизован - перемещаем в личный кабинет
            if (_authService.IsAuthenticated)
                return Redirect(Url.Content("~/user"));
            return View(new RegistrationViewModel()); //model with default values
        }
        
        //Запрос с формы на регистрацию
        //terms - нажал ли галочку соглашения с пользовательским соглашением
        [HttpPost]
        [Route("register")]
        public IActionResult Registration(string login, string name, string surname,
            string email, string password,
            string confirmationPassword, bool terms)
        {
            //Need to check is user authenticated
            var model = new RegistrationViewModel();
            model.Email = email;
            model.Login = login;
            model.Name = name;
            model.Surname = surname;
            //Validate email, login, name, surname, password, etc.
            //if errors set model.IsErrors = true
            if (TestDataProvider.Users.Values.Any(user => user.Email == email))
                model.IsEmailUnavailable = true;
            if (TestDataProvider.Users.Values.Any(user => user.NickName == login))
                model.IsLoginUnavailable = true;

            if (model.IsErrors || model.IsEmailUnavailable || model.IsLoginUnavailable)
                return View(model);
            else //If registration went successfully -
                //add user to db, authorize him and redirect to main page
            {
                RegisterUser(login, name, surname, email);
                _authService.AuthenticateUser(login);
                return Redirect(Url.Content("~/"));
            }
        }

        //This method is only for test purposes!
        //Здесь может быть добавление в базу данных
        public void RegisterUser(string login, string name, string surname,
            string email)
        {
            TestDataProvider.Users.Add(TestDataProvider.Users.Count,
                new UserViewModel()
                {
                    NickName = login,
                    Name = name,
                    Surname = surname,
                    Email = email,
                    Id = TestDataProvider.Users.Count,
                    ImagePath = "~/images/users/user.png",
                    Bookings = new List<BookingViewModel>()
                });
        }
    }
}