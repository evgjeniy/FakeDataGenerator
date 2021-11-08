using System;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FakeDataGenerator.Models;
using FakeDataGenerator.Models.Contexts;
using static FakeDataGenerator.Models.Region;

namespace FakeDataGenerator.Controllers
{
    public class HomeController : Controller
    {
        private const int PageSize = 20;
        private readonly List<Record> _records = new(PageSize);
        private readonly Settings _settings = new();

        public HomeController()
        {
            ChangeSettings(By, 5, 123);
        }

        private void AddRecordFromRussia(Random random, double errors)
        {
            using (RuRecordsDbContext db = new RuRecordsDbContext())
            {
                _records.Add(MakeRecordWithErrors(new Record
                {
                    RecordNumber = _records.Count + 1,
                    Id = random.Next(1000, 999999999).ToString(),
                    FullName = db.RuSurnames.Skip(random.Next(0, db.RuSurnames.Count())).FirstOrDefault()?.Surname + (random.Next(0, 2) == 0
                        ? " " + db.RuMaleNames.Skip(random.Next(0, db.RuMaleNames.Count())).FirstOrDefault()?.Name + " " +
                          db.RuMalePatronymics.Skip(random.Next(0, db.RuMalePatronymics.Count())).FirstOrDefault()?.Patronymic
                        : "a " + db.RuFemaleNames.Skip(random.Next(0, db.RuFemaleNames.Count())).FirstOrDefault()?.Name + " " +
                          db.RuFemalePatronymics.Skip(random.Next(0, db.RuFemalePatronymics.Count())).FirstOrDefault()?.Patronymic),
                    Address = "обл. Московская, г. Москва, 3-я ул. Бебеля, д. 34, 127220",
                    Phone = "+7" + random.Next(10000, 99999) + random.Next(10000, 99999)
                }, random, errors));
            }
        }

        private void AddRecordFromUsa(Random random, double errors)
        {
            using (UsaRecordsDbContext db = new UsaRecordsDbContext())
            {
                _records.Add(MakeRecordWithErrors(new Record
                {
                    RecordNumber = _records.Count + 1,
                    Id = random.Next(10000, 99999).ToString(),
                    FullName = (random.Next(0, 2) == 0
                        ? db.UsaMaleNames.Skip(random.Next(0, db.UsaMaleNames.Count())).FirstOrDefault()?.Name
                        : db.UsaFemaleNames.Skip(random.Next(0, db.UsaFemaleNames.Count())).FirstOrDefault()?.Name) + 
                               " " + db.UsaSurnames.Skip(random.Next(0, db.UsaSurnames.Count())).FirstOrDefault()?.Surname,
                    Address = "California, San-Francisco, Fulton st. 635",
                    Phone = "+1" + random.Next(10000, 99999) + random.Next(10000, 99999)
                }, random, errors));
            }
        }

        private void AddRecordFromBelarus(Random random, double errors)
        {
            using (ByRecordsDbContext db = new ByRecordsDbContext())
            {
                _records.Add(MakeRecordWithErrors(new Record
                {
                    RecordNumber = _records.Count + 1,
                    Id = random.Next(1000, 999999999).ToString(),
                    FullName = db.BySurnames.Skip(random.Next(0, db.BySurnames.Count())).FirstOrDefault()?.Surname + " " + (random.Next(0, 2) == 0
                        ? db.ByMaleNames.Skip(random.Next(0, db.ByMaleNames.Count())).FirstOrDefault()?.Name + " " +
                          db.ByMalePatronymics.Skip(random.Next(0, db.ByMalePatronymics.Count())).FirstOrDefault()?.Patronymic
                        : db.ByFemaleNames.Skip(random.Next(0, db.ByFemaleNames.Count())).FirstOrDefault()?.Name + " " +
                          db.ByFemalePatronymics.Skip(random.Next(0, db.ByFemalePatronymics.Count())).FirstOrDefault()?.Patronymic),
                    Address = "обл. Минская, г. Минск, ул. Якуба Коласа, д. 28, 220013",
                    Phone = "+375" + random.Next(1000, 9999) + random.Next(10000, 99999)
                }, random, errors));
            }
        }
        
