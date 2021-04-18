# visma-homework
### Welcome to my implementation of Visma homework task - library application.
To open the solution please navigate to ./ConsoleApp1/ and open up ConsoleApp1.sln
To review the source code please navigate to ./ConsoleApp1/ConsoleApp1/

Available commands:
```
list [author <option> | category <option>  | language <option> | isbn <option> | name <option> | date <option> | taken | available] [ascending | descending]
add <isbn> <book_name> <author> <category> <language> <publication_date> 
take <name> <surname> <return_date> <book_isbn>
return <name> <surname> <book_isbn>
delete <book_isbn>
help
quit
```
Or simply type the command (list/add/take/return/delete) to review the instructions.
### Enjoy!
----------------------------
Command examples:
```
list
list language en
list language en ascending
add
add 555 My_Awesome_Book John_Smith Fantasy en 1999-10-11
take
take John Smith 2021-06-20 555
list taken
list available
list available descending
return
return John Smith 555
delete
delete 555
```