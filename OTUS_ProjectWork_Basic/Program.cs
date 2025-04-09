// See https://aka.ms/new-console-template for more information
using OTUS_ProjectWork_Basic.DataBase;
using OTUS_ProjectWork_Basic.Repositories;

Console.WriteLine("Hello, World!");

var context = new CloudReaderContext();

var AuthorRepo = new Repository<Author>(context);
var allAuthors = AuthorRepo.GetAll();
Console.WriteLine("Все авторы: " + string.Join("", allAuthors.Select(x => $"{x.Id} | {x.Fullname}\n")));