        private void AddRecordFromUkraine(Random random, double errors)
        {
            using (UaRecordsDbContext db = new UaRecordsDbContext())
            {
                _records.Add(MakeRecordWithErrors(new Record
                {
                    RecordNumber = _records.Count + 1,
                    Id = random.Next(1000, 999999999).ToString(),
                    FullName = db.UaSurnames.Skip(random.Next(0, db.UaSurnames.Count())).FirstOrDefault()?.Surname + (random.Next(0, 2) == 0
                        ? " " + db.UaMaleNames.Skip(random.Next(0, db.UaMaleNames.Count())).FirstOrDefault()?.Name + " " +
                          db.UaMalePatronymics.Skip(random.Next(0, db.UaMalePatronymics.Count())).FirstOrDefault()?.Patronymic
                        : "a " + db.UaFemaleNames.Skip(random.Next(0, db.UaFemaleNames.Count())).FirstOrDefault()?.Name + " " +
                          db.UaFemalePatronymics.Skip(random.Next(0, db.UaFemalePatronymics.Count())).FirstOrDefault()?.Patronymic),
                    Address = "обл. Киевская, г. Киев, вул. Володимирська, д. 32",
                    Phone = "+380" + random.Next(1000, 9999) + random.Next(10000, 99999)
                }, random, errors));
            }
        }

        private Record MakeRecordWithErrors(Record record, Random random, double errors)
        {
            if (errors < 0) errors = 0;
            while (errors-- > 1)
                DoErrorsInRecord(record, random);
            if ((errors - Math.Floor(errors)) * 100 > random.Next(0, 101))
                DoErrorsInRecord(record, random);
            return record;
        }

        private void DoErrorsInRecord(Record record, Random random)
        {
            record.Id = MakeStringWithErrors(new StringBuilder(record.Id), 
                random, _settings.LeftDigit, _settings.RightDigit);
            
            record.FullName = MakeStringWithErrors(new StringBuilder(record.FullName), 
                random, _settings.LeftBorder, _settings.RightBorder);
            
            record.Address = MakeStringWithErrors(new StringBuilder(record.Address), 
                random, _settings.LeftBorder, _settings.RightBorder);
            
            record.Phone =  MakeStringWithErrors(new StringBuilder(record.Phone), 
                random, _settings.LeftDigit, _settings.RightDigit);
        }

        private string MakeStringWithErrors(StringBuilder str, Random random, char fromSymbol, char toSymbol)
        {
            if (str.Length > 1)
            {
                int index = random.Next(0, str.Length);
                switch (random.Next(0, 3))
                {
                    case 0:
                        str.Remove(index, 1);
                        break;
                    case 1:
                        str.Insert(index, (char) random.Next(fromSymbol, toSymbol + 1));
                        break;
                    case 2:
                        int tempIndex = (index > 0 && index < str.Length - 1)
                            ? (random.Next(0, 2) == 0 ? index - 1 : index + 1)
                            : (index == 0 ? 1 : str.Length - 2);
                        (str[index], str[tempIndex]) = (str[tempIndex], str[index]);
                        break;
                }
            }
            else
            {
                str.Insert(0, (char) random.Next(fromSymbol, toSymbol + 1));
            }
            return str.ToString();
        }

        public IActionResult ChangeSettings(Region region, double errorsCount, int seed, int pageNumber = 0)
        {
            _settings.Region = region;
            _settings.ErrorsCount = errorsCount;
            _settings.Random = new Random(seed + pageNumber);

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            switch (_settings.Region)
            {
                case Ru:
                    for (int i = 0; i < PageSize; i++) 
                        AddRecordFromRussia(_settings.Random, _settings.ErrorsCount);
                    break;
                case Usa:
                    for (int i = 0; i < PageSize; i++)
                        AddRecordFromUsa(_settings.Random, _settings.ErrorsCount);
                    break;
                case By:
                    for (int i = 0; i < PageSize; i++)
                        AddRecordFromBelarus(_settings.Random, _settings.ErrorsCount);
                    break;
                case Ua:
                    for (int i = 0; i < PageSize; i++)
                        AddRecordFromUkraine(_settings.Random, _settings.ErrorsCount);
                    break;
                default:
                    for (int i = 0; i < PageSize; i++) 
                        AddRecordFromRussia(_settings.Random, _settings.ErrorsCount);
                    break;
            }
            
            return View(_records);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}