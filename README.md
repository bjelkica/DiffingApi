# DiffingApi
DiffingApi is a Web API for comparing two strings. If the strings don't have the same length or if they are equal then an app returns that. Otherwise if the strings are of 
a same length but are not equal then the app finds differences and returns their offsets and lengths.

## Preparation
At first connect to a local SQL Server __(localdb)\MSSQLLocalDB__ and create a new database
```sql
create database DiffsDB;
```
Then create a table
```sql
create table Diffs (
  Id int not null,
  LeftData nvarchar (max) null,
  RightData nvarchar (max) null
);
```
After that you can build and run an API.

## Usage
To compare two strings first use an Http __PUT__ method to add one string to an endpoint 
[${host}/v1/diff/${id}/left](#put-leftdata) and the other string to an endpoint
[${host}/v1/diff/${id}/right](#put-rightdata). Both endpoints accept JSON with base64 encoded binary data, which can not be null.

The result of the comparison is available through an Http __GET__ method on an endpoint [${host}/v1/diff/${id}](#get-diff). \
If there isn't any data on the left or on the right endpoint then we don't get any result. 
