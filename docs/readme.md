# README
## Установка MySql
Следовать инструкции: https://it-black.ru/ustanovka-mysql-workbench/

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
  dotnet ef migrations add <name> -s ..\LiWiMus.Web.MVC\ -o "Data/Migrations"
  ```
* Удаление последней миграции:
  ```
  dotnet ef migrations remove -s ..\LiWiMus.Web.MVC\
  ```
* Обновление бд:
  ```
  dotnet ef database update -s ..\LiWiMus.Web.MVC\
  ```
  *запускать из папки LiWiMus.Infrastructure* 
  
## Подключение через Rider
* Нажать на кнопку справа Database
* Нажать на "+"
* Далее Data Source 
* Затем MySql
* Установить driver
* Пароль, логин: то, что вводили при установке MySql
* База данных: liwimus

## Добавление youtrack integration
* Rider -> File -> Settings -> Plugins
* Скачать Youtrack Integration
* Переходим на сайт youtrack 
* Profile -> Account Security
* Слева снизу new token, название любое, копируете токен
* В rider появится расширение Youtrack, переходим
* Вставляем путь https://timson777-7.youtrack.cloud/, а в токен то, что скопировали
* Подтверждаете