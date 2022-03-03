# README

## Строка подключения
Добавить в папку LiWiMus.Web файл `appsettings.Development.json` со следующим содержимым:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=liwimus;Uid=username;Pwd=password;"
  },
}
```
*заменить **username** и **password** на настоящие*

## Команды
* Добавление миграции:
  ```
  dotnet ef --startup-project ..\LiWiMus.Web\ migrations add <name> -o "Data/Migrations"
  ```
* Удаление последней миграции:
  ```
  dotnet ef --startup-project ..\LiWiMus.Web\ migrations remove
  ```
* Обновление бд:
  ```
  dotnet ef --startup-project ..\LiWiMus.Web\ database update
  ```
  *запускать из папки LiWiMus.Infrastructure* 