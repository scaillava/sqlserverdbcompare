# sqlserverdbcompare
- Small Blazor Project that compares MSSQL Server Databases using google diff algorithm.
- It also has electro framework so you can create a cross platfrom desktop app with it.
- It has 2 comparsions at the moment but its easy to add more.
<br/>

This compares stored procedures and views.
```sql
SELECT Object_name(object_id) as SPECIFIC_NAME ,definition as ROUTINE_DEFINITION FROM sys.sql_modules 
```
The other comparison uses 2 queries to compares tables and constraints.

```sql
SELECT ORDINAL_POSITION, COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, TABLE_NAME, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS

SELECT CONSTRAINT_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE
```
