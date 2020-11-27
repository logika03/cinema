using System;
using System.Collections.Generic;
using System.Linq;
using test.Models;

namespace test
{
    public static class TestDataProvider
    {
        public static Dictionary<int, FilmViewModel> Films =
            new Dictionary<int, FilmViewModel>();

        public static Dictionary<int, UserViewModel> Users = new Dictionary<int, UserViewModel>();

        private static int scheduleIdSequence = 0;
        public static Dictionary<int, ScheduleViewModel> Schedules = new Dictionary<int, ScheduleViewModel>();

        public static Dictionary<int, HallViewModel> Halls = new Dictionary<int, HallViewModel>();

        private static int bookingsIdSequence = 0;
        public static Dictionary<int, BookingViewModel> Bookings = new Dictionary<int, BookingViewModel>();
        public static List<string> Genres = new List<string>()
        {
            "Ужасы",
            "Комедия",
            "Драма",
            "Детектив",
            "Триллер",
            "Sci-fi"
        };

        public static int GetUserId(string login)
        {
            return Users.Values
                .Single(user => user.NickName == login).Id;
        }

        public static void AddBooking(int userId, BookingViewModel booking)
        {
            booking.Id = bookingsIdSequence++;
            var user = Users[userId];

            booking.User = user;
            user.Bookings = user.Bookings.Concat(new[] {booking});

            Bookings.Add(booking.Id, booking);
        }
        
        public static void InitializeWithRandom(int count, int userCount)
        {
            Random rand = new Random();
            
            for (int i = 0; i < userCount; i++)
            {
                var user = new UserViewModel();
                user.Name = $"Имя {i}";
                user.NickName = $"Пользователь {i}";
                user.Surname = $"Фамилия {i}";
                user.Email = $"email{i}@email.com";

                user.ImagePath = "~/images/users/user.png";
                user.Id = i;
                Users.Add(user.Id, user);
            }

            for (int i = 0; i < count; i++)
            {
                var film = new FilmViewModel();

                film.Name = $"Фильм {i}";
                film.Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur in velit lacus. Praesent rutrum convallis sapien, et ultricies eros laoreet quis. Nullam ultrices lectus in leo scelerisque sollicitudin. Etiam dapibus tempus ultricies. Fusce porttitor nisl in nibh fringilla, et pretium lorem posuere. Vestibulum tristique eros eget lectus pharetra, ut laoreet erat volutpat.";
                film.Year = rand.Next(1900, 2020);
                film.ImagePath = "~/images/films/film.png";
                film.Rating = rand.Next(0, 10);
                
                film.Producer = $"Продьюсер {i}";
                var actors = new List<string>();
                var actorsCount = rand.Next(1,6);
                for (int j = 0; j < actorsCount; j++)
                    actors.Add($"Актер {i * 47 + j}");

                film.Actors = actors;

                var genres = new List<string>();
                
                for(int j = 0; j<Genres.Count; j++)
                    if(rand.Next(2) == 1)
                        genres.Add(Genres[j]);

                if(genres.Count == 0)
                    genres.Add(Genres[0]);
                film.Genres = genres;

                var reviews = new List<ReviewViewModel>();
                var reviewsCount = rand.Next(10);
                for (int j = 0; j < reviewsCount; j++)
                {
                    var review = new ReviewViewModel();
                    var userId = rand.Next(Users.Count);
                    review.User = Users[userId];
                    review.Rating = rand.Next(11);
                    review.TimeOfReview = DateTime.Now;
                    review.TimeOfReview = review.TimeOfReview.AddDays(rand.Next(-3, 3));
                    review.TimeOfReview = review.TimeOfReview.AddHours(rand.Next(-5, 5));
                    review.TimeOfReview = review.TimeOfReview.AddMinutes(rand.Next(-30, 30));
                    review.ReviewText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur in velit lacus. Praesent rutrum convallis sapien, et ultricies eros laoreet quis. Nullam ultrices lectus in leo scelerisque sollicitudin. Etiam dapibus tempus ultricies. Fusce porttitor nisl in nibh fringilla, et pretium lorem posuere. Vestibulum tristique eros eget lectus pharetra, ut laoreet erat volutpat.";
                    reviews.Add(review);
                }

                film.Reviews = reviews;

                film.Duration = TimeSpan.FromMinutes(rand.Next(60, 200));

                var schedules = new List<ScheduleViewModel>();

                var scheduleCount = rand.Next(1, 6);
                for (int j = 0; j < scheduleCount; j++)
                {
                    var schedule = new ScheduleViewModel();
                    var hallNumber = rand.Next(1, 5);
                    if (Halls.ContainsKey(hallNumber))
                        schedule.Hall = Halls[hallNumber];
                    else
                    {
                        schedule.Hall = new HallViewModel()
                        {
                            HallNumber = hallNumber,
                            SeatsCount = rand.Next(10, 30),
                            SeatsPerRow = 10
                        };
                        
                        Halls.Add(hallNumber, schedule.Hall);
                    }

                    schedule.PricePerSeat = rand.Next(100, 1000);
                    schedule.Film = film;
                    schedule.Time = DateTime.Now;
                    schedule.Time = schedule.Time.AddDays(rand.Next(-3, 3));
                    schedule.Time = schedule.Time.AddHours(rand.Next(-5, 5));
                    schedule.Time = schedule.Time.AddMinutes(rand.Next(-30, 30));

                    schedule.Id = scheduleIdSequence++;

                    Schedules.Add(schedule.Id, schedule);
                    schedules.Add(schedule);
                }

                film.Schedule = schedules;

                film.Id = i;
                Films.Add(film.Id, film);
            }
            
            for (int i = 0; i < userCount; i++)
            {
                var bookings = new List<BookingViewModel>();
                var bookingCount = rand.Next(10);
                for (int j = 0; j < bookingCount; j++)
                {
                    var booking = new BookingViewModel();
                    booking.User = Users[i];
                    var film = Films[rand.Next(Films.Count)];
                    var schedules = film.Schedule.ToList();
                    booking.Schedule = schedules[rand.Next(schedules.Count)];
                    var seatNumber = rand.Next(booking.Schedule.Hall.SeatsCount);
                    booking.Seat = rand.Next(booking.Schedule.Hall.SeatsCount);
                    booking.BookingCode = "123412341234";

                    booking.Id = bookingsIdSequence++;
                    
                    Bookings.Add(booking.Id, booking);
                    bookings.Add(booking);
                }

                Users[i].Bookings = bookings;
            }
        }
    }
}